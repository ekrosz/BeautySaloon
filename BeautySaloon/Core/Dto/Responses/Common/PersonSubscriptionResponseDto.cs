namespace BeautySaloon.Core.Dto.Responses.Common;

public record PersonSubscriptionResponseDto
{
    public Guid Id { get; init; }

    public SubscriptionResponseDto Subscription { get; init; } = new();

    public CosmeticServiceResponseDto CosmeticService { get; init; } = new();
}
