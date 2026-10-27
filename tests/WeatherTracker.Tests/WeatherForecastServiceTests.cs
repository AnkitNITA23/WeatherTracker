namespace WeatherTracker.Tests;

public class WeatherForecastServiceTests
{
    [Fact]
    public void GetNextFiveDays_ReturnsFiveForecastsForFollowingDays()
    {
        var service = new WeatherForecastService();
        var startDate = new DateOnly(2026, 1, 1);

        var forecasts = service.GetNextFiveDays(startDate);

        Assert.Equal(5, forecasts.Count);
        Assert.Equal(new DateOnly(2026, 1, 2), forecasts[0].Date);
        Assert.Equal(new DateOnly(2026, 1, 6), forecasts[4].Date);
    }

    [Fact]
    public void TemperatureF_ConvertsCelsiusToFahrenheit()
    {
        var forecast = new WeatherForecast(new DateOnly(2026, 1, 2), 18, "Cloudy");

        Assert.Equal(64, forecast.TemperatureF);
    }
}
