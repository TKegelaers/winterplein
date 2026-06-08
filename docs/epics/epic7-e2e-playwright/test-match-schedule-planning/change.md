# test-match-schedule-planning

## Problem Statement

Match schedule planning (generate schedule, clear matches, absence management, absence-aware generation) has no E2E coverage. This is the highest-level integration check of the scheduling domain and cannot be verified by unit or integration tests alone.

## Proposed Solution

Write a `MatchSchedulePlanningTests` class with seven E2E tests covering schedule generation, clearing, regeneration, absence recording, and absence-aware generation. Add a `SetupSeasonWithPlayersAsync` helper to reduce test setup boilerplate.

## Business Requirements

**Given** a season with enrolled players exists
**When** a user generates or clears a schedule, or marks absences
**Then** the schedule and absence state reflect the expected changes

## Acceptance Criteria

- [ ] `MatchSchedulePlanningTests` class with `[Collection("Playwright")]`, extending `PageTest`
- [ ] `GenerateSchedule_FillsAllMatchdays` test
- [ ] `GenerateSchedule_ShowsMatchDetails` test
- [ ] `ClearPlannedMatch_SetsMatchdayToOpen` test
- [ ] `ClearAllPlannedMatches_SetsAllMatchdaysToOpen` test
- [ ] `RegenerateSchedule_FillsOpenSlots` test
- [ ] `MarkAbsence_PlayerAbsenceDialogSavesCorrectly` test
- [ ] `AbsenceAwareGeneration_ExcludesAbsentPlayer` test (requires ≥5 enrolled players)
- [ ] `data-testid` attributes on schedule section, status chips, clear buttons, absence dialog
- [ ] `SetupSeasonWithPlayersAsync(string seasonName, int playerCount, int matchdayCount)` helper reused across tests

## Potential Pitfalls

- Absence-aware test requires ≥5 enrolled players so the scheduler has valid alternatives after excluding one
- Use fixed anchor dates (not DateTime.Today) for season creation to keep matchday counts deterministic
- `data-testid="schedule-entry-status"` text will be "Planned" or "Open" — use `Expect(locator).ToHaveTextAsync`
- These tests depend on **Epics 3 and 4 being fully implemented**

## Dependencies

Story 1 (infrastructure), Story 2 (data-testid conventions), Story 3 (setup patterns), Epics 3+4 implemented
