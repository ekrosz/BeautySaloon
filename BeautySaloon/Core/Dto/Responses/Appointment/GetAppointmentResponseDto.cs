using BeautySaloon.Core.Dto.Responses.Common;
using BeautySaloon.DAL.Entities.Enums;

namespace BeautySaloon.Core.Dto.Responses.Appointment;

public record GetAppointmentResponseDto
{
    public Guid Id { get; init; }

    public DateTime AppointmentDate { get; init; }

    public int DurationInMinutes { get; init; }

    public AppointmentStatus Status { get; init; }

    public string? Comment { get; init; }

    public PersonResponseDto Person { get; init; } = new();

    public IReadOnlyCollection<PersonSubscriptionResponseDto> Subscriptions { get; init; } = Array.Empty<PersonSubscriptionResponseDto>();

    public record PersonSubscriptionResponseDto
    {
        public Guid Id { get; init; }

        public Guid SubscriptionId { get; init; }

        public Guid CosmeticServiceId { get; init; }

        public string SubscriptionName { get; init; } = string.Empty;

        public string CosmeticServiceName { get; init; } = string.Empty;
    }
}
