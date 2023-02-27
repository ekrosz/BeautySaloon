namespace BeautySaloon.Core.Dto.Responses.Subscription;

public record GetSubscriptionResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int? LifeTime { get; init; }

    public decimal Price { get; init; }

    public IReadOnlyCollection<CosmeticServiceResponseDto> CosmeticServices { get; init; } = Array.Empty<CosmeticServiceResponseDto>();

    public record CosmeticServiceResponseDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public int ExecuteTime { get; init; }
    }
}
