# Story 4 — Test Match Schedule Planning

**Epic:** Epic 7 — E2E Tests with Playwright
**Dependencies:** Story 1 (Playwright infrastructure), Story 2 (data-testid conventions), Story 3 (season + player setup patterns); requires Epics 3 and 4 to be fully implemented

## Description

Write E2E tests covering the full match schedule planning flow: generating a season schedule, browsing matchday assignments, clearing individual and all planned matches, marking player absences, and verifying that the absence-aware schedule generator excludes absent players. These tests are the highest-level integration check of the scheduling domain.

## Acceptance Criteria

### Test Class (`tests/Winterplein.E2eTests/`)

- `MatchSchedulePlanningTests` class annotated with `[Collection("Playwright")]`, extending `PageTest`
- Tests cover:
  - **`GenerateSchedule_FillsAllMatchdays`** — create a season with 3 matchdays and 4 enrolled players, click "Generate Schedule", assert all 3 matchday rows show status "Planned"
  - **`GenerateSchedule_ShowsMatchDetails`** — after generating, assert each planned row displays four player names (two teams)
  - **`ClearPlannedMatch_SetsMatchdayToOpen`** — generate a schedule, clear one matchday's planned match via the clear button and confirmation dialog, assert that row's status changes to "Open"
  - **`ClearAllPlannedMatches_SetsAllMatchdaysToOpen`** — generate a schedule, click "Clear All" and confirm, assert all matchday rows show status "Open"
  - **`RegenerateSchedule_FillsOpenSlots`** — generate, clear one match, regenerate, assert all matchdays are "Planned" again
  - **`MarkAbsence_PlayerAbsenceDialogSavesCorrectly`** — open the player absences dialog for an enrolled player, check one matchday date, save, reopen, assert the checkbox is still checked
  - **`AbsenceAwareGeneration_ExcludesAbsentPlayer`** — mark a player absent for all matchdays, generate schedule, assert no planned match includes that player's name

### data-testid Attributes (`src/Winterplein.Client/`)

- Season detail page schedule section:
  - `data-testid="generate-schedule-btn"` on the Generate Schedule button
  - `data-testid="clear-all-schedule-btn"` on the Clear All button
  - `data-testid="clear-all-confirm-btn"` on the clear-all dialog confirm button
  - `data-testid="schedule-entry-row"` on each matchday row in the schedule table
  - `data-testid="schedule-entry-status"` on the status chip/badge within each row
  - `data-testid="schedule-entry-match"` on the match display (team names) within each planned row
  - `data-testid="clear-match-btn"` on the clear button for each planned matchday row
  - `data-testid="clear-match-confirm-btn"` on the clear confirmation dialog confirm button
- Player absence dialog:
  - `data-testid="open-absences-btn"` on the absences icon button per player in the player list
  - `data-testid="absence-checkbox"` on each matchday checkbox in the dialog (use index or date suffix)
  - `data-testid="save-absences-btn"` on the dialog save button

### Test Setup Helpers (`tests/Winterplein.E2eTests/`)

- A private helper `SetupSeasonWithPlayersAsync(string seasonName, int playerCount, int matchdayCount)` that:
  - Adds the required players via the Players page
  - Creates a season with a date range spanning exactly `matchdayCount` occurrences of a chosen weekday
  - Enrolls all players in the season via the season detail page
  - Returns the season detail page URL
- Reused across all tests in this class to reduce boilerplate

## Technical Notes

- A 3-matchday season can be reliably created by picking a start date on the Monday before a known Tuesday and setting end date 3 weeks later (3 × same weekday = 3 matchdays). Use fixed test dates relative to a known anchor date rather than `DateTime.Today` to keep tests deterministic.
- `data-testid="schedule-entry-status"` text content will be "Planned" or "Open" — use `Expect(locator).ToHaveTextAsync("Planned")` for assertions.
- The absence-aware test (`AbsenceAwareGeneration_ExcludesAbsentPlayer`) requires at least 5 players enrolled so the scheduler has enough alternatives — with only 4 players, removing any one leaves zero valid matches and the test cannot assert "planned with different players."
- Player name assertions in schedule rows check that all four player names from a match are present in the row's text content; use `Expect(row).ToContainTextAsync(playerName)`.
- Each test navigates fresh and uses uniquely named seasons (append a short GUID suffix) to avoid cross-test state collisions in the in-memory store.
- The helper `SetupSeasonWithPlayersAsync` can reuse UI patterns established in Story 2 (`data-testid="player-name-input"`) and Story 3 (`data-testid="season-submit-btn"`, `data-testid="add-player-to-season-btn"`).

## Tasks

- [ ] T1: Add `data-testid` attributes to schedule section buttons, row status chips, match display cells, and clear confirmation dialogs (blockedBy: Story 1 T5)
- [ ] T2: Add `data-testid` attributes to player absence dialog open button, checkboxes, and save button (blockedBy: Story 1 T5)
- [ ] T3: Create `MatchSchedulePlanningTests.cs` with `[Collection("Playwright")]` and `SetupSeasonWithPlayersAsync` helper (blockedBy: Story 3 T4)
- [ ] T4: Write `GenerateSchedule_FillsAllMatchdays` test (blockedBy: T3, T1)
- [ ] T5: Write `GenerateSchedule_ShowsMatchDetails` test (blockedBy: T3, T1)
- [ ] T6: Write `ClearPlannedMatch_SetsMatchdayToOpen` test (blockedBy: T3, T1)
- [ ] T7: Write `ClearAllPlannedMatches_SetsAllMatchdaysToOpen` test (blockedBy: T3, T1)
- [ ] T8: Write `RegenerateSchedule_FillsOpenSlots` test (blockedBy: T3, T1)
- [ ] T9: Write `MarkAbsence_PlayerAbsenceDialogSavesCorrectly` test (blockedBy: T3, T2)
- [ ] T10: Write `AbsenceAwareGeneration_ExcludesAbsentPlayer` test (blockedBy: T3, T1, T2)
- [ ] T11: Verify all tests pass against running dev stack (blockedBy: T4, T5, T6, T7, T8, T9, T10)
