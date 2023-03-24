using BeautySaloon.Api.Dto.Requests.Person;
using BeautySaloon.Api.Dto.Responses.Person;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface IPersonHttpClient
{
    [Post("/api/persons")]
    Task CreateAsync([Header("Authorization")] string accessToken, [Body] CreatePersonRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/persons/{id}")]
    Task UpdateAsync([Header("Authorization")] string accessToken, Guid id, [Body] UpdatePersonRequestDto request, CancellationToken cancellationToken = default);

    [Delete("/api/persons/{id}")]
    Task DeleteAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);

    [Get("/api/persons/{id}")]
    Task<GetPersonResponseDto> GetAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);

    [Get("/api/persons")]
    Task<PageResponseDto<GetPersonListItemResponseDto>> GetListAsync([Header("Authorization")] string accessToken, [Query] GetPersonListRequestDto request, CancellationToken cancellationToken = default);
}