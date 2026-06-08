# test-season-management

## Problem Statement

Season management (CRUD, matchday listing, player enrollment) has no E2E coverage. Epic 2 will implement the full vertical slice, but without E2E tests there is no automated verification that the browser-to-API round trip works correctly for season workflows.

## Proposed Solution

Write a `SeasonManagementTests` class with seven E2E tests covering season CRUD, matchday listing, player enrollment, and deletion. Add `data-testid` attributes to season list, form, and detail page components.

## Business Requirements

**Given** a user navigates to the Seasons section
**When** they create, edit, enroll players, or delete seasons
**Then** the UI reflects the correct state after each operation

## Acceptance Criteria

- [ ] `SeasonManagementTests` class with `[Collection("Playwright")]`, extending `PageTest`
- [ ] `CreateSeason_AppearsInList` test
- [ ] `CreateSeason_ComputedMatchdayCount_IsCorrect` test (4-Tuesday span → 4 matchdays)
- [ ] `EditSeason_UpdatesName` test
- [ ] `SeasonDetail_ShowsMatchdays` test
- [ ] `EnrollPlayer_AppearsInSeasonPlayerList` test
- [ ] `RemovePlayer_DisappearsFromSeasonPlayerList` test
- [ ] `DeleteSeason_RemovedFromList` test
- [ ] `data-testid` attributes on season list, form, and detail page per story specification
- [ ] Tests use unique season names (GUID suffix) to avoid inter-test collisions

## Potential Pitfalls

- MudBlazor date pickers accept typed ISO date strings via `Page.FillAsync` — don't click the calendar UI
- MudBlazor dialogs render in a portal — use `Page.WaitForSelectorAsync` before interacting with confirmation dialogs
- Player enrollment tests require at least one player existing in the global store — add a player via the Players page as a setup step
- These tests depend on **Epic 2 being fully implemented**

## Dependencies

Story 1 (infrastructure), Story 2 (data-testid conventions), Epic 2 fully implemented
