using BeautySaloon.Api.Dto.Common;
using BeautySaloon.Api.Dto.Requests.Appointment;
using BeautySaloon.Api.Dto.Responses.Appointment;
using BeautySaloon.DAL.Entities.ValueObjects.Pagination;

namespace BeautySaloon.Core.Services.Contracts;

public interface IAppointmentService
{
    Task CreateAppointmentAsync(CreateAppointmentRequestDto request, CancellationToken cancellationToken = default);

    Task UpdateAppointmentAsync(ByIdWithDataRequestDto<UpdateAppointmentRequestDto> request, CancellationToken cancellationToken = default);

    Task CompleteAppointmentAsync(ByIdWithDataRequestDto<CompleteOrCancelAppointmentRequestDto> request, CancellationToken cancellationToken = default);

    Task CancelAppointmentAsync(ByIdWithDataRequestDto<CompleteOrCancelAppointmentRequestDto> request, CancellationToken cancellationToken = default);

    Task<PageResponseDto<GetAppointmentListItemResponseDto>> GetAppointmentListAsync(GetAppointmentListRequestDto request, CancellationToken cancellationToken = default);

    Task<GetAppointmentResponseDto> GetAppointmentAsync(ByIdRequestDto request, CancellationToken cancellationToken = default);
}
