CREATE PROCEDURE [dbo].[Forecast_Insert]
  @Date DATETIME2 NOT NULL,
  @TemperatureC INT NOT NULL,
  @Summary VARCHAR(100) NOT NULL 
AS
  INSERT INTO Forecast (Date, TemperatureC, Summary)
  VALUES (@Date, @TemperatureC, @Summary)
RETURN 0
