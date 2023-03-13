using BeautySaloon.Api.Dto.Requests.Auth;
using BeautySaloon.Api.Dto.Responses.Auth;

namespace BeautySaloon.Core.Services.Contracts;

public interface IAuthService
{
    Task<AuthorizeResponseDto> AuthorizeByCredentialsAsync(AuthorizeByCredentialsRequestDto request, CancellationToken cancellationToken = default);

    Task<AuthorizeResponseDto> AuthorizeByRefreshTokenAsync(AuthorizeByRefreshTokenRequestDto request, CancellationToken cancellationToken = default);
}
