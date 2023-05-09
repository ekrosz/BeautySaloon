using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.CosmeticService;
using BeautySaloon.Api.Dto.Responses.CosmeticService;
using BeautySaloon.Api.Services;
using BeautySaloon.Common;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Route("api/cosmetic-services")]
[ApiController]
public class CosmeticServicesController : ControllerBase
{
    private readonly ICosmeticServiceService _cosmeticServiceService;

    private readonly IValidator<CreateCosmeticServiceRequestDto> _createCosmeticServiceRequestValidator;
    private readonly IValidator<UpdateCosmeticServiceRequestDto> _updateCosmeticServiceRequestValidator;
    private readonly IValidator<GetCosmeticServiceListRequestDto> _getCosmeticServiceListRequestValidator;
    private readonly IValidator<GetCosmeticServiceAnalyticRequestDto> _getCosmeticServiceAnalyticRequestValidator;
    private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;

    public CosmeticServicesController(
        ICosmeticServiceService cosmeticServiceService,
        IValidator<CreateCosmeticServiceRequestDto> createCosmeticServiceRequestValidator,
        IValidator<UpdateCosmeticServiceRequestDto> updateCosmeticServiceRequestValidator,
        IValidator<GetCosmeticServiceListRequestDto> getCosmeticServiceListRequestValidator,
        IValidator<GetCosmeticServiceAnalyticRequestDto> getCosmeticServiceAnalyticRequestValidator,
        IValidator<ByIdRequestDto> byIdRequestValidator)
    {
        _cosmeticServiceService = cosmeticServiceService;
        _createCosmeticServiceRequestValidator = createCosmeticServiceRequestValidator;
        _updateCosmeticServiceRequestValidator = updateCosmeticServiceRequestValidator;
        _getCosmeticServiceListRequestValidator = getCosmeticServiceListRequestValidator;
        _getCosmeticServiceAnalyticRequestValidator = getCosmeticServiceAnalyticRequestValidator;
        _byIdRequestValidator = byIdRequestValidator;
    }

    [HttpGet]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task<PageResponseDto<GetCosmeticServiceResponseDto>> GetListAsync([FromQuery] GetCosmeticServiceListRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getCosmeticServiceListRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _cosmeticServiceService.GetCosmeticServiceListAsync(request, cancellationToken);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = Constants.Roles.AdminAndEmployee)]
    public async Task<GetCosmeticServiceResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById);

        return await _cosmeticServiceService.GetCosmeticServiceAsync(requestById, cancellationToken);
    }

    [HttpPost]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task CreateAsync([FromBody] CreateCosmeticServiceRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createCosmeticServiceRequestValidator.ValidateAndThrowAsync(request);

        await _cosmeticServiceService.CreateCosmeticServiceAsync(request, cancellationToken);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCosmeticServiceRequestDto request, CancellationToken cancellationToken = default)
    {
        await _updateCosmeticServiceRequestValidator.ValidateAndThrowAsync(request);

        var requestById = new ByIdWithDataRequestDto<UpdateCosmeticServiceRequestDto>(id, request);

        await _cosmeticServiceService.UpdateCosmeticServiceAsync(requestById, cancellationToken);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById);

        await _cosmeticServiceService.DeleteCosmeticServiceAsync(requestById, cancellationToken);
    }

    [HttpGet("analytic")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<GetCosmeticServiceAnalyticResponseDto> GetAnalyticAsync([FromQuery] GetCosmeticServiceAnalyticRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getCosmeticServiceAnalyticRequestValidator.ValidateAndThrowAsync(request);

        return await _cosmeticServiceService.GetCosmeticServiceAnalyticAsync(request, cancellationToken);
    }
}
