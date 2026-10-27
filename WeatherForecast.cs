public record WeatherForecast(DateOnly Date, double TemperatureC, string Summary)
{
    public double TemperatureF => TemperatureC * 9 / 5 + 32;
}
