using AutoMapper;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.Subscription;
using BeautySaloon.Core.Dto.Responses.Subscription;
using BeautySaloon.Core.Exceptions;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;

namespace BeautySaloon.Core.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly IQueryRepository<Subscription> _subscriptionQueryRepository;

    private readonly IQueryRepository<CosmeticService> _cosmeticServiceQueryRepository;

    private readonly IWriteRepository<Subscription> _subscriptionWriteRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public SubscriptionService(
        IQueryRepository<Subscription> subscriptionQueryRepository,
        IQueryRepository<CosmeticService> cosmeticServiceQueryRepository,
        IWriteRepository<Subscription> subscriptionWriteRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _subscriptionQueryRepository = subscriptionQueryRepository;
        _cosmeticServiceQueryRepository = cosmeticServiceQueryRepository;
        _subscriptionWriteRepository = subscriptionWriteRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateSubscriptionAsync(CreateSubscriptionRequestDto request, CancellationToken cancellationToken = default)
    {
        var isExistName = await _subscriptionQueryRepository.ExistAsync(
            x => x.Name.ToLower().Contains(request.Name.ToLower()),
            cancellationToken);

        if (isExistName)
        {
            throw new EntityAlreadyExistException($"Абонемент {request.Name} уже существует.", typeof(Subscription));
        }

        var cosmeticServices = await _cosmeticServiceQueryRepository.FindAsync(
            x => request.CosmeticServices.Any(y => y.Id == x.Id),
            cancellationToken);

        var notExistCosmeticServices = request.CosmeticServices
            .ExceptBy(cosmeticServices.Select(x => x.Id), x => x.Id);

        if (notExistCosmeticServices.Any())
        {
            throw new EntityNotFoundException($"Абонимент {notExistCosmeticServices.First()} не найден.", typeof(Subscription));
        }

        var entity = new Subscription(
            request.Name,
            request.Price,
            request.LifeTime);

        var subscriptionCosmeticServices = request.CosmeticServices
            .Select(x => new SubscriptionCosmeticService(x.Id, x.Count))
            .ToArray();

        entity.AddCosmeticServices(subscriptionCosmeticServices);

        await _subscriptionWriteRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSubscriptionAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _subscriptionWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Абонемент {request.Id} не найден.", typeof(Subscription));

        _subscriptionWriteRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetSubscriptionResponseDto> GetSubscriptionAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var subscription = await _subscriptionQueryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Абонемент {request.Id} не найден.", typeof(Subscription));

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
            ?? throw new EntityNotFoundException($"Абонемент {request.Id} не найден.", typeof(Subscription));

        var isExistName = await _subscriptionQueryRepository.ExistAsync(
            x => x.Name.ToLower().Contains(request.Data.Name.ToLower()) && x.Id != request.Id,
            cancellationToken);

        if (isExistName)
        {
            throw new EntityAlreadyExistException($"Абонемент {request.Data.Name} уже существует.", typeof(Subscription));
        }

        var cosmeticServices = await _cosmeticServiceQueryRepository.FindAsync(
            x => request.Data.CosmeticServices.Any(y => y.Id == x.Id),
            cancellationToken);

        var notExistCosmeticServices = request.Data.CosmeticServices
            .ExceptBy(cosmeticServices.Select(x => x.Id), x => x.Id);

        if (notExistCosmeticServices.Any())
        {
            throw new EntityNotFoundException($"Абонимент {notExistCosmeticServices.First()} не найден.", typeof(Subscription));
        }

        entity.Update(
            request.Data.Name,
            request.Data.Price,
            request.Data.LifeTime);

        var subscriptionCosmeticServices = request.Data.CosmeticServices
            .Select(x => new SubscriptionCosmeticService(x.Id, x.Count))
            .ToArray();

        entity.AddCosmeticServices(subscriptionCosmeticServices);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
