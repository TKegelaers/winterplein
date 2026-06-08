# absence-aware-schedule

## Problem Statement

The schedule generator ignores player absences. It can assign matches containing players who are marked absent on that matchday, producing an invalid schedule.

## Proposed Solution

Modify `GenerateScheduleCommandHandler` to load all season absences upfront and filter the available match pool per matchday — excluding matches where any of the 4 players is absent on that date.

## Business Requirements

**Given** player A is marked absent on matchday 1
**When** the schedule is generated
**Then** matchday 1 is assigned a match that does not include player A

**Given** no valid matches exist for a matchday (all players absent in every remaining match)
**When** the schedule is generated
**Then** that matchday remains open and `OpenCount` is incremented

## Acceptance Criteria

- [ ] `GenerateScheduleCommandHandler` accepts `IPlayerAbsenceRepository` as an additional injected parameter
- [ ] Handler loads all season absences once via `GetBySeason(seasonId)` before the assignment loop
- [ ] Per-matchday: filter unused matches to those where none of the 4 players is in that day's absent set
- [ ] Unit tests: skips absent-player matches; same match usable on another day where player is available; all players absent → open slot; no absences → same behavior as before
- [ ] Integration tests: set absences → generate → verify no absent player appears in a planned match

## Potential Pitfalls

- Filter is per-matchday (not global) — a match excluded on day 1 due to absence can still be used on day 2
- Load all absences once upfront (single `GetBySeason` call) rather than one `GetBySeasonAndDate` call per matchday
- Process matchdays in chronological order — sufficient for current scale; no priority re-ordering needed
