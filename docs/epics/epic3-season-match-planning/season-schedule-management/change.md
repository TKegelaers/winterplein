# season-schedule-management

## Problem Statement

The schedule can be generated but users cannot view the full matchday-by-matchday overview or clear individual / all planned matches. Without clear functionality, fixing scheduling mistakes requires recreating the entire season.

## Proposed Solution

Add a `GetSeasonScheduleQuery` that joins a season's computed matchdays with its planned matches into an ordered matchday-by-matchday overview (each matchday is either planned or open). Add `ClearPlannedMatchCommand` (one matchday) and `ClearAllPlannedMatchesCommand` (whole season). On the season detail page, **replace** the flat planned-matches list added in Story 2 with this full schedule view, adding per-row Clear and a Clear All action. Cleared slots become open again for re-generation.

Clear handlers follow the existing delete-command convention (void Wolverine `Handle`, signal not-found by throwing `KeyNotFoundException` which the global handler maps to 404) — they do not return value types.

## Business Requirements

**Given** a season exists
**When** a user views the season detail page
**Then** they see every matchday in date order, each showing its assigned match or an "Open" status

**Given** a planned match exists for a matchday
**When** the user clears that matchday
**Then** the planned match is removed (204) and the slot becomes open for re-generation

**Given** a matchday has no planned match (or the season is unknown)
**When** a clear of that single matchday is requested
**Then** HTTP 404 is returned

**Given** a season exists
**When** Clear All is requested
**Then** all planned matches for the season are removed and HTTP 204 is returned, even if the schedule was already empty (idempotent); an unknown season returns 404

## Acceptance Criteria

- [ ] `MatchdayScheduleEntryDto(DateOnly Date, PlannedMatchDto? PlannedMatch, bool IsPlanned)` and a `SeasonScheduleResponse` (ordered entries) in `Winterplein.Application.IO/DTOs/`; `GetSeasonScheduleQuery(int SeasonId)` in `Queries/`, `ClearPlannedMatchCommand(int SeasonId, DateOnly Date)` and `ClearAllPlannedMatchesCommand(int SeasonId)` in `Commands/`
- [ ] `GetSeasonScheduleQuery` handler: builds entries from `Season.GetMatchdays()` joined with planned matches (by date), ordered by date; returns `null` for unknown season (→ 404)
- [ ] `ClearPlannedMatchCommand` handler (void): removes the planned match for that season+date; throws `KeyNotFoundException` if the season is unknown or no planned match exists at that date (→ 404)
- [ ] `ClearAllPlannedMatchesCommand` handler (void): removes all planned matches for the season; throws `KeyNotFoundException` only for an unknown season; idempotent otherwise (→ 204)
- [ ] `IPlannedMatchRepository` gains async delete methods (delete by season+date, delete all by season); EF implementation
- [ ] Endpoints: `GET /api/seasons/{id}/schedule` (200 / 404), `DELETE /api/seasons/{id}/matchdays/{date}/planned-match` (204 / 404), `DELETE /api/seasons/{id}/schedule` (204 / 404); matching `SeasonApiClient` methods
- [ ] Season detail page: the Story-2 flat planned list is replaced by a matchday-by-matchday `MudTable` (Date, Match, Status chip), with a per-row Clear button (planned rows only) and a "Clear All" button with confirmation; loads via `GetSeasonScheduleQuery`
- [ ] Unit and integration tests, including a round-trip: generate → clear one → re-generate (the cleared matchday is refilled); clear-empty → 404; Clear All idempotent → 204

## Technical Notes

- `{date}` in the DELETE route is `DateOnly` in ISO 8601 (e.g. `2026-04-14`); ASP.NET Core 10 binds `DateOnly` route params natively (this is the first route in the app to do so — verify binding in an integration test)
- Wolverine handlers cannot return value types, so the clear commands are void and use the established `KeyNotFoundException` → 404 pattern (see `DeleteSeasonCommandHandler` / `RemoveSeasonPlayerCommandHandler`); the repository (not a Wolverine handler) may return a bool/int to let the handler decide whether to throw
- Repositories are async EF Core against SQL Server; integration tests run against the real `Winterplein_integrationTests` DB with Respawn
- DTOs/commands live in `Winterplein.Application.IO` (the older `Winterplein.Shared` name no longer exists after the Epic 8 rename)
