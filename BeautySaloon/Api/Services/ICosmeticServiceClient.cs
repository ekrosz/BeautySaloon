using BeautySaloon.Api.Dto.Requests.CosmeticService;
using BeautySaloon.Api.Dto.Responses.CosmeticService;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface ICosmeticServiceClient
{
    [Get("/api/cosmetic-services")]
    Task<PageResponseDto<GetCosmeticServiceResponseDto>> GetListAsync([Query] GetCosmeticServiceListRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/cosmetic-services/{id}")]
    Task<GetCosmeticServiceResponseDto> GetAsync(Guid id, CancellationToken cancellationToken = default);

    [Post("/api/cosmetic-services")]
    Task CreateAsync([Body] CreateCosmeticServiceRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/cosmetic-services/{id}")]
    Task UpdateAsync(Guid id, [Body] UpdateCosmeticServiceRequestDto request, CancellationToken cancellationToken = default);

    [Delete("/api/cosmetic-services/{id}")]
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
