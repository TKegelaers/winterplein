# create-e2e-test-class

## Scope

Create `PlayerMatchGenerationTests` in `tests/Winterplein.E2eTests` with five E2E tests covering the full add/remove player and generate-matches flow.

Tests to implement:

- `AddPlayers_ShowsCorrectCount` — add 4 players, assert `player-count` text shows 4
- `GenerateMatches_WithFourPlayers_ShowsThreeMatches` — navigate to Matches, add 4 players via Players page, generate, assert 3 `match-list-item` elements visible
- `GenerateMatches_WithFivePlayers_ShowsFifteenMatches` — same flow with 5 players, assert 15 items
- `RemovePlayer_UpdatesCount` — add 3 players, remove 1, assert count drops to 2
- `GenerateMatches_WithFewerThanFourPlayers_ShowsNoMatches` — add 3 players, navigate to Matches, assert Generate button is disabled or no match items rendered

Each test must:

- Navigate fresh to the relevant page at test start
- Use `data-testid` selectors exclusively
- Use `Expect(locator).ToHaveCountAsync` / `ToBeDisabledAsync` for async Blazor WASM rendering — never assert immediately after a click
- Add players by filling first name, last name, then clicking `add-player-btn`
- Be independent: the in-memory API persists state across tests in the same run, so each test adds only its own players and the assertions are based on counts relative to the starting state

The class is decorated with `[Collection("Playwright")]` and extends the `PageTest` base class defined in Story 1 (scaffold-playwright-project).

## Domain model changes

None.

## Test cases

The five tests listed above are the acceptance criteria themselves.

## Affected files

- create: `tests/Winterplein.E2eTests/PlayerMatchGenerationTests.cs`
