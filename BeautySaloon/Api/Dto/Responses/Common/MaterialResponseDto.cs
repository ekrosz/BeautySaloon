namespace BeautySaloon.Api.Dto.Responses.Common;
public record MaterialResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int Count { get; init; }

    public decimal? Cost { get; init; }

}
