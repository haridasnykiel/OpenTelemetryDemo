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

        activity?.SetTag("forecasts.found.count", forecasts.Count);

        if(forecasts.Count is 0) {
            return NoContent();
        }

        return Ok(forecasts);
    }

    [HttpPost(Name = "PostWeather")]
    public IActionResult Post(IEnumerable<WeatherForecast> weatherForecasts) 
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Post Weather Forecasts");

        var validForecasts = weatherForecasts
        .Where(w => w.Date > DateOnly.FromDateTime(DateTime.UtcNow))
        .ToList();

        if(validForecasts?.Count() <= 0)
        {
            activity?.SetTag("forecasts.invalid", "Ensure the dates for forecasts are in the future");
            return BadRequest();
        }

        var hasSetValues = service.AddForecasts(validForecasts);

        if(!hasSetValues)
        {
            activity?.SetTag("forecasts.notset", "Some forecasts were not set");
        }

        return Ok();
    }   
}
