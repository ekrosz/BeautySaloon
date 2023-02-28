namespace BeautySaloon.Core.Dto.Requests.Appointment;

public record CreateAppointmentRequestDto
{
    public Guid PersonId { get; init; }

    public DateTime AppointmentDate { get; init; }

    public IReadOnlyCollection<Guid> PersonSubcriptionIds { get; init; } = Array.Empty<Guid>();
}
