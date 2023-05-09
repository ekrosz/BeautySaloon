using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Subscription;
using BeautySaloon.Api.Dto.Responses.Subscription;
using AutoMapper;
using BeautySaloon.Common.Exceptions;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;
using BeautySaloon.DAL.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Core.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IQueryRepository<Subscription> _subscriptionQueryRepository;

    private readonly IQueryRepository<CosmeticService> _cosmeticServiceQueryRepository;

    private readonly IQueryRepository<Order> _orderQueryRepository;

    private readonly IWriteRepository<Subscription> _subscriptionWriteRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public SubscriptionService(
        IQueryRepository<Subscription> subscriptionQueryRepository,
        IQueryRepository<CosmeticService> cosmeticServiceQueryRepository,
        IQueryRepository<Order> orderQueryRepository,
        IWriteRepository<Subscription> subscriptionWriteRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _subscriptionQueryRepository = subscriptionQueryRepository;
        _cosmeticServiceQueryRepository = cosmeticServiceQueryRepository;
        _orderQueryRepository = orderQueryRepository;
        _subscriptionWriteRepository = subscriptionWriteRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateSubscriptionAsync(CreateSubscriptionRequestDto request, CancellationToken cancellationToken = default)
    {
        var isExistName = await _subscriptionQueryRepository.ExistAsync(
            x => x.Name.ToLower().Equals(request.Name.ToLower()),
            cancellationToken);

        if (isExistName)
        {
            throw new SubscriptionAlreadyExistException(nameof(request.Name), request.Name);
        }

        var cosmeticServices = await _cosmeticServiceQueryRepository.FindAsync(
            x => request.CosmeticServices.Select(s => s.Id).Contains(x.Id),
            cancellationToken);

        var notExistCosmeticServices = request.CosmeticServices
            .ExceptBy(cosmeticServices.Select(x => x.Id), x => x.Id);

        if (notExistCosmeticServices.Any())
        {
            throw new CosmeticServiceNotFoundException(notExistCosmeticServices.First().Id);
        }

        var entity = new Subscription(
            request.Name,
            request.Price,
            request.LifeTimeInDays)
        { Id = Guid.NewGuid() };

        var subscriptionCosmeticServices = request.CosmeticServices
            .Select(x => new SubscriptionCosmeticService(x.Id, x.Count) { Id = Guid.NewGuid() })
            .ToArray();

        entity.AddCosmeticServices(subscriptionCosmeticServices);

        await _subscriptionWriteRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSubscriptionAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _subscriptionWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new SubscriptionNotFoundException(request.Id);

        _subscriptionWriteRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetSubscriptionResponseDto> GetSubscriptionAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionQueryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new SubscriptionNotFoundException(request.Id);

        return _mapper.Map<GetSubscriptionResponseDto>(subscription);
    }

    public async Task<PageResponseDto<GetSubscriptionListItemResponseDto>> GetSubscriptionListAsync(GetSubscriptionListRequestDto request, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _subscriptionQueryRepository.GetPageAsync(
            request: request.Page,
            predicate: x => string.IsNullOrWhiteSpace(request.SearchString)
                || x.Name.ToLower().Contains(request.SearchString.ToLower()),
            sortProperty: x => x.Name,
            cancellationToken: cancellationToken);

        return _mapper.Map<PageResponseDto<GetSubscriptionListItemResponseDto>>(subscriptions);
    }

    public async Task UpdateSubscriptionAsync(ByIdWithDataRequestDto<UpdateSubscriptionRequestDto> request, CancellationToken cancellationToken = default)
    {
        var entity = await _subscriptionWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new SubscriptionNotFoundException(request.Id);

        var isExistName = await _subscriptionQueryRepository.ExistAsync(
            x => x.Name.ToLower().Contains(request.Data.Name.ToLower()) && x.Id != request.Id,
            cancellationToken);

        if (isExistName)
        {
            throw new SubscriptionAlreadyExistException(nameof(request.Data.Name), request.Data.Name);
        }

        var cosmeticServices = await _cosmeticServiceQueryRepository.FindAsync(
            x => request.Data.CosmeticServices.Select(y => y.Id).Contains(x.Id),
            cancellationToken);

        var notExistCosmeticServices = request.Data.CosmeticServices
            .ExceptBy(cosmeticServices.Select(x => x.Id), x => x.Id);

        if (notExistCosmeticServices.Any())
        {
            throw new CosmeticServiceNotFoundException(notExistCosmeticServices.First().Id);
        }

        entity.Update(
            request.Data.Name,
            request.Data.Price,
            request.Data.LifeTimeInDays);

        var subscriptionCosmeticServices = request.Data.CosmeticServices
            .Select(x => new SubscriptionCosmeticService(x.Id, x.Count))
            .ToArray();

        entity.AddCosmeticServices(subscriptionCosmeticServices);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetSubscriptionAnalyticResponseDto> GetSubscriptionAnalyticAsync(GetSubscriptionAnalyticRequestDto request, CancellationToken cancellationToken = default)
    {
        var subscriptions = (await _orderQueryRepository.GetQuery()
            .Where(x => (!request.StartCreatedOn.HasValue || request.StartCreatedOn.Value.Date <= x.CreatedOn.Date)
                && (!request.EndCreatedOn.HasValue || request.EndCreatedOn.Value.Date >= x.CreatedOn.Date)
                && !x.PersonSubscriptions.Any(y => y.Status == PersonSubscriptionCosmeticServiceStatus.Cancelled || y.Status == PersonSubscriptionCosmeticServiceStatus.NotPaid))
            .ToArrayAsync(cancellationToken))
            .SelectMany(x => x.PersonSubscriptions.GroupBy(y => y.SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot.Id).Select(z => z.First().SubscriptionCosmeticServiceSnapshot.SubscriptionSnapshot))
            .GroupBy(x => x.Name)
            .Select(x => new GetSubscriptionAnalyticResponseDto.GetSubscriptionAnalyticItemResponseDto
            {
                SubscriptionName = x.First().Name,
                Count = x.Count()
            }).OrderBy(x => x.SubscriptionName)
            .ToArray();

        return new GetSubscriptionAnalyticResponseDto
        {
            Items = subscriptions,
            TotalCount = subscriptions.Sum(x => x.Count)
        };
    }
}
