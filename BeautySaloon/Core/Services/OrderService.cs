﻿using AutoMapper;
using BeautySaloon.Common.Exceptions;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.Order;
using BeautySaloon.Core.Dto.Responses.Order;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;

namespace BeautySaloon.Core.Services;
public class OrderService : IOrderService
{
    private readonly IWriteRepository<Order> _orderWriteRepository;

    private readonly IWriteRepository<Person> _personWriteRepository;

    private readonly IQueryRepository<Order> _orderQueryRepository;

    private readonly IQueryRepository<Subscription> _subscriptionQueryRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public OrderService(
        IWriteRepository<Order> orderWriteRepository,
        IWriteRepository<Person> personWriteRepository,
        IQueryRepository<Order> orderQueryRepository,
        IQueryRepository<Subscription> subscriptionQueryRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _orderWriteRepository = orderWriteRepository;
        _personWriteRepository = personWriteRepository;
        _orderQueryRepository = orderQueryRepository;
        _subscriptionQueryRepository = subscriptionQueryRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateOrderAsync(CreateOrderRequestDto request, CancellationToken cancellationToken = default)
    {
        var person = await _personWriteRepository.GetByIdAsync(request.PersonId, cancellationToken)
            ?? throw new EntityNotFoundException($"Клиент {request.PersonId} не найден.", typeof(Person));

        var subscriptions = await _subscriptionQueryRepository.FindAsync(
            x => request.SubscriptionIds.Contains(x.Id),
            cancellationToken);

        var notExistSubscriptions = request.SubscriptionIds
            .Except(subscriptions.Select(x => x.Id))
            .ToArray();

        if (notExistSubscriptions.Any())
        {
            throw new EntityNotFoundException($"Абонемент {notExistSubscriptions.First()} не найден.", typeof(Subscription));
        }

        var order = new Order(subscriptions.Sum(x => x.Price));

        var personSubscriptions = subscriptions.SelectMany(x => x.SubscriptionCosmeticServices)
            .Select(x => new PersonSubscription(x.Id))
            .ToArray();

        order.AddPersonSubscriptions(personSubscriptions);
        person.AddOrder(order);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateOrderAsync(ByIdWithDataRequestDto<UpdateOrderRequestDto> request, CancellationToken cancellationToken = default)
    {
        var order = await _orderWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Заказ {request.Id} не найден.", typeof(Order));

        var subscriptions = await _subscriptionQueryRepository.FindAsync(
            x => request.Data.SubscriptionIds.Contains(x.Id),
            cancellationToken);

        var notExistSubscriptions = request.Data.SubscriptionIds
            .Except(subscriptions.Select(x => x.Id))
            .ToArray();

        if (notExistSubscriptions.Any())
        {
            throw new EntityNotFoundException($"Абонемент {notExistSubscriptions.First()} не найден.", typeof(Subscription));
        }

        var personSubscriptions = subscriptions.SelectMany(x => x.SubscriptionCosmeticServices)
            .Select(x => new PersonSubscription(x.Id))
            .ToArray();

        order.AddPersonSubscriptions(personSubscriptions);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task PayOrderAsync(ByIdWithDataRequestDto<PayOrderRequestDto> request, CancellationToken cancellationToken = default)
    {
        var order = await _orderWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Заказ {request.Id} не найден.", typeof(Order));

        order.Pay(request.Data.PaymentMethod, request.Data.Comment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task CancelOrderAsync(ByIdWithDataRequestDto<CancelOrderRequestDto> request, CancellationToken cancellationToken = default)
    {
        var order = await _orderWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Заказ {request.Id} не найден.", typeof(Order));

        order.Cancel(request.Data.Comment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<PageResponseDto<GetOrderResponseDto>> GetOrderListAsync(GetOrderListRequestDto request, CancellationToken cancellationToken = default)
    {
        var orders = await _orderQueryRepository.GetPageAsync(
            request: request.Page,
            predicate: x => x.PersonId == request.PersonId
                    && (string.IsNullOrWhiteSpace(request.SearchString)
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
            ?? throw new EntityNotFoundException($"Заказ {request.Id} не найден.", typeof(Order));

        return _mapper.Map<GetOrderResponseDto>(order);
    }
}