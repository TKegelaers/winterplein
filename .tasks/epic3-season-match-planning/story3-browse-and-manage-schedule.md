# Story 3 — Browse & Manage Schedule

**Epic:** Epic 3 — Season Match Planning
**Dependencies:** Story 2

## Description

Allow a user to see the full matchday schedule for a season — which matchdays have a planned match and which are still open — and to clear individual planned matches or the entire schedule. After clearing, the user can re-generate to fill open slots.

## Acceptance Criteria

### DTOs (`Winterplein.Shared/DTOs/`)

- **`MatchdayScheduleEntryDto`** — `record MatchdayScheduleEntryDto(DateOnly Date, PlannedMatchDto? PlannedMatch, bool IsPlanned)`

### CQRS (`Winterplein.Application/PlannedMatches/`)

- **`GetSeasonScheduleQuery(int SeasonId)`** → `List<MatchdayScheduleEntryDto>?`
  - Loads season via `ISeasonRepository` — returns null if not found
  - Gets matchdays via `season.GetMatchdays()`
  - Gets planned matches via `IPlannedMatchRepository.GetAllBySeason()`
  - Joins: for each matchday, finds any matching planned match by date, sets `IsPlanned` accordingly
  - Returns entries ordered by date
- **`ClearPlannedMatchCommand(int SeasonId, DateOnly Date)`** → `bool`
  - Calls `IPlannedMatchRepository.Delete(seasonId, date)`
  - Returns false if no planned match exists for that matchday
- **`ClearAllPlannedMatchesCommand(int SeasonId)`** → `bool`
  - Loads season to verify it exists — returns false if not found
  - Calls `IPlannedMatchRepository.DeleteAllBySeason(seasonId)`
  - Returns true if season exists (even if no planned matches existed — idempotent)

### API Endpoints (on `SeasonsController`)

| Method | Route                                              | Request | Response                                   |
| ------ | -------------------------------------------------- | ------- | ------------------------------------------ |
| GET    | `/api/seasons/{id}/schedule`                       | —       | `List<MatchdayScheduleEntryDto>` 200 / 404 |
| DELETE | `/api/seasons/{id}/matchdays/{date}/planned-match` | —       | 204 / 404                                  |
| DELETE | `/api/seasons/{id}/schedule`                       | —       | 204 / 404                                  |

- GET returns 404 if season not found
- DELETE single returns 404 if no planned match exists for that matchday; 204 on success
- DELETE all returns 404 if season not found; 204 otherwise (even if schedule was already empty)

### Blazor UI (`Winterplein.Client/`)

- Add `GetScheduleAsync`, `ClearPlannedMatchAsync(int seasonId, DateOnly date)`, and `ClearAllPlannedMatchesAsync(int seasonId)` to `SeasonApiClient`
- **Schedule Overview** section on season detail page (`/seasons/{id}`):
  - Section header with "Schedule" title, "Generate Schedule" `MudButton` (from Story 2), and "Clear All" `MudButton` (outlined, Color.Error)
  - `MudTable` with columns: #, Date, Match (Team 1 vs Team 2 or "—"), Status (`MudChip`: "Planned" in green / "Open" in default)
  - "Clear" `MudIconButton` (delete icon, Color.Error) for planned matchdays — with `MudDialog` confirmation
  - "Clear All" button — with `MudDialog` confirmation ("Clear the entire schedule? This will remove all planned matches.")
  - After any clear or generate action: refresh the schedule table

### Tests

- **Unit tests**:
  - `GetSeasonScheduleQueryHandler` — returns null for unknown season; returns entries with correct `IsPlanned` (mix of planned and open); entries are ordered by date
  - `ClearPlannedMatchCommandHandler` — returns true when cleared, false when nothing to clear
  - `ClearAllPlannedMatchesCommandHandler` — returns false for unknown season; returns true and clears all for known season
- **Integration tests**:
  - GET schedule → 200 with entries after generating schedule
  - GET schedule → 200 with all-open entries for empty schedule
  - GET schedule → 404 for unknown season
  - DELETE single → 204 after generating (clears one match)
  - DELETE single → 404 when nothing planned for that date
  - DELETE all → 204 clears entire schedule
  - DELETE all → 404 for unknown season
  - Round-trip: generate → clear one → re-generate (verify cleared slot gets filled, others remain, uniqueness preserved)

## Technical Notes

- The schedule overview replaces/extends the matchday list from Epic 2's season detail page
- Clearing a planned match frees the slot for re-generation — the "Generate Schedule" button (Story 2) only fills open matchdays, enabling partial re-scheduling
- The round-trip integration test validates both partial regeneration behavior and the uniqueness constraint together
- `{date}` in the DELETE route is `DateOnly` in ISO 8601 format (e.g. `2026-04-14`) — ASP.NET Core 10 handles `DateOnly` route parameters natively

## Tasks

- [ ] T1: Create `MatchdayScheduleEntryDto` record in `Winterplein.Shared/DTOs/`
- [ ] T2: Create `GetSeasonScheduleQuery` + handler (blockedBy: T1, Story 2 T3)
- [ ] T3: Create `ClearPlannedMatchCommand` + handler (blockedBy: Story 2 T3)
- [ ] T4: Create `ClearAllPlannedMatchesCommand` + handler (blockedBy: Story 2 T3)
- [ ] T5: Implement GET schedule, DELETE single, and DELETE all API actions on `SeasonsController` (blockedBy: T2, T3, T4, Story 2 T5)
- [ ] T6: Add `GetScheduleAsync`, `ClearPlannedMatchAsync`, and `ClearAllPlannedMatchesAsync` to `SeasonApiClient` (blockedBy: T5)
- [ ] T7: Add schedule overview section to season detail page with table, clear/clear-all actions, and generate button (blockedBy: T6, Story 2 T10)
- [ ] T8: Write unit tests for all three handlers (blockedBy: T2, T3, T4)
- [ ] T9: Write integration tests including round-trip scenario (blockedBy: T5)
