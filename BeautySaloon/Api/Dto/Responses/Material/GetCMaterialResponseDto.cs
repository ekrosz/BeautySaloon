namespace BeautySaloon.Api.Dto.Responses.Material;

public record GetMaterialResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }
}
