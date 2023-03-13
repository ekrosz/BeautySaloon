namespace BeautySaloon.DAL.Entities.ValueObjects;

public record SubscriptionSnapshot
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;
    
    public decimal Price { get; init; }
    
    public int? LifeTimeInDays { get; init; }
}
