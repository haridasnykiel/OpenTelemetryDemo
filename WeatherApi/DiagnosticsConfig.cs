using System.Diagnostics;

namespace WeatherApi;

public static class DiagnosticsConfig
{
    public const string ServiceName = "WeatherAPI-OT";
    public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
}