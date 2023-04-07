namespace BeautySaloon.Api.Dto.Responses.Common;

public record PersonSubscriptionCosmeticServiceResponseDto
{
    public Guid Id { get; init; }

    public SubscriptionResponseDto Subscription { get; init; } = new();

    public CosmeticServiceResponseDto CosmeticService { get; init; } = new();
}
