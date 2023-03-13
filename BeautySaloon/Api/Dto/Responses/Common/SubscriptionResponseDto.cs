namespace BeautySaloon.Api.Dto.Responses.Common;

public record SubscriptionResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int? LifeTimeInDays { get; init; }

    public decimal Price { get; init; }
}
