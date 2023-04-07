namespace BeautySaloon.DAL.Entities.ValueObjects;

public record CosmeticServiceSnapshot
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public int? ExecuteTimeInMinutes { get; init; }
}
