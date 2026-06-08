# write-generate-clear-tests

## Scope

Write the five schedule generation and clearing E2E tests in `MatchSchedulePlanningTests`. Each test calls `SetupSeasonWithPlayersAsync` then exercises the schedule UI.

Tests to implement:

- `GenerateSchedule_FillsAllMatchdays` — generate schedule, assert all `data-testid="schedule-entry-status"` chips show "Planned"
- `GenerateSchedule_ShowsMatchDetails` — generate schedule, assert at least one entry shows player names from the enrolled set
- `ClearPlannedMatch_SetsMatchdayToOpen` — generate schedule, click the first `data-testid="clear-match-btn"`, assert that entry's status chip changes to "Open"
- `ClearAllPlannedMatches_SetsAllMatchdaysToOpen` — generate schedule, click `data-testid="clear-all-btn"`, assert all chips show "Open"
- `RegenerateSchedule_FillsOpenSlots` — generate schedule, clear one match, generate again, assert all chips show "Planned"

## Domain model changes

None.

## Test cases

The tests listed above are the test cases.

## Affected files

- create: `tests/Winterplein.E2eTests/MatchSchedulePlanningTests.cs`
