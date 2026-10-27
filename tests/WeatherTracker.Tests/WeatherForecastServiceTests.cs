namespace WeatherTracker.Tests;

public class WeatherForecastServiceTests
{
    [Fact]
    public async Task GetNextFiveDays_MapsProviderResponseAndRequestsLocation()
    {
        const string providerResponse = """
            {
              "daily": {
                "time": ["2026-01-02", "2026-01-03", "2026-01-04", "2026-01-05", "2026-01-06"],
                "temperature_2m_mean": [18.5, 20, 16, 22, 19],
                "weather_code": [0, 2, 63, 3, 80]
              }
            }
            """;
        var handler = new StubHttpMessageHandler(providerResponse);
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.open-meteo.com/")
        };
        var service = new WeatherForecastService(httpClient);

        var forecasts = await service.GetNextFiveDays(47.3769, 8.5417);

        Assert.Equal(5, forecasts.Count);
        Assert.Equal(new DateOnly(2026, 1, 2), forecasts[0].Date);
        Assert.Equal(new DateOnly(2026, 1, 6), forecasts[4].Date);
        Assert.Equal(18.5, forecasts[0].TemperatureC);
        Assert.Equal("Clear", forecasts[0].Summary);
        Assert.Equal("Rain", forecasts[2].Summary);
        Assert.Contains("latitude=47.3769", handler.RequestUri!.Query);
        Assert.Contains("longitude=8.5417", handler.RequestUri.Query);
        Assert.Contains("forecast_days=5", handler.RequestUri.Query);
    }

    [Fact]
    public void TemperatureF_ConvertsCelsiusToFahrenheit()
    {
        var forecast = new WeatherForecast(new DateOnly(2026, 1, 2), 18, "Cloudy");

        Assert.Equal(64.4, forecast.TemperatureF);
    }

    [Fact]
    public async Task GetNextFiveDays_ThrowsWhenProviderReturnsIncompleteForecast()
    {
        const string providerResponse = """
            {
              "daily": {
                "time": ["2026-01-02"],
                "temperature_2m_mean": [18],
                "weather_code": [0]
              }
            }
            """;
        var handler = new StubHttpMessageHandler(providerResponse);
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.open-meteo.com/")
        };
        var service = new WeatherForecastService(httpClient);

        await Assert.ThrowsAsync<InvalidDataException>(
            () => service.GetNextFiveDays(47.3769, 8.5417));
    }

    private sealed class StubHttpMessageHandler(string responseBody) : HttpMessageHandler
    {
        public Uri? RequestUri { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestUri = request.RequestUri;
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(responseBody, System.Text.Encoding.UTF8, "application/json")
            });
        }
    }
}
