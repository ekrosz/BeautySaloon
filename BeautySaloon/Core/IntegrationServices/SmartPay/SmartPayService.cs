using BeautySaloon.Common.Exceptions;
using BeautySaloon.Core.Api.SmartPay;
using BeautySaloon.Core.Api.SmartPay.Dto.CreateInvoice;
using BeautySaloon.Core.Api.SmartPay.Dto.ProcessInvoice;
using BeautySaloon.Core.IntegrationServices.SmartPay.Contracts;
using BeautySaloon.Core.IntegrationServices.SmartPay.Dto;
using BeautySaloon.Core.IntegrationServices.SmartPay.Utils;
using BeautySaloon.Core.Settings;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Repositories.Abstract;
using Microsoft.Extensions.Options;
using QRCoder;

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
            SmartPayConstants.Status.Created => PaymentRequestStatus.InProgress,
            SmartPayConstants.Status.Executed => PaymentRequestStatus.InProgress,
            SmartPayConstants.Status.Confirmed => PaymentRequestStatus.Completed,
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

        async Task<CreatePaymentRequestResponseDto> ProcessInvoiceAsync(string invoiceId, Order order, CancellationToken cancellationToken = default)
        {
            var processInvoiceRequest = MapProcessInvoiceRequest(order);

            var processInvoiceResponse = await _smartPayHttpClient.ProcessInvoiceAsync(invoiceId, processInvoiceRequest, cancellationToken);

            var qrCode = GenerateQrCodeFromUrl(processInvoiceResponse.FormUrl);

            return new CreatePaymentRequestResponseDto
            {
                InvoiceId = invoiceId,
                QrCode = qrCode
            };
        }

        if (!string.IsNullOrEmpty(order.SpInvoiceId))
        {
            return await ProcessInvoiceAsync(order.SpInvoiceId, order, cancellationToken);
        }

        var createInvoiceRequest = MapCreateInvoiceRequest(order);

        var createInvoiceResponse = await _smartPayHttpClient.CreateInvoiceAsync(createInvoiceRequest, cancellationToken);

        if (string.IsNullOrEmpty(createInvoiceResponse.InvoiceId))
        {
            throw new SmartPayApiException(createInvoiceResponse.Error.UserMessage, createInvoiceResponse.Error.ErrorDescription, createInvoiceResponse.Error.ErrorCode);
        }

        return await ProcessInvoiceAsync(createInvoiceResponse.InvoiceId, order, cancellationToken);
    }

    private CreateInvoiceRequestDto MapCreateInvoiceRequest(Order order)
        => new()
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
                    Phone = SmartPayConverter.ToSmartPayPhoneFormat(order.Person.PhoneNumber),
                    Email = order.Person.Email ?? _settings.SmartPaySettings.DefaultEmail,
                    Contact = string.IsNullOrEmpty(order.Person.Email)
                    ? SmartPayConstants.ContactType.PhoneType
                    : SmartPayConstants.ContactType.EmailType
                },
                Order = new CreateInvoiceRequestDto.InvoiceRequestDto.OrderRequestDto
                {
                    OrderId = order.Id,
                    OrderNumber = order.Number.ToString(),
                    OrderDate = SmartPayConverter.ToRfc3999Format(order.CreatedOn),
                    ServiceId = _settings.SmartPaySettings.ServiceId,
                    Amount = SmartPayConverter.ToCentsAmount(order.Cost),
                    Currency = SmartPayConstants.Currency,
                    Purpose = SmartPayConstants.Purpose,
                    Description = SmartPayConstants.DescriptionPrefix + string.Join(", ", order.PersonSubscriptions
                        .GroupBy(x => x.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id)
                        .Select(x => x.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name)),
                    Language = SmartPayConstants.Language,
                    ExpirationDate = DateTime.UtcNow.AddMinutes(SmartPayConstants.ExpireTime),
                    TaxSystem = SmartPayConstants.TaxSystem,
                    OrderBundles = order.PersonSubscriptions
                    .GroupBy(x => x.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id)
                    .Select((x, i) => new CreateInvoiceRequestDto.InvoiceRequestDto.OrderRequestDto.OrderBundleRequestDto
                    {
                        PositionId = i + 1,
                        Name = x.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Name,
                        Quantity = new CreateInvoiceRequestDto.InvoiceRequestDto.OrderRequestDto.OrderBundleRequestDto.QuantityRequestDto
                        {
                            Value = 1,
                            Measure = SmartPayConstants.Measure
                        },
                        ItemAmount = SmartPayConverter.ToCentsAmount(x.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Price),
                        Currency = SmartPayConstants.Currency,
                        ItemCode = x.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id,
                        ItemPrice = SmartPayConverter.ToCentsAmount(x.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Price),
                        TaxType = SmartPayConstants.TaxType,
                        TaxSum = SmartPayConverter.ToCentsAmount(order.Cost * SmartPayConstants.NdsRate / 100)
                    }).ToArray()
                }
            }
        };

    private ProcessInvoiceRequestDto MapProcessInvoiceRequest(Order order)
        => new()
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

    private static byte[] GenerateQrCodeFromUrl(string url)
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
