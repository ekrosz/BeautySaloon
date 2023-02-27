namespace BeautySaloon.Core.Dto.Responses.Subscription;

public record GetSubscriptionListItemResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int? LifeTime { get; init; }

    public decimal Price { get; init; }
}
