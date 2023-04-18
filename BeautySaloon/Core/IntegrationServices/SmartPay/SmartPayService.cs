using BeautySaloon.Common.Exceptions;
using BeautySaloon.Core.Api.SmartPay;
using BeautySaloon.Core.Api.SmartPay.Dto.CreateInvoice;
using BeautySaloon.Core.Api.SmartPay.Dto.ProcessInvoice;
using BeautySaloon.Core.IntegrationServices.SmartPay.Contracts;
using BeautySaloon.Core.IntegrationServices.SmartPay.Dto;
using BeautySaloon.Core.Settings;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;
using Microsoft.Extensions.Options;
using QRCoder;
using Refit;

namespace BeautySaloon.Core.IntegrationServices.SmartPay;

public class SmartPayService : ISmartPayService
{
    private readonly IWriteRepository<Order> _orderWriteRepository;

    private readonly ISmartPayHttpClient _smartPayHttpClient;

    private readonly BLayerSettings _settings;

    public SmartPayService(
        IWriteRepository<Order> orderWriteRepository,
        ISmartPayHttpClient smartPayHttpClient,
        IOptionsSnapshot<BLayerSettings> settings)
    {
        _orderWriteRepository = orderWriteRepository;
        _smartPayHttpClient = smartPayHttpClient;
        _settings = settings.Value;
    }

    public async Task<GetPaymentRequestResponseDto> GetPaymentRequestStatusAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderWriteRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new OrderNotFoundException(orderId);

        if (string.IsNullOrEmpty(order.SpInvoiceId))
        {
            throw new OrderInvoiceIsEmptyException();
        }

        var getInvoiceResponse = await _smartPayHttpClient.GetInvoiceAsync(order.SpInvoiceId, cancellationToken);

        var status = getInvoiceResponse.InvoiceStatus switch
        {
            "created" => PaymentRequestStatus.InProgress,
            "executed" => PaymentRequestStatus.InProgress,
            "confirmed" => PaymentRequestStatus.Completed,
            _ => PaymentRequestStatus.Failed
        };

        return new GetPaymentRequestResponseDto
        {
            PaymentStatus = status
        };
    }

    public async Task<CreatePaymentRequestResponseDto> CreatePaymentRequestAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderWriteRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new OrderNotFoundException(orderId);

        var createInvoiceRequest = MapCreateInvoiceRequest(order);

        var createInvoiceResponse = await _smartPayHttpClient.CreateInvoiceAsync(createInvoiceRequest, cancellationToken);

        if (string.IsNullOrEmpty(createInvoiceResponse.InvoiceId))
        {
            throw new SmartPayApiException(createInvoiceResponse.Error.UserMessage, createInvoiceResponse.Error.ErrorDescription, createInvoiceResponse.Error.ErrorCode);
        }

        var processInvoiceRequest = MapProcessInvoiceRequest(order);

        var processInvoiceResponse = await _smartPayHttpClient.ProcessInvoiceAsync(createInvoiceResponse.InvoiceId, processInvoiceRequest, cancellationToken);

        var qrCode = GenerateQrCodeFromUrl(processInvoiceResponse.FormUrl);

        return new CreatePaymentRequestResponseDto
        {
            InvoiceId = createInvoiceResponse.InvoiceId,
            QrCode = qrCode
        };
    }

    private CreateInvoiceRequestDto MapCreateInvoiceRequest(Order order)
        => new CreateInvoiceRequestDto
        {
            Type = SmartPayConstants.Type,
            UserId = new CreateInvoiceRequestDto.UserRequestDto
            {
                PartnerClientId = order.Person.Id
            },
            Invoice = new CreateInvoiceRequestDto.InvoiceRequestDto
            {
                Purchaser = new CreateInvoiceRequestDto.InvoiceRequestDto.PurchaserRequestDto
                {
                    Phone = order.Person.PhoneNumber[2..],
                    Email = order.Person.Email,
                    Contact = string.IsNullOrEmpty(order.Person.Email)
                    ? SmartPayConstants.ContactType.PhoneType
                    : SmartPayConstants.ContactType.EmailType
                },
                Order = new CreateInvoiceRequestDto.InvoiceRequestDto.OrderRequestDto
                {
                    OrderId = order.Id,
                    OrderNumber = order.Number.ToString(),
                    OrderDate = order.CreatedOn.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss''K"),
                    ServiceId = _settings.SmartPaySettings.ServiceId,
                    Amount = Convert.ToInt32(order.Cost * 100),
                    Currency = SmartPayConstants.Currency,
                    Purpose = SmartPayConstants.Purpose,
                    Description = SmartPayConstants.DescriptionPrefix + string.Join(", ", order.PersonSubscriptions
                        .GroupBy(x => x.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id)
                        .Select(x => x.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name)),
                    Language = SmartPayConstants.Language,
                    ExpirationDate = DateTime.Now.AddMinutes(20),
                    TaxSystem = SmartPayConstants.TaxSystem,
                    OrderBundles = order.PersonSubscriptions
                    .GroupBy(x => x.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id)
                    .Select((x, i) => new CreateInvoiceRequestDto.InvoiceRequestDto.OrderRequestDto.OrderBundleRequestDto
                    {
                        PositionId = i,
                        Name = x.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                        Quantity = new CreateInvoiceRequestDto.InvoiceRequestDto.OrderRequestDto.OrderBundleRequestDto.QuantityRequestDto
                        {
                            Value = 1,
                            Measure = SmartPayConstants.Measure
                        },
                        ItemAmount = Convert.ToInt32(x.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Price * 100),
                        Currency = SmartPayConstants.Currency,
                        ItemCode = x.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id,
                        ItemPrice = Convert.ToInt32(x.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Price * 100),
                        TaxType = SmartPayConstants.TaxType,
                        TaxSum = Convert.ToInt32(order.Cost * 0.2m * 100)
                    }).ToArray()
                }
            }
        };

    private ProcessInvoiceRequestDto MapProcessInvoiceRequest(Order order)
        => new ProcessInvoiceRequestDto
        {
            UserId = new ProcessInvoiceRequestDto.UserRequestDto
            {
                PartnerClientId = order.Person.Id
            },
            Operations = new[]
            {
                new ProcessInvoiceRequestDto.OperationRequestDto
                {
                    Operation = SmartPayConstants.OperationType,
                    Code = SmartPayConstants.OperationCode,
                    Value = _settings.SmartPaySettings.ServiceId
                }
            }
        };

    private byte[] GenerateQrCodeFromUrl(string url)
    {
        var qrCodeData = new QRCodeGenerator().CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        var qrCodeImage = new QRCode(qrCodeData).GetGraphic(20);

        using (var stream = new MemoryStream())
        {
            qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

            return stream.ToArray();
        }
    }
}
