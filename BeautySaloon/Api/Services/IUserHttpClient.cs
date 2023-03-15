using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.User;
using BeautySaloon.Api.Dto.Responses.User;
using BeautySaloon.DAL.Providers;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface IUserHttpClient
{
    [Post("/api/users")]
    Task CreateAsync([Body] CreateUserRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/users/{id}")]
    Task UpdateAsync(Guid id, [Body] UpdateUserRequestDto request, CancellationToken cancellationToken = default);

    [Delete("/api/users/{id}")]
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    [Get("/api/users/{id}")]
    Task<GetUserResponseDto> GetAsync(Guid id, CancellationToken cancellationToken = default);

    [Get("/api/users/me")]
    Task<GetUserResponseDto> GetAsync(CancellationToken cancellationToken = default);

    [Get("/api/users")]
    Task<ItemListResponseDto<GetUserResponseDto>> GetListAsync([Query] GetUserListRequestDto request, CancellationToken cancellationToken = default);
}
