using BeautySaloon.Core.Dto.Responses.Common;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Core.Dto.Responses.Appointment;

public record GetAppointmentListItemResponseDto
{
    public Guid Id { get; init; }

    public DateTime AppointmentDate { get; init; }

    public int DurationInMinutes { get; init; }

    public AppointmentStatus Status { get; init; }

    public string? Comment { get; init; }

    public PersonResponseDto Person { get; init; } = new();
}
