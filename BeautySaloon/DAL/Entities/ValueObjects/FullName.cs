namespace BeautySaloon.DAL.Entities.ValueObjects;

public record FullName(string FirstName, string LastName, string? MiddleName)
{
    public static FullName Empty => new(string.Empty, string.Empty, null);

    public string ConcatedName => $"{LastName} {FirstName} {MiddleName}".TrimEnd(' ');
}
