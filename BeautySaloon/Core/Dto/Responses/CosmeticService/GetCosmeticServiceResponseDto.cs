namespace BeautySaloon.Core.Dto.Responses.CosmeticService;

public record GetCosmeticServiceResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int ExecuteTime { get; init; }
}
