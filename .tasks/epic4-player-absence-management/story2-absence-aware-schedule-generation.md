# Story 2 — Absence-Aware Schedule Generation

**Epic:** Epic 4 — Player Absence Management
**Dependencies:** Story 1, Epic 3 Story 2 (GenerateScheduleCommand)

## Description

Modify the automatic schedule generator so that for each matchday, it only picks matches where all four players are available. A match is excluded from a matchday if any of its players is marked absent on that date. Matches excluded on one matchday may still be used on other matchdays where those players are available.

## Acceptance Criteria

### CQRS (`Winterplein.Application/PlannedMatches/`)

- **Modify `GenerateScheduleCommandHandler`** to accept `IPlayerAbsenceRepository` via constructor injection
- **Updated algorithm:**
  1. Load season, get matchdays, generate full match pool (unchanged)
  2. Load all absences for the season via `IPlayerAbsenceRepository.GetBySeason(seasonId)`
  3. Build a lookup: `Dictionary<DateOnly, HashSet<int>>` mapping each matchday to its set of absent player IDs
  4. Track used match IDs across all matchdays (unchanged — uniqueness constraint)
  5. For each open matchday:
     - Get absent player IDs for that date from the lookup
     - Filter unused matches to those where none of the 4 players (Team1.Player1, Team1.Player2, Team2.Player1, Team2.Player2) is in the absent set
     - Shuffle the filtered candidates and pick one
     - Mark it as used
  6. Persist and return response (unchanged)
- **`GenerateScheduleResponse`** unchanged — `OpenCount` naturally reflects matchdays that couldn't be filled (whether due to pool exhaustion or absences)

### Blazor UI (`Winterplein.Client/`)

- No new UI required — the schedule view (Epic 3 Story 3) already shows planned/open matchdays
- Optional enhancement: if `OpenCount > 0` after generation, the snackbar message could mention absences as a possible reason (e.g. "Scheduled {n} matches, {m} matchdays could not be filled")

### Tests

- **Unit tests** for `GenerateScheduleCommandHandler` (new test cases, added to existing test class):
  - Skips match when a player is absent on that matchday (set up: 4 players, 2 matchdays, player A absent on matchday 1 → matchday 1 only gets matches without player A)
  - Same match can be used on a different matchday where the player is available (match excluded from day 1 due to absence can still appear on day 2)
  - All players absent on a matchday → no match assigned, `OpenCount` incremented
  - No absences → behavior identical to before (regression check)
  - Absences only for players not in the pool → no effect (e.g. absence recorded for a player who was later removed from the season)
- **Integration tests**:
  - Set absences → generate schedule → verify matchday with absent player has a valid match (no absent players in it), or is open if no valid matches exist
  - Generate with no absences → same behavior as before (regression)

## Technical Notes

- The key change is that match filtering is now **per-matchday** rather than global. Previously, the handler shuffled the unused pool once and assigned sequentially. Now, for each matchday, it filters the unused pool by that day's absences before picking.
- The order in which matchdays are processed matters: matchdays with more absences (fewer available matches) could be processed first to maximize total assignments. However, for simplicity, process in chronological order — this is sufficient for the current scale and avoids over-engineering.
- The `GetBySeason` call loads all absences once upfront rather than calling `GetBySeasonAndDate` per matchday — this is more efficient (single repository call instead of N).
- No changes needed to `PlannedMatch`, `PlannedMatchDto`, `IPlannedMatchRepository`, or the API endpoints — the filtering is entirely internal to the handler.

## Tasks

- [ ] T1: Add `IPlayerAbsenceRepository` to `GenerateScheduleCommandHandler` constructor (blockedBy: Story 1 T3)
- [ ] T2: Implement per-matchday absence filtering in the handler's assignment loop (blockedBy: T1)
- [ ] T3: Write unit tests for absence-aware generation scenarios (blockedBy: T2, Story 1 T12)
- [ ] T4: Write integration tests for absence + schedule round-trip (blockedBy: T2, Story 1 T9)
