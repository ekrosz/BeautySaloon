namespace BeautySaloon.Api.Dto.Responses.Order;

public record GetOrderAnalyticResponseDto
{
    public decimal TotalRevenues { get; init; }

    public IReadOnlyCollection<GetOrderAnalyticItemResponseDto> Items { get; init; } = Array.Empty<GetOrderAnalyticItemResponseDto>();

    public IReadOnlyCollection<GetOrderAnalyticItemResponseDto> ForecastItems { get; init; } = Array.Empty<GetOrderAnalyticItemResponseDto>();

    public record GetOrderAnalyticItemResponseDto
    {
        public DateTime Period { get; init; }

        public decimal Revenues { get; init; }

        public int Count { get; init; }
    }
}
