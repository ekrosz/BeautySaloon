﻿namespace BeautySaloon.Core.Settings;

public record BLayerSettings
{
    public JobSettings JobSettings { get; init; } = new();

    public SmartPaySettings SmartPaySettings { get; init; } = new();

    public MailKitSettings MailKitSettings { get; init; } = new();
}

public record JobSettings
{
    public int RefreshPersonSubscriptionStatusJobDelayInDays { get; init; }

    public int AppointmentNotificationJobInMinutes { get; init; }
}

public record SmartPaySettings
{
    public string BaseUrl { get; init; } = string.Empty;

    public string AuthToken { get; init; } = string.Empty;

    public string AuthScheme { get; init; } = string.Empty;

    public string ServiceId { get; init; } = string.Empty;

    public string DefaultEmail { get; init; } = string.Empty;
}

public record MailKitSettings
{
    public string SenderName { get; init; } = string.Empty;

    public string SenderEmail { get; init; } = string.Empty;

    public string Host { get; init; } = string.Empty;

    public int Port { get; init; }

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}
