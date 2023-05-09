using BeautySaloon.Core.Utils.Dto;

namespace BeautySaloon.Core.Utils.Contracts;

public interface IForecastService
{
    IReadOnlyCollection<ForecastDto> GetForecast(IReadOnlyCollection<ForecastDto> source, int forecastPeriodCount);
}
