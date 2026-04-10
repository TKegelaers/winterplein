# Story 1 — Scaffold Playwright Test Project

**Epic:** Epic 7 — E2E Tests with Playwright
**Dependencies:** None

## Description

Create a new xUnit test project for Playwright E2E tests, wire it into the solution, and configure browser setup and base URL so subsequent stories can write tests against the running dev stack. The project targets the live Blazor WASM client and ASP.NET Core API; tests are not run in CI until the API and client URLs are stable across environments.

## Acceptance Criteria

### Test Project (`tests/Winterplein.E2eTests/`)

- Project `Winterplein.E2eTests` created as an xUnit project targeting `net10.0`
- Added to `Winterplein.sln`
- No project references to other Winterplein projects (E2E tests hit the live HTTP stack, not internal code)
- `Microsoft.Playwright` NuGet package installed

### Playwright Infrastructure

- `PlaywrightFixture` class implementing `IAsyncLifetime` that:
  - Launches a Chromium browser instance (`Playwright.Chromium.LaunchAsync`)
  - Exposes an `IBrowser` for test classes to create pages from
  - Disposes the browser on teardown
- `PageTest` base class (or equivalent collection fixture) that:
  - Receives `PlaywrightFixture` via constructor injection
  - Creates a fresh `IPage` per test and disposes it after
  - Exposes `BaseUrl` read from the `E2E_BASE_URL` environment variable, defaulting to `http://localhost:5149`
- `[Collection("Playwright")]` xUnit collection and `PlaywrightCollection` attribute defined so all E2E test classes share one browser instance

### Smoke Test

- A single smoke test `HomePageLoads` that navigates to `/` and asserts the page title contains "Winterplein"

### Commands

- `CLAUDE.md` updated with E2E test run command:
  ```powershell
  # Run E2E tests (requires API + Client running)
  $env:E2E_BASE_URL = "http://localhost:5149"; dotnet test tests/Winterplein.E2eTests
  ```

## Technical Notes

- Playwright for .NET requires browser binaries installed separately: `pwsh bin/Debug/net10.0/playwright.ps1 install --with-deps chromium`. Add a note to CLAUDE.md or a README comment so developers know to run this once after project creation.
- `Microsoft.Playwright` is the core package; no MSTest or NUnit adapter is needed — the xUnit `IAsyncLifetime` pattern replaces the framework-specific base classes.
- E2E tests are **not** run by `dotnet test` at solution root by default during CI until a test environment (running API + Client) is provisioned. Isolate by running `dotnet test tests/Winterplein.E2eTests` explicitly.
- `E2E_BASE_URL` must point to the Blazor WASM client, not the API. The API URL is `http://localhost:5095` and is called by the client — E2E tests drive only the browser.
- Headless mode should be the default (`Headless = true` in `BrowserTypeLaunchOptions`); allow override via `E2E_HEADLESS=false` environment variable for local debugging.

## Tasks

- [ ] T1: Create `tests/Winterplein.E2eTests` xUnit project (`dotnet new xunit -n Winterplein.E2eTests -o tests/Winterplein.E2eTests -f net10.0`)
- [ ] T2: Add project to solution (`dotnet sln add tests/Winterplein.E2eTests`) (blockedBy: T1)
- [ ] T3: Install `Microsoft.Playwright` NuGet package in the project (blockedBy: T1)
- [ ] T4: Define `PlaywrightCollection` and `PlaywrightFixture` with browser lifecycle (blockedBy: T3)
- [ ] T5: Define `PageTest` base class with `IPage` per-test setup and `BaseUrl` from env (blockedBy: T4)
- [ ] T6: Write `HomePageLoads` smoke test navigating to `/` and asserting page title (blockedBy: T5)
- [ ] T7: Update `CLAUDE.md` with E2E run command and `playwright install` note (blockedBy: T2)
- [ ] T8: Verify smoke test passes against running dev stack (blockedBy: T6, T7)
