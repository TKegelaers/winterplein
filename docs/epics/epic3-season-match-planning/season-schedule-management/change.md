# season-schedule-management

## Problem Statement

The schedule can be generated but users cannot view the full matchday-by-matchday overview or clear individual / all planned matches. Without clear functionality, fixing scheduling mistakes requires recreating the entire season.

## Proposed Solution

Add a `GetSeasonScheduleQuery` that joins matchdays with planned matches into a schedule overview. Add `ClearPlannedMatchCommand` and `ClearAllPlannedMatchesCommand`. Display the full schedule table on the season detail page with clear actions.

## Business Requirements

**Given** a season schedule has been generated
**When** a user views the season detail page
**Then** they see each matchday with its assigned match (or "Open" status)

**Given** a planned match exists for a matchday
**When** the user clicks "Clear" for that matchday
**Then** the planned match is removed and the slot becomes open for re-generation

## Acceptance Criteria

- [ ] `MatchdayScheduleEntryDto` record in `Winterplein.Shared/DTOs/`: `Date`, `PlannedMatch?`, `IsPlanned`
- [ ] `GetSeasonScheduleQuery` handler: joins matchdays with planned matches, ordered by date; null for unknown season
- [ ] `ClearPlannedMatchCommand(SeasonId, Date)` handler: removes single planned match; returns false if nothing to clear
- [ ] `ClearAllPlannedMatchesCommand(SeasonId)` handler: removes all for season; returns false for unknown season
- [ ] `GET /api/seasons/{id}/schedule`, `DELETE /api/seasons/{id}/matchdays/{date}/planned-match`, `DELETE /api/seasons/{id}/schedule`
- [ ] Schedule overview section on season detail page: MudTable with Date, Match, Status chip; per-row clear button; "Clear All" button with confirmation
- [ ] Unit and integration tests including round-trip: generate → clear one → re-generate

## Technical Notes

- `{date}` in the DELETE route is `DateOnly` in ISO 8601 (e.g. `2026-04-14`); ASP.NET Core 10 handles `DateOnly` route params natively
- "Clear All" is idempotent — returns 204 even if schedule was already empty (as long as season exists)
