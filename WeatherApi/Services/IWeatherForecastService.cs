namespace WeatherApi.Services;

public interface IWeatherForecastService
{
    WeatherForecast?[] GetForecasts(List<DateOnly> datesRequested);
    void AddForecasts(IEnumerable<WeatherForecast> weatherForecasts);
}