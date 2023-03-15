namespace BeautySaloon.DAL.Entities.ValueObjects;

public record FullName
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    public static FullName Empty => new();

    public string ConcatedName => $"{LastName} {FirstName} {MiddleName}".TrimEnd(' ');
}
