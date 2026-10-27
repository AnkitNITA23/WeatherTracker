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

This is **continuous integration (CI)**: the automated checks catch problems before changes are merged.

To see it run, create a GitHub repository, push this folder to it, and open the repository's **Actions** tab.

## Step 4: Prepare an Azure deployment

The separate workflow in `.github/workflows/deploy-azure.yml` is a manually triggered deployment. It runs the tests, publishes the app, and deploys it to Azure App Service only when you start it from GitHub Actions. A normal push does not deploy.

Before using it:

1. Create an Azure App Service web app for **.NET 10**. Review the plan and region pricing in the Azure portal before creating resources; available free tiers and runtime options can vary.
2. In the web app's **Overview**, copy its name. In GitHub, open **Settings → Secrets and variables → Actions → Variables**, add a repository variable named `AZURE_WEBAPP_NAME`, and set its value to that app name.
3. In the Azure portal, download the web app's **publish profile**. Treat this file as a password: do not commit it or paste it into chat. In GitHub, open **Settings → Secrets and variables → Actions → Secrets**, add a repository secret named `AZURE_WEBAPP_PUBLISH_PROFILE`, and paste the complete publish-profile XML as its value.
4. Open GitHub's **Actions → Deploy to Azure → Run workflow** and manually start the workflow.
5. After it succeeds, visit `https://<your-app-name>.azurewebsites.net/api/weatherforecast?latitude=47.3769&longitude=8.5417`.

The publish profile is stored as an encrypted GitHub secret, not in the repository. It is a deployment credential: limit access to the repository, never print or share it, and rotate it if exposed. For production, prefer a federated identity (OIDC) setup over a long-lived publish profile.

This is **continuous delivery/deployment (CD)**: the workflow prepares and deploys a release after you manually approve the run. Keeping the deployment manual lets you learn and verify the Azure setup before considering automatic deployments.

## Learning path

1. Run the API and tests locally.
2. Push the project to GitHub and learn how each CI step appears in Actions.
3. Learn how the weather API calls Open-Meteo and maps its weather codes.
4. Deploy to Azure App Service with the manual workflow and learn how GitHub secrets protect deployment credentials.
