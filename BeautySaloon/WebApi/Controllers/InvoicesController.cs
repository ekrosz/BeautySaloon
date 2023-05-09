using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Invoice;
using BeautySaloon.Api.Dto.Responses.Invoice;
using BeautySaloon.Api.Services;
using BeautySaloon.Common;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Route("api/invoices")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    private readonly IValidator<CreateInvoiceRequestDto> _createInvoiceRequestValidator;
    private readonly IValidator<UpdateInvoiceRequestDto> _updateInvoiceRequestValidator;
    private readonly IValidator<GetInvoiceListRequestDto> _getInvoiceListRequestValidator;
    private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;

    public InvoicesController(
        IInvoiceService invoiceService,
        IValidator<CreateInvoiceRequestDto> createInvoiceRequestValidator,
        IValidator<UpdateInvoiceRequestDto> updateInvoiceRequestValidator,
        IValidator<GetInvoiceListRequestDto> getInvoiceListRequestValidator,
        IValidator<ByIdRequestDto> byIdRequestValidator)
    {
        _invoiceService = invoiceService;
        _createInvoiceRequestValidator = createInvoiceRequestValidator;
        _updateInvoiceRequestValidator = updateInvoiceRequestValidator;
        _getInvoiceListRequestValidator = getInvoiceListRequestValidator;
        _byIdRequestValidator = byIdRequestValidator;
    }

    [HttpPost]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task CreateAsync([FromBody] CreateInvoiceRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createInvoiceRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        await _invoiceService.CreateInvoiceAsync(request, cancellationToken);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateInvoiceRequestDto request, CancellationToken cancellationToken = default)
    {
        await _updateInvoiceRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<UpdateInvoiceRequestDto>(id, request);

        await _invoiceService.UpdateInvoiceAsync(requestById, cancellationToken);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task DeleteAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        await _invoiceService.DeleteInvoiceAsync(requestById, cancellationToken);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<GetInvoiceResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        return await _invoiceService.GetInvoiceAsync(requestById, cancellationToken);
    }

    [HttpGet]
    [Authorize(Roles = Constants.Roles.Admin)]
    public async Task<PageResponseDto<GetInvoiceListItemResponseDto>> GetListAsync([FromQuery] GetInvoiceListRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getInvoiceListRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _invoiceService.GetInvoiceListAsync(request, cancellationToken);
    }
}
