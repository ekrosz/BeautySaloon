using BeautySaloon.Api.Dto.Requests.CosmeticService;
using BeautySaloon.Api.Dto.Responses.CosmeticService;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface ICosmeticServiceHttpClient
{
    [Get("/api/cosmetic-services")]
    Task<PageResponseDto<GetCosmeticServiceResponseDto>> GetListAsync([Header("Authorization")] string accessToken, [Query] GetCosmeticServiceListRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/cosmetic-services/{id}")]
    Task<GetCosmeticServiceResponseDto> GetAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);

    [Post("/api/cosmetic-services")]
    Task CreateAsync([Header("Authorization")] string accessToken, [Body] CreateCosmeticServiceRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/cosmetic-services/{id}")]
    Task UpdateAsync([Header("Authorization")] string accessToken, Guid id, [Body] UpdateCosmeticServiceRequestDto request, CancellationToken cancellationToken = default);

    [Delete("/api/cosmetic-services/{id}")]
    Task DeleteAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);
}
