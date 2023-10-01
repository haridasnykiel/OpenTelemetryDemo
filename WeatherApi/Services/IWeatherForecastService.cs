namespace WeatherApi.Services;

public interface IWeatherForecastService
{
    IList<WeatherForecast> GetForecasts(IList<DateOnly> datesRequested);
    bool AddForecasts(IList<WeatherForecast> weatherForecasts);
}