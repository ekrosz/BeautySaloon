using BeautySaloon.DAL.Entities.Enums;

namespace BeautySaloon.Api.Dto.Responses.Person;

public record PersonSubscriptionResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int? LifeTimeInDays { get; init; }

    public decimal Price { get; init; }

    public PersonSubscriptionStatus Status { get; init; }

    public DateTime PaidOn { get; init; }
}
