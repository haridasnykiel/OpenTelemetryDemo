using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace WeatherApi;

public static class DiagnosticsConfig
{
    public const string ServiceName = "WeatherAPI-OT";
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);

    public static Meter Meter = new(ServiceName);
    public static Counter<long> RequestCounter =
        Meter.CreateCounter<long>("app.weather_ot_request_counter");
}