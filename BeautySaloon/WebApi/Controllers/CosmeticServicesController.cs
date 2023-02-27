using BeautySaloon.Common;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.CosmeticService;
using BeautySaloon.Core.Dto.Responses.CosmeticService;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Authorize(Roles = Constants.Roles.AdminAndEmployee)]
[Route("api/cosmetic-services")]
[ApiController]
public class CosmeticServicesController : ControllerBase
{
    private readonly ICosmeticServiceService _cosmeticServiceService;

    private readonly IValidator<CreateCosmeticServiceRequestDto> _createCosmeticServiceRequestValidator;
    private readonly IValidator<UpdateCosmeticServiceRequestDto> _updateCosmeticServiceRequestValidator;
    private readonly IValidator<GetCosmeticServiceListRequestDto> _getCosmeticServiceListRequestValidator;
    private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;

    public CosmeticServicesController(
        ICosmeticServiceService cosmeticServiceService,
        IValidator<CreateCosmeticServiceRequestDto> createCosmeticServiceRequestValidator,
        IValidator<UpdateCosmeticServiceRequestDto> updateCosmeticServiceRequestValidator,
        IValidator<GetCosmeticServiceListRequestDto> getCosmeticServiceListRequestValidator,
        IValidator<ByIdRequestDto> byIdRequestValidator)
    {
        _cosmeticServiceService = cosmeticServiceService;
        _createCosmeticServiceRequestValidator = createCosmeticServiceRequestValidator;
        _updateCosmeticServiceRequestValidator = updateCosmeticServiceRequestValidator;
        _getCosmeticServiceListRequestValidator = getCosmeticServiceListRequestValidator;
        _byIdRequestValidator = byIdRequestValidator;
    }

    [HttpGet]
    public async Task<PageResponseDto<GetCosmeticServiceResponseDto>> GetListAsync([FromQuery] GetCosmeticServiceListRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getCosmeticServiceListRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _cosmeticServiceService.GetCosmeticServiceListAsync(request, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<GetCosmeticServiceResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById);

        return await _cosmeticServiceService.GetCosmeticServiceAsync(requestById, cancellationToken);
    }

    [HttpPost]
    public async Task CreateAsync([FromBody] CreateCosmeticServiceRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createCosmeticServiceRequestValidator.ValidateAndThrowAsync(request);

        await _cosmeticServiceService.CreateCosmeticServiceAsync(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateCosmeticServiceRequestDto request, CancellationToken cancellationToken = default)
    {
        await _updateCosmeticServiceRequestValidator.ValidateAndThrowAsync(request);

        var requestById = new ByIdWithDataRequestDto<UpdateCosmeticServiceRequestDto>(id, request);

        await _cosmeticServiceService.UpdateCosmeticServiceAsync(requestById, cancellationToken);
    }

    [HttpDelete("{id}")]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById);

        await _cosmeticServiceService.DeleteCosmeticServiceAsync(requestById, cancellationToken);
    }
}
