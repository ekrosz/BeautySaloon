using BeautySaloon.Core.Dto.Responses.Common;
using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Core.Dto.Responses.Person;

public record GetPersonResponseDto
{
    public Guid Id { get; set; }

    public FullName Name { get; init; } = FullName.Empty;

    public DateTime BirthDate { get; init; }

    public string? Email { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string? Comment { get; init; }

    public IReadOnlyCollection<SubscriptionResponseDto> Subscriptions { get; init; } = Array.Empty<SubscriptionResponseDto>();
}
