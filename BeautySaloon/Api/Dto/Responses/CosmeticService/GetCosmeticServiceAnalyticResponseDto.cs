namespace BeautySaloon.Api.Dto.Responses.CosmeticService;

public record GetCosmeticServiceAnalyticResponseDto
{
    public IReadOnlyCollection<GetCosmeticServiceAnalyticItemResponseDto> Items { get; init; } = Array.Empty<GetCosmeticServiceAnalyticItemResponseDto>();

    public int TotalCount { get; init; }

    public record GetCosmeticServiceAnalyticItemResponseDto
    {
        public string CosmeticServiceName { get; init; } = string.Empty;

        public int Count { get; init; }
    }
}
