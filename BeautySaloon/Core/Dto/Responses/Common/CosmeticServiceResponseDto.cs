﻿namespace BeautySaloon.Core.Dto.Responses.Common;

public record CosmeticServiceResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int ExecuteTimeInMinutes { get; init; }
}
