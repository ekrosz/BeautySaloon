namespace BeautySaloon.Core.Settings;

public record BLayerSettings
{
    public int ExecuteJobPeriodInDays { get; init; }

    public SmartPaySettings SmartPaySettings { get; init; } = new();
}

public record SmartPaySettings
{
    public string BaseUrl { get; init; } = string.Empty;

    public string AuthToken { get; init; } = string.Empty;

    public string AuthScheme { get; init; } = string.Empty;

    public string ServiceId { get; init; } = string.Empty;

    public string DefaultEmail { get; init; } = string.Empty;
}
