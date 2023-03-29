using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Subscription;
using BeautySaloon.Api.Dto.Responses.Subscription;
using BeautySaloon.Api.Services;
using BeautySaloon.Common;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Authorize(Roles = Constants.Roles.AdminAndEmployee)]
[Route("api/subscriptions")]
[ApiController]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _personService;

    private readonly IValidator<CreateSubscriptionRequestDto> _createSubscriptionRequestValidator;
    private readonly IValidator<UpdateSubscriptionRequestDto> _updateSubscriptionRequestValidator;
    private readonly IValidator<GetSubscriptionListRequestDto> _getSubscriptionListRequestValidator;
    private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;

    public SubscriptionsController(
        ISubscriptionService personService,
        IValidator<CreateSubscriptionRequestDto> createSubscriptionRequestValidator,
        IValidator<UpdateSubscriptionRequestDto> updateSubscriptionRequestValidator,
        IValidator<GetSubscriptionListRequestDto> getSubscriptionListRequestValidator,
        IValidator<ByIdRequestDto> byIdRequestValidator)
    {
        _personService = personService;
        _createSubscriptionRequestValidator = createSubscriptionRequestValidator;
        _updateSubscriptionRequestValidator = updateSubscriptionRequestValidator;
        _getSubscriptionListRequestValidator = getSubscriptionListRequestValidator;
        _byIdRequestValidator = byIdRequestValidator;
    }

    [HttpPost]
    public async Task CreateAsync([FromBody] CreateSubscriptionRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createSubscriptionRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        await _personService.CreateSubscriptionAsync(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateSubscriptionRequestDto request, CancellationToken cancellationToken = default)
    {
        await _updateSubscriptionRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<UpdateSubscriptionRequestDto>(id, request);

        await _personService.UpdateSubscriptionAsync(requestById, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        await _personService.DeleteSubscriptionAsync(requestById, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<GetSubscriptionResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        return await _personService.GetSubscriptionAsync(requestById, cancellationToken);
    }

    [HttpGet]
    public async Task<PageResponseDto<GetSubscriptionListItemResponseDto>> GetListAsync([FromQuery] GetSubscriptionListRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getSubscriptionListRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _personService.GetSubscriptionListAsync(request, cancellationToken);
    }
}
