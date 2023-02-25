using BeautySaloon.Core.Dto.Common;
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
    private readonly IValidator<UpdateUserRequestDto> _updateUserByIdValidator;
    private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;

    public UsersController(
        IAuthService authService,
        IValidator<AuthorizeByCredentialsRequestDto> authorizeByCredentialsValidator,
        IValidator<AuthorizeByRefreshTokenRequestDto> authorizeByRefreshTokenValidator,
        IValidator<CreateUserRequestDto> createUserValidator,
        IValidator<UpdateUserRequestDto> updateUserByIdValidator,
        IValidator<ByIdRequestDto> byIdRequestValidator)
    {
        _authService = authService;
        _authorizeByCredentialsValidator = authorizeByCredentialsValidator;
        _authorizeByRefreshTokenValidator = authorizeByRefreshTokenValidator;
        _createUserValidator = createUserValidator;
        _updateUserByIdValidator = updateUserByIdValidator;
        _byIdRequestValidator = byIdRequestValidator;
    }

    [HttpPost("/auth/credentials")]
    public async Task<AuthorizeResponseDto> AuthorizeAsync([FromBody] AuthorizeByCredentialsRequestDto request, CancellationToken cancellationToken = default)
    {
        await _authorizeByCredentialsValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _authService.AuthorizeByCredentialsAsync(request, cancellationToken);
    }

    [HttpPost("/auth/refresh")]
    public async Task<AuthorizeResponseDto> AuthorizeAsync([FromBody] AuthorizeByRefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        await _authorizeByRefreshTokenValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _authService.AuthorizeByRefreshTokenAsync(request, cancellationToken);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task CreateUserAsync([FromBody] CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createUserValidator.ValidateAndThrowAsync(request, cancellationToken);

        await _authService.CreateUserAsync(request, cancellationToken);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        await _updateUserByIdValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<UpdateUserRequestDto>(id, request);

        await _authService.UpdateUserAsync(requestById, cancellationToken);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        await _authService.DeleteUserAsync(requestById, cancellationToken);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<GetUserResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        return await _authService.GetUserAsync(requestById, cancellationToken);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public Task<ItemListResponseDto<GetUserResponseDto>> GetListAsync([FromQuery] GetUserListRequestDto request, CancellationToken cancellationToken = default)
        => _authService.GetUserListAsync(request, cancellationToken);
}
