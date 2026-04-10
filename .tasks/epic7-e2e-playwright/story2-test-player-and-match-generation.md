# Story 2 — Test Player & Match Generation

**Epic:** Epic 7 — E2E Tests with Playwright
**Dependencies:** Story 1 (Playwright infrastructure — `PageTest` base class and `PlaywrightFixture` required)

## Description

Write E2E tests covering the core match generation flow: navigating to the Players page, adding players by name, generating matches, and verifying the correct number of matches is displayed. This validates the end-to-end path from browser input through the Blazor WASM client to the ASP.NET Core API and back.

## Acceptance Criteria

### Test Class (`tests/Winterplein.E2eTests/`)

- `PlayerMatchGenerationTests` class annotated with `[Collection("Playwright")]`, extending `PageTest`
- Tests cover:
  - **`AddPlayers_ShowsCorrectCount`** — add 4 players one by one, assert the player count indicator shows 4
  - **`GenerateMatches_WithFourPlayers_ShowsThreeMatches`** — add 4 players, generate matches, assert exactly 3 match cards/rows are visible (C(4,4)×3 = 3)
  - **`GenerateMatches_WithFivePlayers_ShowsFifteenMatches`** — add 5 players, generate matches, assert exactly 15 match cards/rows are visible (C(5,4)×3 = 15)
  - **`RemovePlayer_UpdatesCount`** — add 3 players, remove 1, assert count drops to 2
  - **`GenerateMatches_WithFewerThanFourPlayers_ShowsNoMatches`** — add 3 players, attempt to generate, assert match list is empty or generation is disabled
- Each test navigates to the Players page fresh via `Page.GotoAsync(BaseUrl + "/players")` (or equivalent route)
- Player name inputs use realistic test data (e.g. "Alice", "Bob", "Carol", "Dave")

### Selectors & Resilience

- Tests use `data-testid` attributes (e.g. `[data-testid="player-input"]`, `[data-testid="add-player-btn"]`, `[data-testid="match-list-item"]`) rather than CSS class or element selectors, so tests survive UI restyling
- If `data-testid` attributes are missing from the Blazor components, add them as part of this story

### Blazor Client (`src/Winterplein.Client/`)

- `data-testid` attributes added to relevant elements in the Players and Matches pages/components:
  - `data-testid="player-name-input"` on the player name text field
  - `data-testid="add-player-btn"` on the Add Player button
  - `data-testid="player-count"` on the player count display
  - `data-testid="remove-player-btn"` on each player's remove button
  - `data-testid="generate-matches-btn"` on the Generate Matches button
  - `data-testid="match-list-item"` on each rendered match row/card

## Technical Notes

- Blazor WASM renders asynchronously; after clicking buttons use `Page.WaitForSelectorAsync` or `Expect(locator).ToHaveCountAsync(N)` rather than asserting immediately.
- MudBlazor components render with their own CSS classes and DOM structure — `data-testid` attributes added to the wrapping element are the most stable selector strategy.
- The `data-testid` attribute is set via Blazor's HTML attribute binding: `<MudButton data-testid="add-player-btn" ...>` or `<button data-testid="add-player-btn" ...>`.
- Match count formula: C(N,4) × 3, so N=4 → 3, N=5 → 15, N=6 → 45. Use these as exact expected counts in assertions.
- Tests must be independent — each test navigates fresh and does not depend on state left by a previous test. The in-memory API resets between `dotnet run` sessions, not between individual tests; design tests to add their own players each time.

## Tasks

- [ ] T1: Identify current routes and component structure for Players and Matches pages (read `src/Winterplein.Client/Pages/`) (blockedBy: Story 1 T5)
- [ ] T2: Add `data-testid` attributes to player input, add/remove buttons, player count display, and match list items in the relevant Blazor components (blockedBy: T1)
- [ ] T3: Add `data-testid="generate-matches-btn"` to the Generate Matches button (blockedBy: T1)
- [ ] T4: Create `PlayerMatchGenerationTests.cs` extending `PageTest` with `[Collection("Playwright")]` (blockedBy: Story 1 T5)
- [ ] T5: Write `AddPlayers_ShowsCorrectCount` test (blockedBy: T4, T2)
- [ ] T6: Write `GenerateMatches_WithFourPlayers_ShowsThreeMatches` test (blockedBy: T4, T2, T3)
- [ ] T7: Write `GenerateMatches_WithFivePlayers_ShowsFifteenMatches` test (blockedBy: T4, T2, T3)
- [ ] T8: Write `RemovePlayer_UpdatesCount` test (blockedBy: T4, T2)
- [ ] T9: Write `GenerateMatches_WithFewerThanFourPlayers_ShowsNoMatches` test (blockedBy: T4, T2, T3)
- [ ] T10: Verify all tests pass against running dev stack (blockedBy: T5, T6, T7, T8, T9)
