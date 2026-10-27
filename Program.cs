var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/api/weatherforecast", (WeatherForecastService forecastService) =>
    forecastService.GetNextFiveDays(DateOnly.FromDateTime(DateTime.UtcNow)))
    .WithName("GetWeatherForecast");

app.Run();
