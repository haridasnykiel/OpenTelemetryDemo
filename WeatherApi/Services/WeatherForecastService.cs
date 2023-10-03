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
                continue;
            }

            var value = JsonSerializer.Deserialize<WeatherForecast>(result);

            if(value is null) continue;

            results.Add(value);
        }

        return results;
    }

    public bool AddForecasts(IList<WeatherForecast> weatherForecasts) 
    {
        var redisDatabase = redisClient.GetDatabase();

        bool hasSetAllValues = true;
        
        foreach (var forecast in weatherForecasts)
        {
            var hasSet = redisClient.Set(
                redisDatabase,
                forecast.Date.ToString(),
                JsonSerializer.Serialize(forecast));
            
            if(!hasSet)
            {
                hasSetAllValues = false;
            }
        }

        return hasSetAllValues;
    }
}