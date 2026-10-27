# Weather Tracker

A beginner-friendly ASP.NET Core API for learning .NET, Git, CI/CD, and Azure.

The app gets a five-day forecast from [Open-Meteo](https://open-meteo.com/). This learning project uses its public API and does not need an API key.

## Step 1: Run the app locally

You need the .NET 10 SDK. From this folder, run:

```powershell
dotnet run
```

In another terminal, open the URL printed by the app with `/api/weatherforecast` at the end. For example:

```text
http://localhost:5000/api/weatherforecast
```

The actual port may be different; use the URL printed in your terminal. Supply a location using latitude and longitude; for example, Zurich:

```text
http://localhost:5000/api/weatherforecast?latitude=47.3769&longitude=8.5417
```

The response is JSON containing five days of daily mean temperatures in Celsius and Fahrenheit, plus a weather summary. Try your own coordinates. Latitude ranges from -90 to 90, and longitude from -180 to 180.

In development, the API's OpenAPI description is also available at `/openapi/v1.json`.

## Step 2: Run the tests

```powershell
dotnet test WeatherTracker.slnx
```

The tests check that provider data is mapped to five forecast days, the location is sent to the provider, incomplete provider responses are rejected, and Fahrenheit conversion works. Tests use a fake HTTP response, so they do not depend on internet access or the live weather service.

## Step 3: Understand the CI workflow

The workflow in `.github/workflows/ci.yml` runs when code is pushed to GitHub or a pull request is opened. GitHub Actions checks out the code, installs .NET, restores dependencies, builds the solution, and runs its tests.

This is **continuous integration (CI)**: the automated checks catch problems before changes are merged. The workflow does not deploy the app yet.

To see it run, create a GitHub repository, push this folder to it, and open the repository's **Actions** tab.

## Learning path

1. Run the API and tests locally.
2. Push the project to GitHub and learn how each CI step appears in Actions.
3. Learn how the weather API calls Open-Meteo and maps its weather codes.
4. Add deployment (CD) to Azure App Service, then keep credentials in GitHub/Azure secrets—not in source files.

**Continuous delivery/deployment (CD)** adds a release/deployment after CI succeeds. We will add that only after the app is running reliably and its configuration and Azure credentials are understood.
