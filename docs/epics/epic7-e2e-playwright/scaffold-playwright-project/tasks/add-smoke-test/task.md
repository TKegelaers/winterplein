# add-smoke-test

## Scope

Add the `HomePageLoads` smoke test that verifies the Blazor WASM client serves the home page:

- Create `Tests/HomePageTests.cs` in `Winterplein.E2eTests`
- Test class is decorated with `[Collection("Playwright")]` and inherits `PageTest`
- `HomePageLoads` test navigates to `BaseUrl + "/"` and asserts `Page.TitleAsync()` contains `"Winterplein"`

## Domain model changes

None.

## Test cases

- `HomePageTests.HomePageLoads`
  - Navigate to `/`
  - Assert page title contains `"Winterplein"`

## Affected files

- create: `tests/Winterplein.E2eTests/Tests/HomePageTests.cs`
