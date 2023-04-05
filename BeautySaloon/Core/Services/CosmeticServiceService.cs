using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.CosmeticService;
using BeautySaloon.Api.Dto.Responses.CosmeticService;
using AutoMapper;
using BeautySaloon.Common.Exceptions;
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
            throw new CosmeticServiceAlreadyExistException(nameof(request.Name), request.Name);
        }

        var entity = new CosmeticService(
            request.Name,
            request.ExecuteTimeInMinutes,
            request.Description);

        await _cosmeticServiceWriteRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCosmeticServiceAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var entity = await _cosmeticServiceWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new CosmeticServiceNotFoundException(request.Id);

        _cosmeticServiceWriteRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<GetCosmeticServiceResponseDto> GetCosmeticServiceAsync(ByIdRequestDto request, CancellationToken cancellationToken = default)
    {
        var cosmeticService = await _cosmeticServiceQueryRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new CosmeticServiceNotFoundException(request.Id);

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
            throw new CosmeticServiceAlreadyExistException(nameof(request.Data.Name), request.Data.Name);
        }

        var entity = await _cosmeticServiceWriteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new CosmeticServiceNotFoundException(request.Id);

        entity.Update(
            request.Data.Name,
            request.Data.ExecuteTimeInMinutes,
            request.Data.Description);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
