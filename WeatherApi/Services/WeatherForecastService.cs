using System.Diagnostics;
using System.Text;
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

    public IList<WeatherForecast> GetForecasts(IList<DateOnly> datesRequested)
    {
        var activity = Activity.Current;
        var redisDatabase = redisClient.GetDatabase();
        var results = new List<WeatherForecast>();

        var datesNotFound = new StringBuilder();

        foreach (var date in datesRequested)
        {
            var key = date.ToString();
            var result = redisClient.Get(redisDatabase, key);
            
            if(result is null)
            {
                datesNotFound.Append(date + ",");
                activity?.SetTag("forecasts.dates.notfound", datesNotFound.ToString());
                continue;
            }

            results.Add(JsonSerializer.Deserialize<WeatherForecast>(result));
        }

        return results;
    }

    public bool AddForecasts(IList<WeatherForecast> weatherForecasts) 
    {
        var activity = Activity.Current;

        var redisDatabase = redisClient.GetDatabase();
        var datesNotSet = new StringBuilder();

        bool hasSetAllValues = true;
        
        foreach (var forecast in weatherForecasts)
        {
            var hasSet = redisClient.Set(redisDatabase, forecast.Date.ToString(), JsonSerializer.Serialize(forecast));

            Thread.Sleep(1000);
            
            if(!hasSet)
            {
                var message = $"{forecast.Date} -> {forecast.Summary} {forecast.TemperatureC}c";
                datesNotSet.AppendLine(message);
                activity?.SetTag("forecast.added.failed", datesNotSet.ToString());
                hasSetAllValues = false;
            }
        }

        return hasSetAllValues;
    }
}