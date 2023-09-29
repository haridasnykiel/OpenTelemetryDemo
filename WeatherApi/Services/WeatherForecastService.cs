using System.Diagnostics;
using System.Text.Json;
using WeatherApi.Clients;

namespace WeatherApi.Services;

public class WeatherForecastService : IWeatherForecastService
{
    IRedisClient redisClient;

    public WeatherForecastService(IRedisClient redisClient)
    {
        this.redisClient = redisClient;
    }

    public WeatherForecast?[] GetForecasts(List<DateOnly> datesRequested)
    {
        var activity = Activity.Current;
        var redisDatabase = redisClient.GetDatabase();

        return datesRequested.Select(date => 
        {

            var key = date.ToString();
            var result = redisClient.Get(redisDatabase, date.ToString());
            
            if(result is null)
            {
                activity?.SetTag("forecasts.date.notfound", date.ToString());
                return null;
            }
            
            return JsonSerializer.Deserialize<WeatherForecast>(result);
        }).ToArray();
    }

    public void AddForecasts(IEnumerable<WeatherForecast> weatherForecasts) 
    {
        var activity = Activity.Current;

        var redisDatabase = redisClient.GetDatabase();

        if(weatherForecasts?.Count() <= 0)
        {
            activity?.SetTag("forecasts.invalid", "Ensure the dates for forecasts are in the future");
            return;
        }

        foreach (var forecast in weatherForecasts)
        {
            var hasSet = redisClient.Set(redisDatabase, forecast.Date.ToString(), JsonSerializer.Serialize(forecast));

            var message = $"{forecast.Date} -> {forecast.Summary} {forecast.TemperatureC}c";
            
            if(hasSet)
            {
                activity?.SetTag("forecast.added", message);
            }
            else 
            {
                activity?.SetTag("forecast.added.failed", message);
            }
        } 
    }
}