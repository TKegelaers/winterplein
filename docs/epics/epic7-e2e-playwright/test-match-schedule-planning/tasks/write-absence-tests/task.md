# write-absence-tests

## Scope

Write the two absence-related E2E tests in `MatchSchedulePlanningTests`.

Tests to implement:

- `MarkAbsence_PlayerAbsenceDialogSavesCorrectly` — call `SetupSeasonWithPlayersAsync` with 4 players and 2 matchdays, open the absence dialog for the first player via `data-testid="absence-btn-{playerId}"`, check the first matchday checkbox, click `data-testid="absence-save-btn"`, reopen the dialog and assert the checkbox is still checked
- `AbsenceAwareGeneration_ExcludesAbsentPlayer` — call `SetupSeasonWithPlayersAsync` with 5 players and 2 matchdays, mark the first player absent on all matchdays, generate the schedule, assert that the first player's name does not appear in any `data-testid="schedule-entry-status"` row that is "Planned"

Implementation notes:

- `AbsenceAwareGeneration_ExcludesAbsentPlayer` requires exactly 5 enrolled players so the scheduler has valid alternatives after excluding one player from all matchdays.
- Use fixed anchor dates (not `DateTime.Today`) to keep matchday counts deterministic.

## Domain model changes

None.

## Test cases

The tests listed above are the test cases.

## Affected files

- modify: `tests/Winterplein.E2eTests/MatchSchedulePlanningTests.cs`
