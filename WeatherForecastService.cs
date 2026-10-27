public class WeatherForecastService
{
    private static readonly (int TemperatureC, string Summary)[] SampleConditions =
    [
        (18, "Clear"),
        (20, "Partly cloudy"),
        (16, "Rain"),
        (22, "Sunny"),
        (19, "Breezy")
    ];

    public IReadOnlyList<WeatherForecast> GetNextFiveDays(DateOnly startDate)
    {
        return SampleConditions
            .Select((condition, index) => new WeatherForecast(
                startDate.AddDays(index + 1),
                condition.TemperatureC,
                condition.Summary))
            .ToArray();
    }
}
