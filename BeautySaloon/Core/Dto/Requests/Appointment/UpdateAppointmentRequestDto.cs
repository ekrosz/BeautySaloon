namespace BeautySaloon.Core.Dto.Requests.Appointment;

public record UpdateAppointmentRequestDto
{
    public DateTime AppointmentDate { get; init; }

    public IReadOnlyCollection<Guid> PersonSubcriptionIds { get; init; } = Array.Empty<Guid>();
}
