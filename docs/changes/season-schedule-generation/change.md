# season-schedule-generation

## Problem Statement

A season has matchdays but no mechanism to automatically assign a unique match to each one. Manually selecting a match per matchday would be impractical given the large pool size.

## Proposed Solution

Implement a `GenerateScheduleCommand` that randomly assigns one unique match from the pool to each open matchday. A `PlannedMatch` entity stores a full match snapshot. The generation is idempotent — re-running only fills remaining open matchdays.

## Business Requirements

**Given** a season with enrolled players and matchdays
**When** `POST /api/seasons/{id}/schedule/generate` is called
**Then** each open matchday receives one unique match from the pool; already-planned matchdays are untouched

**Given** the pool has fewer matches than open matchdays
**When** schedule generation runs
**Then** as many matchdays as possible are filled and `OpenCount` reflects the remainder

## Acceptance Criteria

- [ ] `PlannedMatch` entity: `Id`, `SeasonId`, `Date`, `Match` (full snapshot); constructor validation (null match, default date)
- [ ] `PlannedMatchDto` and `GenerateScheduleResponse` in `Winterplein.Shared/DTOs/`
- [ ] `IPlannedMatchRepository`: `GetAllBySeason`, `Add`, `Delete(seasonId, date)`, `DeleteAllBySeason`
- [ ] `InMemoryPlannedMatchRepository` using `ConcurrentDictionary`, registered as Singleton
- [ ] `GenerateScheduleCommand` handler: shuffles unused pool matches (Fisher-Yates), assigns one per open matchday, persists, returns `GenerateScheduleResponse`
- [ ] `POST /api/seasons/{id}/schedule/generate` — 200 with response or 404
- [ ] "Generate Schedule" button on season detail page with snackbar feedback
- [ ] `PlannedMatchBuilder` in test commons; unit and integration tests

## Potential Pitfalls

- `PlannedMatch` stores a full `Match` snapshot (not just an ID) so it remains readable if the player list changes later
- Uniqueness tracked by `Match.Id` from the current pool vs existing snapshots — if player list changes, pool IDs shift and there are no false collisions
- Randomization uses `Random.Shared` (thread-safe); unit tests verify structural invariants, not specific random outputs
