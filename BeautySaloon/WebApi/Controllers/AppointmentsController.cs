using BeautySaloon.Common;
using BeautySaloon.Core.Dto.Common;
using BeautySaloon.Core.Dto.Requests.Appointment;
using BeautySaloon.Core.Dto.Responses.Appointment;
using BeautySaloon.Core.Services.Contracts;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.WebApi.Controllers;

[Authorize(Roles = Constants.Roles.AdminAndEmployee)]
[Route("api/appointments")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _orderService;

    private readonly IValidator<CreateAppointmentRequestDto> _createAppointmentRequestValidator;
    private readonly IValidator<UpdateAppointmentRequestDto> _updateAppointmentRequestValidator;
    private readonly IValidator<CompleteOrCancelAppointmentRequestDto> _completeOrCancelAppointmentRequestValidator;
    private readonly IValidator<GetAppointmentListRequestDto> _getAppointmentListRequestValidator;
    private readonly IValidator<ByIdRequestDto> _byIdRequestValidator;

    public AppointmentsController(
        IAppointmentService orderService,
        IValidator<CreateAppointmentRequestDto> createAppointmentRequestValidator,
        IValidator<UpdateAppointmentRequestDto> updateAppointmentRequestValidator,
        IValidator<CompleteOrCancelAppointmentRequestDto> completeOrCancelAppointmentRequestValidator,
        IValidator<GetAppointmentListRequestDto> getAppointmentListRequestValidator,
        IValidator<ByIdRequestDto> byIdRequestValidator)
    {
        _orderService = orderService;
        _createAppointmentRequestValidator = createAppointmentRequestValidator;
        _updateAppointmentRequestValidator = updateAppointmentRequestValidator;
        _completeOrCancelAppointmentRequestValidator = completeOrCancelAppointmentRequestValidator;
        _getAppointmentListRequestValidator = getAppointmentListRequestValidator;
        _byIdRequestValidator = byIdRequestValidator;
    }

    [HttpPost]
    public async Task CreateAsync([FromBody] CreateAppointmentRequestDto request, CancellationToken cancellationToken = default)
    {
        await _createAppointmentRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        await _orderService.CreateAppointmentAsync(request, cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task UpdateAsync([FromRoute] Guid id, [FromBody] UpdateAppointmentRequestDto request, CancellationToken cancellationToken = default)
    {
        await _updateAppointmentRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<UpdateAppointmentRequestDto>(id, request);

        await _orderService.UpdateAppointmentAsync(requestById, cancellationToken);
    }

    [HttpPatch("{id}/complete")]
    public async Task CompleteAsync([FromRoute] Guid id, [FromBody] CompleteOrCancelAppointmentRequestDto request, CancellationToken cancellationToken = default)
    {
        await _completeOrCancelAppointmentRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<CompleteOrCancelAppointmentRequestDto>(id, request);

        await _orderService.CompleteAppointmentAsync(requestById, cancellationToken);
    }

    [HttpPatch("{id}/cancel")]
    public async Task CancelAsync([FromRoute] Guid id, [FromBody] CompleteOrCancelAppointmentRequestDto request, CancellationToken cancellationToken = default)
    {
        await _completeOrCancelAppointmentRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        var requestById = new ByIdWithDataRequestDto<CompleteOrCancelAppointmentRequestDto>(id, request);

        await _orderService.CancelAppointmentAsync(requestById, cancellationToken);
    }

    [HttpGet]
    public async Task<PageResponseDto<GetAppointmentListItemResponseDto>> GetListAsync([FromQuery] GetAppointmentListRequestDto request, CancellationToken cancellationToken = default)
    {
        await _getAppointmentListRequestValidator.ValidateAndThrowAsync(request, cancellationToken);

        return await _orderService.GetAppointmentListAsync(request, cancellationToken);
    }

    [HttpGet("{id}")]
    public async Task<GetAppointmentResponseDto> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var requestById = new ByIdRequestDto(id);

        await _byIdRequestValidator.ValidateAndThrowAsync(requestById, cancellationToken);

        return await _orderService.GetAppointmentAsync(requestById, cancellationToken);
    }
}

