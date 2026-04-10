# Story 1 — View Season Match Pool

**Epic:** Epic 3 — Season Match Planning
**Dependencies:** Epic 2 (Season entity with Players collection, ISeasonRepository, Season API + UI)

## Description

Allow a user to view all possible doubles matches generated from a season's enrolled players. This pool is computed on-the-fly using the existing match generator and serves as the source from which individual matchday matches are planned in Story 2.

## Acceptance Criteria

### CQRS (`Winterplein.Application/MatchdayPlans/`)

- **`GetSeasonMatchPoolQuery(int SeasonId)`** → `GenerateMatchesResponse?`
  - Loads season via `ISeasonRepository`, gets enrolled players
  - Generates all possible matches using existing `IMatchGeneratorService.GenerateAllMatches()`
  - Returns null if season not found
  - Returns empty list if fewer than 4 enrolled players

### API Endpoints (on `SeasonsController`)

| Method | Route                          | Request | Response                            |
| ------ | ------------------------------ | ------- | ----------------------------------- |
| GET    | `/api/seasons/{id}/match-pool` | —       | `GenerateMatchesResponse` 200 / 404 |

### Blazor UI (`Winterplein.Client/`)

- Add `GetMatchPoolAsync(int seasonId)` method to `SeasonApiClient`
- **Match Pool section** on season detail page (`/seasons/{id}`) — collapsible `MudExpansionPanel` or similar showing the total match count and a `MudTable` of all possible matches (Match #, Team 1, Team 2)

### Tests

- Unit tests: `GetSeasonMatchPoolQueryHandler` — returns matches for enrolled players, returns null for unknown season, returns empty for fewer than 4 players
- Integration tests: GET match pool → 200 with matches, GET unknown season → 404

## Technical Notes

- The match pool is **not persisted** — it is computed on-the-fly from the enrolled players using the existing `IMatchGeneratorService`
- Reuses the existing `GenerateMatchesResponse` DTO (contains `List<MatchDto>` and `TotalCount`)
- Match IDs in the pool are deterministic (sequential, based on generation order) for a given player set — Story 2 uses these IDs internally for uniqueness tracking during schedule generation

## Tasks

- [ ] T1: Create `GetSeasonMatchPoolQuery` + handler (reuses `ISeasonRepository` + `IMatchGeneratorService`)
- [ ] T2: Implement GET `/api/seasons/{id:int}/match-pool` action on `SeasonsController` (blockedBy: T1)
- [ ] T3: Add `GetMatchPoolAsync` to `SeasonApiClient` (blockedBy: T2)
- [ ] T4: Add match pool section to season detail page with `MudTable` (blockedBy: T3)
- [ ] T5: Write unit tests for `GetSeasonMatchPoolQueryHandler` (blockedBy: T1)
- [ ] T6: Write integration tests for GET match-pool endpoint (blockedBy: T2)
