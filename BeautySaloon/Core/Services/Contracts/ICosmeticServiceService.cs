using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.CosmeticService;
using BeautySaloon.Api.Dto.Responses.CosmeticService;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Services.Contracts;

public interface ICosmeticServiceService
{
    Task CreateCosmeticServiceAsync(CreateCosmeticServiceRequestDto request, CancellationToken cancellationToken = default);

    Task UpdateCosmeticServiceAsync(ByIdWithDataRequestDto<UpdateCosmeticServiceRequestDto> request, CancellationToken cancellationToken = default);

    Task DeleteCosmeticServiceAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

    Task<GetCosmeticServiceResponseDto> GetCosmeticServiceAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);

    Task<PageResponseDto<GetCosmeticServiceResponseDto>> GetCosmeticServiceListAsync(GetCosmeticServiceListRequestDto request, CancellationToken cancellationToken = default);

    Task<GetCosmeticServiceAnalyticResponseDto> GetCosmeticServiceAnalyticAsync(GetCosmeticServiceAnalyticRequestDto request, CancellationToken cancellationToken = default);
}
