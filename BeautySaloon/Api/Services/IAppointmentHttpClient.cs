using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Appointment;
using BeautySaloon.Api.Dto.Responses.Appointment;
using Refit;

namespace BeautySaloon.Api.Services;

[Headers("Content-Type: application/json")]
public interface IAppointmentHttpClient
{
    [Post("/api/appointments")]
    Task CreateAsync([Header("Authorization")] string accessToken, [Body] CreateAppointmentRequestDto request, CancellationToken cancellationToken = default);

    [Put("/api/appointments/{id}")]
    Task UpdateAsync([Header("Authorization")] string accessToken, Guid id, [Body] UpdateAppointmentRequestDto request, CancellationToken cancellationToken = default);

    [Patch("/api/appointments/{id}/complete")]
    Task CompleteAsync([Header("Authorization")] string accessToken, Guid id, [Body] CompleteOrCancelAppointmentRequestDto request, CancellationToken cancellationToken = default);

    [Patch("/api/appointments/{id}/cancel")]
    Task CancelAsync([Header("Authorization")] string accessToken, Guid id, [Body] CompleteOrCancelAppointmentRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/appointments")]
    Task<ItemListResponseDto<GetAppointmentListItemResponseDto>> GetListAsync([Header("Authorization")] string accessToken, [Query] GetAppointmentListRequestDto request, CancellationToken cancellationToken = default);

    [Get("/api/appointments/{id}")]
    Task<GetAppointmentResponseDto> GetAsync([Header("Authorization")] string accessToken, Guid id, CancellationToken cancellationToken = default);
}
