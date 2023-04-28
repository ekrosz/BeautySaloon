using BeautySaloon.Api.Dto.Requests.Material;
using BeautySaloon.Api.Dto.Responses.Material;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface IMaterialHttpClient
{
    [Get("/api/materials")]
    Task<PageResponseDto<GetMaterialResponseDto>> GetListAsync([Header("Authorization")] string accessToken, [Query] GetMaterialListRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/materials/{id}")]
    Task<GetMaterialResponseDto> GetAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);

    [Post("/api/materials")]
    Task CreateAsync([Header("Authorization")] string accessToken, [Body] CreateMaterialRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/materials/{id}")]
    Task UpdateAsync([Header("Authorization")] string accessToken, Guid id, [Body] UpdateMaterialRequestDto request, CancellationToken cancellationToken = default);

    [Delete("/api/materials/{id}")]
    Task DeleteAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);
}
