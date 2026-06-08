# add-playwright-infrastructure

## Scope

Add the Microsoft.Playwright NuGet package and create the shared browser infrastructure:

- Add `Microsoft.Playwright` package reference to `Winterplein.E2eTests.csproj`
- Create `PlaywrightFixture` implementing `IAsyncLifetime`: launches Chromium in `InitializeAsync` (respecting `E2E_HEADLESS` env var, defaulting to headless), disposes the browser and playwright instance in `DisposeAsync`
- Create `PlaywrightCollection` attribute class that declares the `[CollectionDefinition("Playwright")]` xUnit collection backed by `PlaywrightFixture`
- Create `PageTest` abstract base class that implements `IClassFixture<PlaywrightFixture>` (via collection), creates a new `IPage` in its constructor or `InitializeAsync`, exposes `Page` and `BaseUrl` (read from `E2E_BASE_URL` env var, default `http://localhost:5149`), and disposes the page on teardown
- Delete the generated `UnitTest1.cs` placeholder

## Domain model changes

None.

## Test cases

None at this stage — infrastructure classes only. Verify the project builds.

## Affected files

- modify: `tests/Winterplein.E2eTests/Winterplein.E2eTests.csproj`
- create: `tests/Winterplein.E2eTests/Infrastructure/PlaywrightFixture.cs`
- create: `tests/Winterplein.E2eTests/Infrastructure/PlaywrightCollection.cs`
- create: `tests/Winterplein.E2eTests/Infrastructure/PageTest.cs`
- delete: `tests/Winterplein.E2eTests/UnitTest1.cs`
