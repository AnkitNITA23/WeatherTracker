# Weather Tracker

A beginner-friendly ASP.NET Core API for learning .NET, Git, CI/CD, and Azure.

> **Learning demo:** the forecast is sample data, not a real weather prediction. We will connect a real weather provider in a later step.

## Step 1: Run the app locally

You need the .NET 10 SDK. From this folder, run:

```powershell
dotnet run
```

In another terminal, open the URL printed by the app with `/api/weatherforecast` at the end. For example:

```text
http://localhost:5000/api/weatherforecast
```

The actual port may be different; use the URL printed in your terminal. The response is JSON containing five sample days, with temperatures in Celsius and Fahrenheit.

In development, the API's OpenAPI description is also available at `/openapi/v1.json`.

## Step 2: Run the tests

```powershell
dotnet test WeatherTracker.slnx
```

The tests check that the service returns five following days and that the Fahrenheit conversion works.

## Step 3: Understand the CI workflow

The workflow in `.github/workflows/ci.yml` runs when code is pushed to GitHub or a pull request is opened. GitHub Actions checks out the code, installs .NET, restores dependencies, builds the solution, and runs its tests.

This is **continuous integration (CI)**: the automated checks catch problems before changes are merged. The workflow does not deploy the app yet.

To see it run, create a GitHub repository, push this folder to it, and open the repository's **Actions** tab.

## Learning path

1. Run the API and tests locally.
2. Push the project to GitHub and learn how each CI step appears in Actions.
3. Add a real forecast data provider and configuration.
4. Add deployment (CD) to Azure App Service, then keep credentials in GitHub/Azure secrets—not in source files.

**Continuous delivery/deployment (CD)** adds a release/deployment after CI succeeds. We will add that only after the app is running reliably and its configuration and Azure credentials are understood.
