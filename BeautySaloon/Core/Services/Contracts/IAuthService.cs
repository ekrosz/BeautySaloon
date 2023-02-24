using BeautySaloon.Core.Dto.Requests.Auth;
using BeautySaloon.Core.Dto.Responses.Auth;

namespace BeautySaloon.Core.Services.Contracts;

public interface IAuthService
{
    Task<AuthorizeResponseDto> AuthorizeByCredentialsAsync(AuthorizeByCredentialsRequestDto request, CancellationToken cancellationToken = default);

    Task<AuthorizeResponseDto> AuthorizeByRefreshTokenAsync(AuthorizeByRefreshTokenRequestDto request, CancellationToken cancellationToken = default);

    Task CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default);

    Task UpdateUserAsync(UpdateUserByIdRequestDto request, CancellationToken cancellationToken = default);
}
