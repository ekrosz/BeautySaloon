namespace BeautySaloon.Api.Dto.Responses.CosmeticService;

public record GetCosmeticServiceResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int? ExecuteTimeInMinutes { get; init; }
}
