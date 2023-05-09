namespace BeautySaloon.Core.Utils.Dto;

public record ForecastDto
{
    public DateTime Period { get; init; }

    public double Value { get; init; }
}
