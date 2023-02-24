using BeautySaloon.Core.Dto.Requests.Auth;
using BeautySaloon.Core.Dto.Responses.Auth;
using BeautySaloon.Core.Services.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IAuthService _authService;

    private readonly IValidator<AuthorizeByCredentialsRequestDto> _authorizeByCredentialsValidator;
    private readonly IValidator<AuthorizeByRefreshTokenRequestDto> _authorizeByRefreshTokenValidator;
    private readonly IValidator<CreateUserRequestDto> _createUserValidator;
    private readonly IValidator<UpdateUserByIdRequestDto> _updateUserByIdValidator;

    public UsersController(
        IAuthService authService,
        IValidator<AuthorizeByCredentialsRequestDto> authorizeByCredentialsValidator,
        IValidator<AuthorizeByRefreshTokenRequestDto> authorizeByRefreshTokenValidator,
        IValidator<CreateUserRequestDto> createUserValidator,
        IValidator<UpdateUserByIdRequestDto> updateUserByIdValidator)
    {
        _authService = authService;
        _authorizeByCredentialsValidator = authorizeByCredentialsValidator;
        _authorizeByRefreshTokenValidator = authorizeByRefreshTokenValidator;
        _createUserValidator = createUserValidator;
        _updateUserByIdValidator = updateUserByIdValidator;
    }

    [HttpPost("/auth/credentials")]
    public async Task<AuthorizeResponseDto> AuthorizeAsync(AuthorizeByCredentialsRequestDto request, CancellationToken cancellationToken = default)
    {
        await _authorizeByCredentialsValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _authService.AuthorizeByCredentialsAsync(request, cancellationToken);
    }

    [HttpPost("/auth/refresh")]
    public async Task<AuthorizeResponseDto> AuthorizeAsync(AuthorizeByRefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        await _authorizeByRefreshTokenValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _authService.AuthorizeByRefreshTokenAsync(request, cancellationToken);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task CreateUserAsync(CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createUserValidator.ValidateAndThrowAsync(request, cancellationToken);

        await _authService.CreateUserAsync(request, cancellationToken);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut]
    public async Task UpdateAsync(Guid id, UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        var requestById = new UpdateUserByIdRequestDto(id, request);

        await _updateUserByIdValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        await _authService.UpdateUserAsync(requestById, cancellationToken);
    }
}
