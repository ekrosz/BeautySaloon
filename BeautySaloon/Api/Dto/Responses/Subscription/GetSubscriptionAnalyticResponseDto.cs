namespace BeautySaloon.Api.Dto.Responses.Subscription;

public record GetSubscriptionAnalyticResponseDto
{
    public IReadOnlyCollection<GetSubscriptionAnalyticItemResponseDto> Items { get; init; } = Array.Empty<GetSubscriptionAnalyticItemResponseDto>();

    public int TotalCount { get; init; }

    public record GetSubscriptionAnalyticItemResponseDto
    {
        public string SubscriptionName { get; init; } = string.Empty;

        public int Count { get; init; }
    }
}
