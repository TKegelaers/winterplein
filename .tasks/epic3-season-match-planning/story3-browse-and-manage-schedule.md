# Story 3 — Browse & Manage Matchday Schedule

**Epic:** Epic 3 — Season Match Planning
**Dependencies:** Story 2

## Description

Allow a user to see the full matchday schedule for a season — which matchdays have a planned match and which are still open — and to clear a planned match to free up the slot for a different selection.

## Acceptance Criteria

### DTOs (`Winterplein.Shared/DTOs/`)

- **`MatchdayScheduleEntryDto`** — `record MatchdayScheduleEntryDto(DateOnly Date, PlannedMatchDto? PlannedMatch, bool IsPlanned)`

### CQRS (`Winterplein.Application/PlannedMatches/`)

- **`GetSeasonScheduleQuery(int SeasonId)`** → `List<MatchdayScheduleEntryDto>?`
  - Combines `season.GetMatchdays()` with `IPlannedMatchRepository.GetAllBySeason()` to build the schedule
  - Returns null if season not found
- **`ClearPlannedMatchCommand(int SeasonId, DateOnly Date)`** → `bool`
  - Deletes the planned match for the given matchday
  - Returns false if no planned match exists

### API Endpoints (on `SeasonsController`)

| Method | Route | Request | Response |
|--------|-------|---------|----------|
| GET | `/api/seasons/{id}/schedule` | — | `List<MatchdayScheduleEntryDto>` 200 / 404 |
| DELETE | `/api/seasons/{id}/matchdays/{date}/planned-match` | — | 204 / 404 |

- GET returns 404 if season not found
- DELETE returns 404 if no planned match exists for that matchday

### Blazor UI (`Winterplein.Client/`)

- Add `GetScheduleAsync` and `ClearPlannedMatchAsync` methods to `SeasonApiClient`
- **Schedule Overview** section on season detail page (`/seasons/{id}`):
  - `MudTable` with columns: Date, Planned Match (Team 1 vs Team 2 or "—"), Status (`MudChip`: Planned / Open), Actions
  - "Plan" link for open matchdays → navigates to matchday detail page (Story 2)
  - "View" link for planned matchdays → navigates to matchday detail page
  - "Clear" button for planned matchdays with `MudDialog` confirmation → removes the planned match

### Tests

- Unit tests: `GetSeasonScheduleQueryHandler` — returns entries with correct `IsPlanned`, returns null for unknown season; `ClearPlannedMatchCommandHandler` — clears match, returns false when not found
- Integration tests: GET schedule → 200 with correct status after planning, DELETE → 204, DELETE when not planned → 404

## Technical Notes

- The schedule overview replaces/extends the matchday list from Epic 2's season detail page
- Clearing a planned match frees the slot — the user can then navigate to the matchday detail page and select a different match

## Tasks

- [ ] T1: Create `MatchdayScheduleEntryDto` record in `Winterplein.Shared/DTOs/`
- [ ] T2: Create `GetSeasonScheduleQuery` + handler (blockedBy: T1, Story 2 T3)
- [ ] T3: Create `ClearPlannedMatchCommand` + handler (blockedBy: Story 2 T3)
- [ ] T4: Implement GET schedule and DELETE planned-match API actions (blockedBy: T2, T3, Story 2 T5)
- [ ] T5: Add `GetScheduleAsync` and `ClearPlannedMatchAsync` to `SeasonApiClient` (blockedBy: T4)
- [ ] T6: Add schedule overview section to season detail page with table, plan/view/clear actions (blockedBy: T5, Story 2 T11)
- [ ] T7: Write unit tests for `GetSeasonScheduleQueryHandler` and `ClearPlannedMatchCommandHandler` (blockedBy: T2, T3)
- [ ] T8: Write integration tests for GET schedule and DELETE endpoints (blockedBy: T4)
