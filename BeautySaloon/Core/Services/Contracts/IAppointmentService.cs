using BeautySaloon.Core.Dto.Requests.Appointment;

namespace BeautySaloon.Core.Services.Contracts;

public interface IAppointmentService
{
    Task CreateAppointmentAsync(CreateAppointmentRequestDto request, CancellationToken cancellationToken = default);
}
