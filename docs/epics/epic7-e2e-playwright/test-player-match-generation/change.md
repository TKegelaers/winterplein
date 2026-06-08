# test-player-match-generation

## Problem Statement

The core match generation flow (add players → generate matches → view results) has no E2E coverage. Unit and integration tests verify the logic and API in isolation, but nothing validates that a user can complete the full flow end-to-end through the browser.

## Proposed Solution

Write a `PlayerMatchGenerationTests` class with five E2E tests covering the add/remove player and generate-matches flow. Add `data-testid` attributes to the relevant Blazor components so tests target stable selectors that survive UI restyling.

## Business Requirements

**Given** a user navigates to the Players page
**When** they add players and click Generate Matches
**Then** the correct number of match cards is displayed based on the C(N,4)×3 formula

## Acceptance Criteria

- [ ] `PlayerMatchGenerationTests` class with `[Collection("Playwright")]`, extending `PageTest`
- [ ] `AddPlayers_ShowsCorrectCount` — add 4 players, assert count shows 4
- [ ] `GenerateMatches_WithFourPlayers_ShowsThreeMatches` — add 4 players, generate, assert 3 match cards visible
- [ ] `GenerateMatches_WithFivePlayers_ShowsFifteenMatches` — add 5 players, generate, assert 15 match cards visible
- [ ] `RemovePlayer_UpdatesCount` — add 3 players, remove 1, assert count drops to 2
- [ ] `GenerateMatches_WithFewerThanFourPlayers_ShowsNoMatches` — add 3 players, attempt generate, assert empty/disabled
- [ ] `data-testid="player-name-input"` on player name field
- [ ] `data-testid="add-player-btn"` on Add Player button
- [ ] `data-testid="player-count"` on player count display
- [ ] `data-testid="remove-player-btn"` on each remove button
- [ ] `data-testid="generate-matches-btn"` on Generate Matches button
- [ ] `data-testid="match-list-item"` on each match row/card

## Potential Pitfalls

- Blazor WASM renders asynchronously — use `Page.WaitForSelectorAsync` or `Expect(locator).ToHaveCountAsync(N)` rather than asserting immediately after clicks
- Match count formula: C(N,4)×3, so N=4→3, N=5→15, N=6→45
- Tests must be independent — each test navigates fresh and adds its own players (in-memory API does not reset between individual tests in the same run)

## Dependencies

Story 1 (PlaywrightFixture + PageTest base class required)
