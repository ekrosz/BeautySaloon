using BeautySaloon.Api.Dto.Requests.Auth;
using BeautySaloon.Api.Dto.Responses.Auth;
using BeautySaloon.Api.Services;
using BeautySaloon.Core.Services.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase, IAuthClient
{
    private readonly IAuthService _authService;

    private readonly IValidator<AuthorizeByCredentialsRequestDto> _authorizeByCredentialsValidator;
    private readonly IValidator<AuthorizeByRefreshTokenRequestDto> _authorizeByRefreshTokenValidator;

    public AuthController(
        IAuthService authService,
        IValidator<AuthorizeByCredentialsRequestDto> authorizeByCredentialsValidator,
        IValidator<AuthorizeByRefreshTokenRequestDto> authorizeByRefreshTokenValidator)
    {
        _authService = authService;
        _authorizeByCredentialsValidator = authorizeByCredentialsValidator;
        _authorizeByRefreshTokenValidator = authorizeByRefreshTokenValidator;
    }

    [HttpPost("credentials")]
    public async Task<AuthorizeResponseDto> AuthorizeAsync([FromBody] AuthorizeByCredentialsRequestDto request, CancellationToken cancellationToken = default)
    {
        await _authorizeByCredentialsValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _authService.AuthorizeByCredentialsAsync(request, cancellationToken);
    }

    [HttpPost("refresh")]
    public async Task<AuthorizeResponseDto> AuthorizeAsync([FromBody] AuthorizeByRefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        await _authorizeByRefreshTokenValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _authService.AuthorizeByRefreshTokenAsync(request, cancellationToken);
    }
}
