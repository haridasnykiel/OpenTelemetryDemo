using Microsoft.AspNetCore.Mvc;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    IWeatherForecastService service;

    public WeatherForecastController(IWeatherForecastService service)
    {
        this.service = service;
    }

    [HttpGet(Name = "GetWeather")]
    public IActionResult Get([FromQuery] List<DateOnly> dates)
    {

        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Get Weather Forecasts");

        var forecasts = service.GetForecasts(dates);

        activity?.SetTag("forecasts.found.count", forecasts.Count(f => f != null));

        return Ok(forecasts);
    }

    [HttpPost(Name = "PostWeather")]
    public void Post(IEnumerable<WeatherForecast> weatherForecasts) 
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Post Weather Forecasts");

        var validForecasts = weatherForecasts.Where(w => w.Date > DateOnly.FromDateTime(DateTime.UtcNow));

        service.AddForecasts(weatherForecasts);
    }   
}
