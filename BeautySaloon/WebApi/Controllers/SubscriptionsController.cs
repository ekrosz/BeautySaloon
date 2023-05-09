using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Subscription;
using BeautySaloon.Api.Dto.Responses.Subscription;
using BeautySaloon.Common;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Route("api/subscriptions")]
[ApiController]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    private readonly IValidator<CreateSubscriptionRequestDto> _createSubscriptionRequestValidator;
    private readonly IValidator<UpdateSubscriptionRequestDto> _updateSubscriptionRequestValidator;
    private readonly IValidator<GetSubscriptionListRequestDto> _getSubscriptionListRequestValidator;
    private readonly IValidator<GetSubscriptionAnalyticRequestDto> _getSubscriptionAnalyticRequestValidator;
    private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;

    public SubscriptionsController(
        ISubscriptionService subscriptionService,
        IValidator<CreateSubscriptionRequestDto> createSubscriptionRequestValidator,
        IValidator<UpdateSubscriptionRequestDto> updateSubscriptionRequestValidator,
        IValidator<GetSubscriptionListRequestDto> getSubscriptionListRequestValidator,
        IValidator<GetSubscriptionAnalyticRequestDto> getSubscriptionAnalyticRequestValidator,
        IValidator<ByIdRequestDto> byIdRequestValidator)
    {
        _subscriptionService = subscriptionService;
        _createSubscriptionRequestValidator = createSubscriptionRequestValidator;
        _updateSubscriptionRequestValidator = updateSubscriptionRequestValidator;
        _getSubscriptionListRequestValidator = getSubscriptionListRequestValidator;
        _getSubscriptionAnalyticRequestValidator = getSubscriptionAnalyticRequestValidator;
        _byIdRequestValidator = byIdRequestValidator;
    }

    [HttpPost]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task CreateAsync([FromBody] CreateSubscriptionRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createSubscriptionRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        await _subscriptionService.CreateSubscriptionAsync(request, cancellationToken);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateSubscriptionRequestDto request, CancellationToken cancellationToken = default)
    {
        await _updateSubscriptionRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<UpdateSubscriptionRequestDto>(id, request);

        await _subscriptionService.UpdateSubscriptionAsync(requestById, cancellationToken);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        await _subscriptionService.DeleteSubscriptionAsync(requestById, cancellationToken);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task<GetSubscriptionResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        return await _subscriptionService.GetSubscriptionAsync(requestById, cancellationToken);
    }

    [HttpGet]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task<PageResponseDto<GetSubscriptionListItemResponseDto>> GetListAsync([FromQuery] GetSubscriptionListRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getSubscriptionListRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _subscriptionService.GetSubscriptionListAsync(request, cancellationToken);
    }

    [HttpGet("analytic")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<GetSubscriptionAnalyticResponseDto> GetAnalyticAsync([FromQuery] GetSubscriptionAnalyticRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getSubscriptionAnalyticRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _subscriptionService.GetSubscriptionAnalyticAsync(request, cancellationToken);
    }
}
