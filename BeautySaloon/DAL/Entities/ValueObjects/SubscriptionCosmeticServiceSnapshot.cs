namespace BeautySaloon.DAL.Entities.ValueObjects;

public record SubscriptionCosmeticServiceSnapshot
{
    public SubscriptionSnapshot SubscriptionSnapshot { get; init; } = new();

    public CosmeticServiceSnapshot CosmeticServiceSnapshot { get; init; } = new();

    public int Count { get; init; }
}
