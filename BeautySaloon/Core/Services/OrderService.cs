﻿using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Order;
using BeautySaloon.Api.Dto.Responses.Order;
using AutoMapper;
using BeautySaloon.Common.Exceptions;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;
using BeautySaloon.Core.IntegrationServices.SmartPay.Contracts;
using BeautySaloon.Core.IntegrationServices.SmartPay.Dto;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.Core.Utils.Dto;
using BeautySaloon.Core.Utils.Contracts;
using BeautySaloon.Core.IntegrationServices.MailKit.Contracts;
using BeautySaloon.Core.IntegrationServices.MailKit.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BeautySaloon.Core.Services;
public class OrderService : IOrderService
{
    private readonly IWriteRepository<Order> _orderWriteRepository;

    private readonly IWriteRepository<Person> _personWriteRepository;

    private readonly IQueryRepository<Order> _orderQueryRepository;

    private readonly IQueryRepository<Subscription> _subscriptionQueryRepository;

    private readonly ISmartPayService _smartPayService;

    private readonly IMailKitService _mailKitService;

    private readonly IDocumentGenerator<ReceiptRequestDto> _receiptDocumentGenerator;

    private readonly IDocumentGenerator<OrderReportRequestDto> _orderReportDocumentGenerator;

    private readonly IForecastService _forecastService;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public OrderService(
        IWriteRepository<Order> orderWriteRepository,
        IWriteRepository<Person> personWriteRepository,
        IQueryRepository<Order> orderQueryRepository,
        IQueryRepository<Subscription> subscriptionQueryRepository,
        ISmartPayService smartPayService,
        IMailKitService mailKitService,
        IDocumentGenerator<ReceiptRequestDto> receiptDocumentGenerator,
        IDocumentGenerator<OrderReportRequestDto> orderReportDocumentGenerator,
        IForecastService forecastService,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _orderWriteRepository = orderWriteRepository;
        _personWriteRepository = personWriteRepository;
        _orderQueryRepository = orderQueryRepository;
        _subscriptionQueryRepository = subscriptionQueryRepository;
        _smartPayService = smartPayService;
        _mailKitService = mailKitService;
        _receiptDocumentGenerator = receiptDocumentGenerator;
        _orderReportDocumentGenerator = orderReportDocumentGenerator;
        _forecastService = forecastService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        var person = await _personWriteRepository.GetByIdAsync(request.PersonId, cancellationToken)
            ?? throw new PersonNotFoundException(request.PersonId);

        var subscriptions = await _subscriptionQueryRepository.FindAsync(
            x => request.SubscriptionIds.Contains(x.Id),
            cancellationToken);

        var notExistSubscriptions = request.SubscriptionIds
            .Except(subscriptions.Select(x => x.Id))
            .ToArray();

        if (notExistSubscriptions.Any())
        {
            throw new SubscriptionNotFoundException(notExistSubscriptions.First());
        }

        var order = new Order(subscriptions.Sum(x => x.Price), request.Comment);

        var personSubscriptions = subscriptions.SelectMany(x => x.SubscriptionCosmeticServices)
            .SelectMany(x => Enumerable.Range(0, x.Count).Select(_ => new PersonSubscription(
                new SubscriptionCosmeticServiceSnapshot
                {
                    SubscriptionSnapshot = _mapper.Map<SubscriptionSnapshot>(x.Subscription),
                    CosmeticServiceSnapshot = _mapper.Map<CosmeticServiceSnapshot>(x.CosmeticService),
                    Count = x.Count
                })))
            .ToArray();

        order.AddPersonSubscriptions(personSubscriptions);
        person.AddOrder(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateOrderAsync(ByIdWithDataRequestDto<UpdateOrderRequestDto> request, CancellationToken cancellationToken = default)
    {
        var order = await _orderWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new OrderNotFoundException(request.Id);

        var subscriptions = await _subscriptionQueryRepository.FindAsync(
            x => request.Data.SubscriptionIds.Contains(x.Id),
            cancellationToken);

        var notExistSubscriptions = request.Data.SubscriptionIds
            .Except(subscriptions.Select(x => x.Id))
            .ToArray();

        if (notExistSubscriptions.Any())
        {
            throw new SubscriptionNotFoundException(notExistSubscriptions.First());
        }

        var personSubscriptions = subscriptions.SelectMany(x => x.SubscriptionCosmeticServices)
            .Select(x => new PersonSubscription(
                new SubscriptionCosmeticServiceSnapshot
                {
                    SubscriptionSnapshot = _mapper.Map<SubscriptionSnapshot>(x.Subscription),
                    CosmeticServiceSnapshot = _mapper.Map<CosmeticServiceSnapshot>(x.CosmeticService),
                    Count = x.Count
                }))
            .ToArray();

        order.Update(subscriptions.Sum(x => x.Price), request.Data.Comment);
        order.AddPersonSubscriptions(personSubscriptions);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<PayOrderResponseDto> PayOrderAsync(ByIdWithDataRequestDto<PayOrderRequestDto> request, CancellationToken cancellationToken = default)
    {
        var order = await _orderWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new OrderNotFoundException(request.Id);

        if (request.Data.PaymentMethod == PaymentMethod.Card)
        {
            var smartPayResponse = await _smartPayService.CreatePaymentRequestAsync(request.Id, cancellationToken);

            return new PayOrderResponseDto { QrCode = new FileResponseDto { Data = smartPayResponse.QrCode } };
        }

        order.Pay(request.Data.PaymentMethod, request.Data.Comment, null);

        await SendReceiptToEmailAsync(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new();
    }

    public async Task CancelOrderAsync(ByIdWithDataRequestDto<CancelOrderRequestDto> request, CancellationToken cancellationToken = default)
    {
        var order = await _orderWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new OrderNotFoundException(request.Id);

        order.Cancel(request.Data.Comment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<PageResponseDto<GetOrderResponseDto>> GetOrderListAsync(GetOrderListRequestDto request, CancellationToken cancellationToken = default)
    {
        var orders = await _orderQueryRepository.GetPageAsync(
            request: request.Page,
            predicate: x => (string.IsNullOrWhiteSpace(request.SearchString)
                        || string.Join(' ', x.Person.Name.LastName, x.Person.Name.FirstName, x.Person.Name.MiddleName).TrimEnd(' ').ToLower().Contains(request.SearchString.ToLower())
                        || x.Person.PhoneNumber.ToLower().Contains(request.SearchString))
                    && ((!request.StartCreatedOn.HasValue || x.CreatedOn.Date >= request.StartCreatedOn.Value.Date)
                        && (!request.EndCreatedOn.HasValue || x.CreatedOn.Date <= request.EndCreatedOn.Value.Date)),
            sortProperty: x => x.UpdatedOn,
            asc: false,
            cancellationToken);

        return _mapper.Map<PageResponseDto<GetOrderResponseDto>>(orders);
    }

    public async Task<GetOrderResponseDto> GetOrderAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var order = await _orderQueryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new OrderNotFoundException(request.Id);

        return _mapper.Map<GetOrderResponseDto>(order);
    }

    public async Task<CheckAndUpdateOrderPaymentStatusResponseDto> CheckAndUpdateOrderPaymentStatusAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var order = await _orderWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new OrderNotFoundException(request.Id);

        var smartPayResponse = await _smartPayService.GetPaymentRequestStatusAsync(order.Id, cancellationToken);

        switch (smartPayResponse.PaymentStatus)
        {
            case PaymentRequestStatus.Completed:
                order.Pay(PaymentMethod.Card, null, order.SpInvoiceId);
                await SendReceiptToEmailAsync(order);
                break;
            case PaymentRequestStatus.InProgress:
                break;
            case PaymentRequestStatus.Failed:
                order.Cancel("Произошла ошибка при оплате заказа.");
                break;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CheckAndUpdateOrderPaymentStatusResponseDto { PaymentStatus = order.PaymentStatus };
    }

    public async Task<FileResponseDto> GetOrderReceiptAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var order = await _orderQueryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new OrderNotFoundException(request.Id);

        if (order.PaymentStatus != PaymentStatus.Paid)
        {
            throw new OrderNotPaidException();
        }

        return await GenerateReceiptAsync(order);
    }

    public async Task<FileResponseDto> GetOrderReportAsync(GetOrderReportRequestDto request, CancellationToken cancellationToken = default)
    {
        var orders = await _orderQueryRepository.FindAsync(
            x => (!request.StartCreatedOn.HasValue || request.StartCreatedOn.Value.Date <= x.CreatedOn.Date) && (!request.EndCreatedOn.HasValue || request.EndCreatedOn.Value.Date >= x.CreatedOn.Date),
            cancellationToken);

        var data = new OrderReportRequestDto
        {
            StartCreatedOn = request.StartCreatedOn ?? orders.Min(x => x.CreatedOn),
            EndCreatedOn = request.EndCreatedOn ?? orders.Max(x => x.CreatedOn),
            TotalCost = orders.Sum(x => x.Cost),
            Items = _mapper.Map<IReadOnlyCollection<OrderReportRequestDto.OrderItem>>(orders)
                .OrderByDescending(x => x.CreatedOn)
                .ToArray()
        };

        return await _orderReportDocumentGenerator.GenerateDocumentAsync($"Отчет", data);
    }

    public async Task<GetOrderAnalyticResponseDto> GetOrderAnalyticAsync(GetOrderAnalyticRequestDto request, CancellationToken cancellationToken = default)
    {
        var orders = (await _orderQueryRepository.GetQuery()
            .Where(x => x.CreatedOn.Year == request.Year && !x.PersonSubscriptions.Any(y => y.Status == PersonSubscriptionCosmeticServiceStatus.NotPaid || y.Status == PersonSubscriptionCosmeticServiceStatus.Cancelled))
            .ToArrayAsync(cancellationToken))
            .GroupBy(x => x.CreatedOn.Month)
            .Select(x => new GetOrderAnalyticResponseDto.GetOrderAnalyticItemResponseDto
            {
                Period = new DateTime(x.First().CreatedOn.Year, x.Key, 1),
                Count = x.Count(),
                Revenues = x.Sum(y => y.Cost)
            }).OrderBy(x => x.Period)
            .ToArray();

        var result = Enumerable.Range(1, 12)
            .Select(i =>
            {
                var date = DateTime.UtcNow.AddMonths(-12).AddMonths(i).Date;

                var period = new DateTime(date.Year, date.Month, 1);

                return new GetOrderAnalyticResponseDto.GetOrderAnalyticItemResponseDto
                {
                    Period = period,
                    Revenues = orders.Where(x => period == x.Period).Sum(x => x.Revenues),
                    Count = orders.Where(x => period == x.Period).Sum(x => x.Count)
                };
            }).ToArray();

        return new GetOrderAnalyticResponseDto
        {
            Items = result,
            ForecastItems = GetOrderForecastAnalytic(result),
            TotalRevenues = orders.Sum(x => x.Revenues),
            TotalCount = orders.Sum(x => x.Count)
        };
    }

    private async Task SendReceiptToEmailAsync(Order order)
    {
        if (string.IsNullOrEmpty(order.Person.Email))
        {
            return;
        }

        var file = await GenerateReceiptAsync(order);

        await _mailKitService.SendEmailAsync(new SendEmailRequestDto
        {
            ReceiverName = order.Person.Name.ConcatedName,
            ReceiverEmail = order.Person.Email,
            Subject = $"Заказ {order.Number} успешно оплачен.",
            Body = $"Благодарим Вас за покупку абонементов в студии красоты Beauty Studio.",
            Files = new[] { file }
        });
    }

    private Task<FileResponseDto> GenerateReceiptAsync(Order order)
    {
        var data = _mapper.Map<ReceiptRequestDto>(order);

        return _receiptDocumentGenerator.GenerateDocumentAsync($"Заказ-{order.Number}", data);
    }

    private IReadOnlyCollection<GetOrderAnalyticResponseDto.GetOrderAnalyticItemResponseDto> GetOrderForecastAnalytic(IReadOnlyCollection<GetOrderAnalyticResponseDto.GetOrderAnalyticItemResponseDto> source)
    {
        var forecastCountRequest = source.Select(x => new ForecastDto { Period = x.Period, Value = x.Count }).ToArray();
        var forecastRevenuesRequest = source.Select(x => new ForecastDto { Period = x.Period, Value = Convert.ToDouble(x.Revenues) }).ToArray();

        var forecastCountResult = _forecastService.GetForecast(forecastCountRequest, 3);
        var forecastRevenuesResult = _forecastService.GetForecast(forecastRevenuesRequest, 3);

        return forecastCountResult.Join(forecastRevenuesResult, x => x.Period, x => x.Period, (x, y) => new GetOrderAnalyticResponseDto.GetOrderAnalyticItemResponseDto
        {
            Period = x.Period,
            Count = Convert.ToInt32(Math.Round(x.Value)),
            Revenues = Convert.ToDecimal(y.Value)
        }).ToArray();
    }
}
