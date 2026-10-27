var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHttpClient<WeatherForecastService>(client =>
    client.BaseAddress = new Uri("https://api.open-meteo.com/"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/api/weatherforecast", async (
    double? latitude,
    double? longitude,
    WeatherForecastService forecastService,
    ILogger<Program> logger) =>
{
    if (latitude is null or < -90 or > 90 || longitude is null or < -180 or > 180)
    {
        return Results.BadRequest(new
        {
            error = "Provide latitude (-90 to 90) and longitude (-180 to 180)."
        });
    }

    try
    {
        var forecasts = await forecastService.GetNextFiveDays(latitude.Value, longitude.Value);
        return Results.Ok(forecasts);
    }
    catch (Exception exception) when (
        exception is HttpRequestException or System.Text.Json.JsonException or InvalidDataException)
    {
        logger.LogError(exception, "The weather provider request failed.");
        return Results.Problem(
            title: "Weather data is temporarily unavailable.",
            statusCode: StatusCodes.Status502BadGateway);
    }
})
    .WithName("GetWeatherForecast");

app.Run();
