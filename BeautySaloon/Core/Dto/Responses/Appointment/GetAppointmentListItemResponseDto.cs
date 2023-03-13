using BeautySaloon.Core.Dto.Responses.Common;
using BeautySaloon.DAL.Entities.Enums;

namespace BeautySaloon.Core.Dto.Responses.Appointment;

public record GetAppointmentListItemResponseDto
{
    public Guid Id { get; init; }

    public DateTime AppointmentDate { get; init; }

    public int DurationInMinutes { get; init; }

    public AppointmentStatus Status { get; init; }

    public string? Comment { get; init; }

    public PersonResponseDto Person { get; init; } = new();

    public ModifierResponseDto Modifier { get; set; } = new();
}
