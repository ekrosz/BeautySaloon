using BeautySaloon.Api.Dto.Responses.Common;
using BeautySaloon.DAL.Entities.Enums;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Api.Dto.Responses.Appointment;

public record GetAppointmentResponseDto
{
    public Guid Id { get; init; }

    public DateTime AppointmentDate { get; init; }

    public int DurationInMinutes { get; init; }

    public AppointmentStatus Status { get; init; }

    public string? Comment { get; init; }

    public PersonResponseDto Person { get; init; } = new();

    public ModifierResponseDto Modifier { get; set; } = new();

    public IReadOnlyCollection<PersonSubscriptionResponseDto> Subscriptions { get; init; } = Array.Empty<PersonSubscriptionResponseDto>();
}
