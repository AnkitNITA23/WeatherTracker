using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class WeatherForecastService(HttpClient httpClient)
{
    public async Task<IReadOnlyList<WeatherForecast>> GetNextFiveDays(
        double latitude,
        double longitude)
    {
        var requestUri = string.Create(
            CultureInfo.InvariantCulture,
            $"v1/forecast?latitude={latitude}&longitude={longitude}&daily=temperature_2m_mean,weather_code&forecast_days=5&timezone=auto");

        var response = await httpClient.GetFromJsonAsync<OpenMeteoResponse>(requestUri);
        var daily = response?.Daily;

        if (daily?.Time is not { Length: 5 } dates
            || daily.TemperatureCelsius is not { Length: 5 } temperatures
            || daily.WeatherCodes is not { Length: 5 } weatherCodes)
        {
            throw new InvalidDataException("The weather provider returned an incomplete forecast.");
        }

        var forecasts = new WeatherForecast[dates.Length];
        for (var index = 0; index < dates.Length; index++)
        {
            if (temperatures[index] is not double temperature || weatherCodes[index] is not int weatherCode)
            {
                throw new InvalidDataException("The weather provider returned an incomplete forecast.");
            }

            if (!DateOnly.TryParseExact(
                    dates[index],
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var date))
            {
                throw new InvalidDataException("The weather provider returned an invalid forecast date.");
            }

            forecasts[index] = new WeatherForecast(
                date,
                temperature,
                GetSummary(weatherCode));
        }

        return forecasts;
    }

    private static string GetSummary(int weatherCode)
    {
        return weatherCode switch
        {
            0 => "Clear",
            1 => "Mainly clear",
            2 => "Partly cloudy",
            3 => "Overcast",
            45 or 48 => "Fog",
            51 or 53 or 55 => "Drizzle",
            56 or 57 => "Freezing drizzle",
            61 or 63 or 65 => "Rain",
            66 or 67 => "Freezing rain",
            71 or 73 or 75 or 77 => "Snow",
            80 or 81 or 82 => "Rain showers",
            85 or 86 => "Snow showers",
            95 or 96 or 99 => "Thunderstorm",
            _ => "Unknown"
        };
    }
}

internal sealed class OpenMeteoResponse
{
    [JsonPropertyName("daily")]
    public OpenMeteoDailyForecast? Daily { get; init; }
}

internal sealed class OpenMeteoDailyForecast
{
    [JsonPropertyName("time")]
    public string[]? Time { get; init; }

    [JsonPropertyName("temperature_2m_mean")]
    public double?[]? TemperatureCelsius { get; init; }

    [JsonPropertyName("weather_code")]
    public int?[]? WeatherCodes { get; init; }
}
