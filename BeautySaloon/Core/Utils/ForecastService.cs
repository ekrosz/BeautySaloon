using BeautySaloon.Core.Utils.Contracts;
using BeautySaloon.Core.Utils.Dto;

namespace BeautySaloon.Core.Utils;

public class ForecastService : IForecastService
{
    private const double SmoothingFactor = 0.5;
    private const double TrendSmoothingFactor = 0.5;

    public IReadOnlyCollection<ForecastDto> GetForecast(IReadOnlyCollection<ForecastDto> source, int forecastPeriodCount)
    {
        var orderedSource = source.OrderBy(x => x.Period).ToArray();

        var forecastModel = new List<ForecastModel>(orderedSource.Length)
        {
            new ForecastModel
            {
                Period = orderedSource.First().Period,
                CurrentValue = orderedSource.First().Value,
                SmoothingValue = orderedSource.First().Value,
                TrendValue = 0
            }
        };

        forecastModel.AddRange(Enumerable.Range(0, forecastModel.Count).Select(i => i == 0
        ? new ForecastModel
        {
            Period = orderedSource.First().Period,
            CurrentValue = orderedSource.First().Value,
            SmoothingValue = orderedSource.First().Value,
            TrendValue = 0
        }
        : new ForecastModel
        {
            Period = orderedSource.ElementAt(i).Period,
            CurrentValue = orderedSource.ElementAt(i).Value,
            SmoothingValue = CalculateSmoothingValue(forecastModel.ElementAt(i - 1), orderedSource.ElementAt(i).Value),
            TrendValue = CalculateTrendValue(forecastModel.ElementAt(i - 1), orderedSource.ElementAt(i).Value)
        }));

        return Enumerable.Range(0, forecastPeriodCount + 1).Select(i => i == 0
        ? new ForecastDto
        {
            Period = orderedSource.Last().Period,
            Value = orderedSource.Last().Value
        }
        : new ForecastDto
        {
            Period = orderedSource.Last().Period.AddMonths(i),
            Value = CalculateForecastValue(forecastModel.Last(), i)
        }).ToArray();
    }

    private double CalculateSmoothingValue(ForecastModel previous, double currentValue)
        => (SmoothingFactor * currentValue) + ((1 - SmoothingFactor) * (previous.SmoothingValue + previous.TrendValue));

    private double CalculateTrendValue(ForecastModel previous, double currentValue)
        => (TrendSmoothingFactor * (CalculateSmoothingValue(previous, currentValue) - previous.SmoothingValue)) + ((1 - TrendSmoothingFactor) * previous.TrendValue);

    private double CalculateForecastValue(ForecastModel previous, int currentStep)
        => CalculateSmoothingValue(previous, previous.CurrentValue) + (currentStep * CalculateTrendValue(previous, previous.CurrentValue));

    private record ForecastModel
    {
        public DateTime Period { get; init; }

        public double CurrentValue { get; init; }

        public double SmoothingValue { get; init; }

        public double TrendValue { get; init; }
    }
}
