using BeautySaloon.Api.Dto.Requests.Auth;
using BeautySaloon.Api.Dto.Responses.Auth;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface IAuthHttpClient
{
    [Post("/api/auth/credentials")]
    Task<AuthorizeResponseDto> AuthorizeAsync([Body] AuthorizeByCredentialsRequestDto request, CancellationToken cancellationToken = default);

    [Post("/api/auth/refresh")]
    Task<AuthorizeResponseDto> AuthorizeAsync([Body] AuthorizeByRefreshTokenRequestDto request, CancellationToken cancellationToken = default);
}
