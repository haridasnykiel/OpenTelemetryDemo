## Code segments

### URL
- http://localhost:5229

### Post Request With Dates in the Past

```json
[
  {
    "date": "2023-09-30",
    "temperatureC": 10,
    "summary": "Chilly"
  },
  {
    "date": "2023-09-29",
    "temperatureC": 11,
    "summary": "Chilly"
  }
]
```

### Valid Post Request

```json
[
  {
    "date": "2023-10-06",
    "temperatureC": 10,
    "summary": "Chilly"
  },
  {
    "date": "2023-10-07",
    "temperatureC": 11,
    "summary": "Chilly"
  },
  {
    "date": "2023-10-08",
    "temperatureC": 12,
    "summary": "Chilly"
  },
  {
    "date": "2023-10-09",
    "temperatureC": 13,
    "summary": "Chilly"
  },
  {
    "date": "2023-10-10",
    "temperatureC": 14,
    "summary": "Chilly"
  },
  {
    "date": "2023-10-11",
    "temperatureC": 15,
    "summary": "Warm"
  },
  {
    "date": "2023-10-12",
    "temperatureC": 20,
    "summary": "Hot"
  },
  {
    "date": "2023-10-13",
    "temperatureC": 19,
    "summary": "Warm"
  }
]
```

## Get request

```text
http://localhost:5229/WeatherForecast?dates=2023-10-07&dates=2023-10-08
```

## Required packages to setup tracing

```xml
  <PackageReference Include="OpenTelemetry.Exporter.Console" Version="1.5.1" />
  <PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" Version="1.5.1" />
  <PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.5.1" />
  <PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" Version="1.5.1-beta.1" />
```

## Tracing Middleware

```c#
builder.Services.AddOpenTelemetry()
    .WithTracing(tracerProviderBuilder =>
        tracerProviderBuilder
            .AddSource(DiagnosticsConfig.ActivitySource.Name)
            .ConfigureResource(resource => resource
                .AddService(DiagnosticsConfig.ServiceName))
            .AddAspNetCoreInstrumentation()
            .AddOtlpExporter()
            .AddConsoleExporter());
```

## Setup Activity Source

```c#
using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Get Weather Forecasts");
```

```c#
using var activity = DiagnosticsConfig.ActivitySource.StartActivity("Post Weather Forecasts");
```

## Tags/Attributes

### Add tag to check forecasts count in get controller

```c#
activity?.SetTag("forecasts.found.count", forecasts.Count);
```

### Activity.Current

```c#
var activity = Activity.Current;
```

### Add tag to check forecasts count in get service

```c#
activity?.SetTag("forecasts.dates.notfound", datesNotFound.ToString());
```

## Span Events

### Validation event when no forecasts are found get controller

```c#
activity?.AddEvent(new ActivityEvent(
                "Validation Failed",
                tags: new ActivityTagsCollection(
                    new List<KeyValuePair<string, object?>> {
                        new("forecasts.notfound", "No valid forecasts found")
                })));
```

### Exception Event when redis client errors trying to get data

```c#
var activity = Activity.Current;
activity.SetStatus(ActivityStatusCode.Error);
activity.RecordException(ex, new TagList {
    {"key", key}
});
```

### Validation Event when forecasts being added are in the past when trying to post data

```c#
activity?.AddEvent(new ActivityEvent(
                "Validation Failed",
                tags: new ActivityTagsCollection(
                    new List<KeyValuePair<string, object?>> {
                        new("forecasts.invalid", "Ensure the dates for forecasts are in the future")
                })));
```

```c#
var activity = Activity.Current;
activity.SetStatus(ActivityStatusCode.Error);
activity.RecordException(ex, new TagList {
    {"key", key}
});
```
