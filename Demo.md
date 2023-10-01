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

Get request

```text
http://localhost:5229/WeatherForecast?dates=2023-10-07&dates=2023-10-08
```
