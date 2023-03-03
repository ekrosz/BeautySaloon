using BeautySaloon.Core.Dto.Responses.Common;

namespace BeautySaloon.Core.Dto.Responses.Subscription;

public record GetSubscriptionResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int? LifeTimeInDays { get; init; }

    public decimal Price { get; init; }

    public IReadOnlyCollection<CosmeticServiceResponseDto> CosmeticServices { get; init; } = Array.Empty<CosmeticServiceResponseDto>();
}
