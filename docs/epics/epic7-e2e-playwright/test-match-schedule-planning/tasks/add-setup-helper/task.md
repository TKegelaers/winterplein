# add-setup-helper

## Scope

Add a `SetupSeasonWithPlayersAsync(string seasonName, int playerCount, int matchdayCount)` helper to `MatchSchedulePlanningTests` (or a shared base) that creates a season with a fixed anchor date range producing the requested number of matchdays, then enrolls `playerCount` players. This helper is called at the start of every test to avoid duplicated setup code.

Implementation notes:

- Use fixed anchor dates (e.g. next Monday from a hardcoded reference date) so matchday counts are deterministic regardless of when the tests run.
- Reuse the `SeasonApiClient` / direct HTTP calls through `Page.APIRequestContext` or navigate via UI — choose whichever pattern the existing Story 3 helper establishes.
- Player creation must use unique names (GUID suffix) to avoid collision with other test runs.

## Domain model changes

None.

## Test cases

No standalone test cases. Correct behavior verified implicitly when all seven `MatchSchedulePlanningTests` pass.

## Affected files

- modify: `tests/Winterplein.E2eTests/MatchSchedulePlanningTests.cs` — add `SetupSeasonWithPlayersAsync` private method (or extract to a shared `E2EHelpers` class if other test classes reuse it)
