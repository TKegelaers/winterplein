# Story 3 — Test Season Management

**Epic:** Epic 7 — E2E Tests with Playwright
**Dependencies:** Story 1 (Playwright infrastructure), Story 2 (data-testid conventions established); requires Epic 2 to be fully implemented

## Description

Write E2E tests covering the season management flows: creating a season, viewing it in the list, editing it, browsing computed matchdays on the detail page, enrolling and removing players, and deleting a season. These tests validate the full vertical slice from the Blazor UI through the API to the in-memory repository and back.

## Acceptance Criteria

### Test Class (`tests/Winterplein.E2eTests/`)

- `SeasonManagementTests` class annotated with `[Collection("Playwright")]`, extending `PageTest`
- Tests cover:
  - **`CreateSeason_AppearsInList`** — fill the create form with a valid season, submit, assert the season name appears in the list at `/seasons`
  - **`CreateSeason_ComputedMatchdayCount_IsCorrect`** — create a season spanning exactly 4 Tuesdays, assert the matchday count preview shows 4 before submission and the detail page lists 4 matchday dates
  - **`EditSeason_UpdatesName`** — create a season, navigate to edit, change the name, save, assert updated name appears in the list
  - **`SeasonDetail_ShowsMatchdays`** — navigate to a season's detail page, assert at least one matchday date row is visible
  - **`EnrollPlayer_AppearsInSeasonPlayerList`** — create a season, navigate to its detail page, add a player via the enrollment control, assert the player appears in the enrolled players table
  - **`RemovePlayer_DisappearsFromSeasonPlayerList`** — enroll a player, then remove them via the confirmation dialog, assert they are no longer in the enrolled players table
  - **`DeleteSeason_RemovedFromList`** — create a season, delete it via the confirmation dialog on the list page, assert it no longer appears in the list
- Each test that requires pre-existing data (players, seasons) creates it via UI actions within the test, not via direct API calls

### data-testid Attributes (`src/Winterplein.Client/`)

- Season list page (`/seasons`):
  - `data-testid="season-list-item"` on each table row
  - `data-testid="create-season-btn"` on the create button
  - `data-testid="edit-season-btn"` on each row's edit button
  - `data-testid="delete-season-btn"` on each row's delete button
  - `data-testid="delete-confirm-btn"` on the confirmation dialog's confirm button
- Season form (`SeasonForm.razor`):
  - `data-testid="season-name-input"` on the name field
  - `data-testid="season-start-date-input"` on the start date picker
  - `data-testid="season-end-date-input"` on the end date picker
  - `data-testid="season-weekday-select"` on the weekday dropdown
  - `data-testid="season-submit-btn"` on the submit button
  - `data-testid="matchday-count-preview"` on the computed matchday count preview
- Season detail page (`/seasons/{id}`):
  - `data-testid="matchday-list-item"` on each matchday row
  - `data-testid="season-player-list-item"` on each enrolled player row
  - `data-testid="remove-player-btn"` on each player's remove button
  - `data-testid="remove-player-confirm-btn"` on the confirmation dialog's confirm button
  - `data-testid="player-enrollment-select"` on the player selector dropdown
  - `data-testid="add-player-to-season-btn"` on the enroll button

## Technical Notes

- Season creation requires date inputs; use `Page.FillAsync` with ISO date strings (e.g. `"2025-09-01"`) rather than clicking the date picker calendar — MudBlazor date pickers accept typed input.
- The weekday dropdown is a `MudSelect<DayOfWeek>` — use `Page.SelectOptionAsync` or `Page.ClickAsync` on the option after opening the dropdown; test with a fixed weekday (e.g. Tuesday = `"2"`) to make matchday count assertions deterministic.
- Tests that create seasons should use unique names (e.g. append a timestamp or GUID) to avoid inter-test collisions when the in-memory store accumulates state across tests in the same run.
- MudBlazor dialogs render in a portal outside the component tree; use `Page.WaitForSelectorAsync("[data-testid='delete-confirm-btn']")` after clicking the delete button before asserting or interacting with the dialog.
- Player enrollment tests depend on at least one player existing in the global player store — add a player via the Players page as a test setup step before navigating to the season detail.
- These tests depend on **Epic 2 being implemented** (Season domain, CQRS, API, and Blazor UI all complete).

## Tasks

- [ ] T1: Add `data-testid` attributes to season list page buttons, rows, and confirmation dialog (blockedBy: Story 1 T5)
- [ ] T2: Add `data-testid` attributes to `SeasonForm.razor` fields and submit button (blockedBy: T1)
- [ ] T3: Add `data-testid` attributes to season detail page matchday list, player list, enrollment controls (blockedBy: T1)
- [ ] T4: Create `SeasonManagementTests.cs` extending `PageTest` with `[Collection("Playwright")]` (blockedBy: Story 1 T5)
- [ ] T5: Write `CreateSeason_AppearsInList` test (blockedBy: T4, T1, T2)
- [ ] T6: Write `CreateSeason_ComputedMatchdayCount_IsCorrect` test (blockedBy: T4, T2, T3)
- [ ] T7: Write `EditSeason_UpdatesName` test (blockedBy: T4, T1, T2)
- [ ] T8: Write `SeasonDetail_ShowsMatchdays` test (blockedBy: T4, T3)
- [ ] T9: Write `EnrollPlayer_AppearsInSeasonPlayerList` test (blockedBy: T4, T3, Story 2 T2)
- [ ] T10: Write `RemovePlayer_DisappearsFromSeasonPlayerList` test (blockedBy: T4, T3, Story 2 T2)
- [ ] T11: Write `DeleteSeason_RemovedFromList` test (blockedBy: T4, T1)
- [ ] T12: Verify all tests pass against running dev stack (blockedBy: T5, T6, T7, T8, T9, T10, T11)
