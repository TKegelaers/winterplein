# add-player-setup-helper

## Scope

Add a reusable `SeasonTestHelpers` static class inside `tests/Winterplein.E2eTests` with a `EnsurePlayerExistsAsync(IPage page, string firstName, string lastName)` helper method. The helper navigates to `/players`, adds a player via the existing `data-testid` inputs, and returns. Season tests that require enrolled players call this helper in their setup step.

This task assumes the `data-testid` attributes from the test-player-match-generation story (`player-name-input`, `add-player-btn`) are already present (Story 2 dependency).

## Domain model changes

None.

## Test cases

None — this is a test infrastructure helper, verified indirectly by the player enrollment tests in T3.

## Affected files

- create: `tests/Winterplein.E2eTests/Helpers/SeasonTestHelpers.cs`
