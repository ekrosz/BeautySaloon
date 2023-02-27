using AutoMapper;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.CosmeticService;
using BeautySaloon.Core.Dto.Responses.CosmeticService;
using BeautySaloon.Core.Exceptions;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using BeautySaloon.DAL.Repositories.Abstract;
using BeautySaloon.DAL.Uow;

namespace BeautySaloon.Core.Services;

public class CosmeticServiceService : ICosmeticServiceService
{
    private readonly IQueryRepository<CosmeticService> _cosmeticServiceQueryRepository;

    private readonly IWriteRepository<CosmeticService> _cosmeticServiceWriteRepository;

    private readonly IUnitOfWork _unitOfWork;

    private readonly IMapper _mapper;

    public CosmeticServiceService(
        IQueryRepository<CosmeticService> cosmeticServiceQueryRepository,
        IWriteRepository<CosmeticService> cosmeticServiceWriteRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _cosmeticServiceQueryRepository = cosmeticServiceQueryRepository;
        _cosmeticServiceWriteRepository = cosmeticServiceWriteRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task CreateCosmeticServiceAsync(CreateCosmeticServiceRequestDto request, CancellationToken cancellationToken = default)
    {
        var isExistName = await _cosmeticServiceQueryRepository.ExistAsync(
            x => x.Name.ToLower().Equals(request.Name.ToLower()),
            cancellationToken);

        if (isExistName)
        {
            throw new EntityAlreadyExistException($"Косметическая услуга {request.Name} уже существует.", typeof(CosmeticService));
        }

        var entity = new CosmeticService(
            request.Name,
            request.Description,
            request.ExecuteTime);

        await _cosmeticServiceWriteRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCosmeticServiceAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _cosmeticServiceWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Косметическая услуга {request.Id} не найдена.", typeof(CosmeticService));

        _cosmeticServiceWriteRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetCosmeticServiceResponseDto> GetCosmeticServiceAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var cosmeticService = await _cosmeticServiceQueryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Косметическая услуга {request.Id} не найдена.", typeof(CosmeticService));

        return _mapper.Map<GetCosmeticServiceResponseDto>(cosmeticService);
    }

    public async Task<PageResponseDto<GetCosmeticServiceResponseDto>> GetCosmeticServiceListAsync(GetCosmeticServiceListRequestDto request, CancellationToken cancellationToken = default)
    {
        var cosmeticServices = await _cosmeticServiceQueryRepository.GetPageAsync(
            request: request.Page,
            x => string.IsNullOrWhiteSpace(request.SearchString)
                || x.Name.ToLower().Contains(request.SearchString.ToLower()),
            sortProperty: x => x.Name,
            cancellationToken: cancellationToken);

        return _mapper.Map<PageResponseDto<GetCosmeticServiceResponseDto>>(cosmeticServices);
    }

    public async Task UpdateCosmeticServiceAsync(ByIdWithDataRequestDto<UpdateCosmeticServiceRequestDto> request, CancellationToken cancellationToken = default)
    {
        var isExistName = await _cosmeticServiceQueryRepository.ExistAsync(
            x => x.Name.ToLower().Equals(request.Data.Name.ToLower()) && x.Id != request.Id,
            cancellationToken);

        if (isExistName)
        {
            throw new EntityAlreadyExistException($"Косметическая услуга {request.Data.Name} уже существует.", typeof(CosmeticService));
        }

        var entity = await _cosmeticServiceWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new EntityNotFoundException($"Косметическая услуга {request.Id} не найдена.", typeof(CosmeticService));

        entity.Update(
            request.Data.Name,
            request.Data.Description,
            request.Data.ExecuteTime);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
