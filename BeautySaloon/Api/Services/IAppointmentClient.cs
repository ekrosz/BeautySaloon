using BeautySaloon.Api.Dto.Requests.Appointment;
using BeautySaloon.Api.Dto.Responses.Appointment;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface IAppointmentClient
{
    [Post("/api/appointments")]
    Task CreateAsync([Body] CreateAppointmentRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/appointments/{id}")]
    Task UpdateAsync(Guid id, [Body] UpdateAppointmentRequestDto request, CancellationToken cancellationToken = default);

    [Patch("/api/appointments/{id}")]
    Task CompleteAsync(Guid id, [Body] CompleteOrCancelAppointmentRequestDto request, CancellationToken cancellationToken = default);

    [Patch("/api/appointments/{id}")]
    Task CancelAsync(Guid id, [Body] CompleteOrCancelAppointmentRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/appointments")]
    Task<PageResponseDto<GetAppointmentListItemResponseDto>> GetListAsync([Query] GetAppointmentListRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/appointments/{id}")]
    Task<GetAppointmentResponseDto> GetAsync(Guid id, CancellationToken cancellationToken = default);
}
