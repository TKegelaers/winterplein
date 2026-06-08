# scaffold-playwright-project

## Problem Statement

No E2E test infrastructure exists for the Winterplein application. There is no way to verify that the Blazor WASM client and ASP.NET Core API work together correctly from a user's perspective — all existing tests (unit and integration) operate below the browser layer.

## Proposed Solution

Create a new xUnit test project (Winterplein.E2eTests) with Microsoft.Playwright, a shared browser fixture, a PageTest base class, and a smoke test that navigates to the home page. The project is wired into the solution but excluded from the default CI test run until a full test environment is available.

## Business Requirements

**Given** a developer wants to run E2E tests against the running Blazor WASM client and ASP.NET Core API
**When** they run `dotnet test tests/Winterplein.E2eTests`
**Then** Playwright launches a Chromium browser, navigates to the app, and the smoke test verifies the home page loads

## Acceptance Criteria

- [ ] `tests/Winterplein.E2eTests` xUnit project targeting net10.0, added to Winterplein.sln
- [ ] No project references to other Winterplein projects
- [ ] `Microsoft.Playwright` NuGet package installed
- [ ] `PlaywrightFixture` class implementing `IAsyncLifetime` that launches Chromium and disposes on teardown
- [ ] `PageTest` base class that creates a fresh `IPage` per test, exposes `BaseUrl` from `E2E_BASE_URL` env var (default: http://localhost:5149)
- [ ] `[Collection("Playwright")]` xUnit collection defined so all test classes share one browser instance
- [ ] Headless mode default (`Headless = true`), overridable via `E2E_HEADLESS=false`
- [ ] `HomePageLoads` smoke test navigates to `/` and asserts page title contains "Winterplein"
- [ ] CLAUDE.md updated with E2E run command and `playwright install` note

## Potential Pitfalls

- Playwright for .NET requires browser binaries installed separately: `pwsh bin/Debug/net10.0/playwright.ps1 install --with-deps chromium` — must be run once after project creation
- E2E tests must NOT be included in `dotnet test` at solution root during CI until a running API + Client environment is provisioned
- `E2E_BASE_URL` must point to the Blazor WASM client (port 5149), not the API (port 5095)
