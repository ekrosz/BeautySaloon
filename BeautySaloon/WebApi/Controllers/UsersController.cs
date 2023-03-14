using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.User;
using BeautySaloon.Api.Dto.Responses.User;
using BeautySaloon.Api.Services;
using BeautySaloon.Common;
using BeautySaloon.Core.Services.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Authorize(Roles = Constants.Roles.Admin)]
[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase, IUserHttpClient
{
    private readonly IUserService _userService;

    private readonly IValidator<CreateUserRequestDto> _createUserValidator;
    private readonly IValidator<UpdateUserRequestDto> _updateUserByIdValidator;
    private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;

    public UsersController(
        IUserService userService,
        IValidator<CreateUserRequestDto> createUserValidator,
        IValidator<UpdateUserRequestDto> updateUserByIdValidator,
        IValidator<ByIdRequestDto> byIdRequestValidator)
    {
        _userService = userService;
        _createUserValidator = createUserValidator;
        _updateUserByIdValidator = updateUserByIdValidator;
        _byIdRequestValidator = byIdRequestValidator;
    }

    [HttpPost]
    public async Task CreateAsync([FromBody] CreateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createUserValidator.ValidateAndThrowAsync(request, cancellationToken);

        await _userService.CreateUserAsync(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateUserRequestDto request, CancellationToken cancellationToken = default)
    {
        await _updateUserByIdValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<UpdateUserRequestDto>(id, request);

        await _userService.UpdateUserAsync(requestById, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        await _userService.DeleteUserAsync(requestById, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<GetUserResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        return await _userService.GetUserAsync(requestById, cancellationToken);
    }

    [HttpGet]
    public Task<ItemListResponseDto<GetUserResponseDto>> GetListAsync([FromQuery] GetUserListRequestDto request, CancellationToken cancellationToken = default)
        => _userService.GetUserListAsync(request, cancellationToken);
}
