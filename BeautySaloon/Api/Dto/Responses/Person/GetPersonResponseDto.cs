using BeautySaloon.DAL.Entities.ValueObjects;

namespace BeautySaloon.Api.Dto.Responses.Person;

public record GetPersonResponseDto
{
    public Guid Id { get; init; }

    public FullName Name { get; init; } = FullName.Empty;

    public DateTime BirthDate { get; init; }

    public string? Email { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public IReadOnlyCollection<PersonSubscriptionResponseDto> Subscriptions { get; init; } = Array.Empty<PersonSubscriptionResponseDto>();
}
