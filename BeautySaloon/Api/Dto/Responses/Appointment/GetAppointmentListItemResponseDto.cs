using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.DAL.Entities.Enums;

namespace BeautySaloon.Api.Dto.Responses.Appointment;

public record GetAppointmentListItemResponseDto
{
    public Guid Id { get; init; }

    public DateTime AppointmentDate { get; init; }

    public DateTime AppointmentDateEnd => AppointmentDate.AddMinutes(DurationInMinutes);

    public int DurationInMinutes { get; init; }

    public AppointmentStatus Status { get; init; }

    public string? Comment { get; init; }

    public PersonResponseDto Person { get; init; } = new();

    public UserResponseDto Modifier { get; set; } = new();

    public DateTime CreatedOn { get; set; }

    public DateTime UpdatedOn { get; set; }
}
