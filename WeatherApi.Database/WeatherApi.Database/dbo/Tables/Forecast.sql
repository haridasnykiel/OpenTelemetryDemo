CREATE TABLE [dbo].[Forecast]
(
  [Id] INT NOT NULL PRIMARY KEY,
  [Date] DATETIME2 NOT NULL,
  [TemperatureC] INT NOT NULL,
  [Summary] VARCHAR(100) NOT NULL
)
