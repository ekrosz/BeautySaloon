namespace BeautySaloon.DAL.Settings;

public record DatabaseSettings
{
    public string ConnectionString { get; init; } = string.Empty;
}
