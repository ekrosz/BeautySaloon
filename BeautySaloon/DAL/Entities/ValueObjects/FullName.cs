namespace BeautySaloon.DAL.Entities.ValueObjects;

public record FullName(string FirstName, string LastName, string? MiddleName)
{
    public static FullName Empty => new FullName(string.Empty, string.Empty, null);
}
