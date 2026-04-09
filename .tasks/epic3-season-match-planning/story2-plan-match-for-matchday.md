# Story 2 — Plan a Match for a Matchday

**Epic:** Epic 3 — Season Match Planning
**Dependencies:** Story 1

## Description

Allow a user to select one match from the season's match pool and assign it to a specific matchday. Each matchday has exactly one planned match. The match is identified by its ID from the generated pool and persisted as a full match snapshot so it remains valid even if the player list later changes.

## Acceptance Criteria

### Domain (`Winterplein.Domain/Entities/PlannedMatch.cs`)

- `Id` (int), `SeasonId` (int), `Date` (DateOnly), `Match` (Match)
- Constructor accepts `int id`, `int seasonId`, `DateOnly date`, `Match match`
- Validation: `match` must not be null, `date` must not be default

### DTOs (`Winterplein.Shared/DTOs/`)

- **`PlannedMatchDto`** — `record PlannedMatchDto(int Id, int SeasonId, DateOnly Date, MatchDto Match)`
- **`PlanMatchRequest`** — `record PlanMatchRequest(int MatchId)` — references a match by its ID in the generated pool

### CQRS & Repository (`Winterplein.Application/`)

- **`IPlannedMatchRepository`** in `Interfaces/` — `GetBySeasonAndDate`, `GetAllBySeason`, `Add`, `Delete`
- **`PlanMatchCommand(int SeasonId, DateOnly Date, int MatchId)`** → `PlannedMatchDto?`
  - Loads season, validates date is a valid matchday via `GetMatchdays()`
  - Generates the match pool, finds the match by `MatchId`
  - Creates `PlannedMatch`, persists via repository
  - Returns null if season not found; throws if date is invalid, match ID not found in pool, or matchday already has a planned match
- **`GetPlannedMatchQuery(int SeasonId, DateOnly Date)`** → `PlannedMatchDto?`
- **`PlannedMatchMapper`** in `Mappers/` — `ToDto(PlannedMatch)` using existing `MatchMapper`

### Infrastructure (`Winterplein.Infrastructure/Persistence/`)

- **`InMemoryPlannedMatchRepository`** backed by `ConcurrentDictionary`, keyed by `(SeasonId, Date)`
- Registered as Singleton in DI

### API Endpoints (on `SeasonsController`)

| Method | Route | Request | Response |
|--------|-------|---------|----------|
| POST | `/api/seasons/{id}/matchdays/{date}/planned-match` | `PlanMatchRequest` | `PlannedMatchDto` 201 / 404 / 400 / 409 |
| GET | `/api/seasons/{id}/matchdays/{date}/planned-match` | — | `PlannedMatchDto` 200 / 404 |

- POST returns 404 if season not found, 400 if invalid date or match ID not in pool, 409 if matchday already has a planned match
- `{date}` is `DateOnly` in ISO 8601 format (e.g. `2026-04-14`)

### Blazor UI (`Winterplein.Client/`)

- Add `PlanMatchAsync` and `GetPlannedMatchAsync` methods to `SeasonApiClient`
- **Matchday Detail Page** (`/seasons/{id}/matchdays/{date}`) — header with season name + date, match selector (`MudSelect<MatchDto>` populated from match pool, filtered to exclude already-planned matches from other matchdays), "Plan Match" button, display of planned match (Team 1 vs Team 2), "Back to Season" link

### Tests

- `PlannedMatchBuilder` in `tests/Winterplein.UnitTests.Common/Builders/`
- Unit tests: `PlannedMatch` domain (constructor, validation), `PlanMatchCommandHandler` (happy path, unknown season, invalid date, unknown match ID, already planned), `GetPlannedMatchQueryHandler` (found, not found)
- Integration tests: POST plan → 201, GET planned match → 200, POST unknown season → 404, POST invalid date → 400, POST duplicate → 409

## Technical Notes

- The `PlannedMatch` stores a full `Match` snapshot (with `Team` and `Player` objects) — not just a match ID reference. This ensures the planned match remains readable even if the enrolled player list changes later.
- Match IDs from the pool are deterministic for a given player set. If players change, the pool changes and old IDs become invalid — the user would need to re-plan.
- Handlers live in `Winterplein.Application/PlannedMatches/`
- `DateOnly` route binding — verify ASP.NET Core 10 handles `/matchdays/{date}` natively or add a `TypeConverter`

## Tasks

- [ ] T1: Create `PlannedMatch` entity with properties and constructor validation
- [ ] T2: Create `PlannedMatchDto` and `PlanMatchRequest` records in `Winterplein.Shared/DTOs/`
- [ ] T3: Define `IPlannedMatchRepository` interface (blockedBy: T1)
- [ ] T4: Implement `InMemoryPlannedMatchRepository` using `ConcurrentDictionary` (blockedBy: T3)
- [ ] T5: Register `InMemoryPlannedMatchRepository` as Singleton in DI (blockedBy: T4)
- [ ] T6: Create `PlannedMatchMapper.ToDto()` using existing `MatchMapper` (blockedBy: T1, T2)
- [ ] T7: Create `PlanMatchCommand` + handler (blockedBy: T3, T6, Story 1 T1)
- [ ] T8: Create `GetPlannedMatchQuery` + handler (blockedBy: T3, T6)
- [ ] T9: Implement POST and GET API actions on `SeasonsController` (blockedBy: T5, T7, T8)
- [ ] T10: Add `PlanMatchAsync` and `GetPlannedMatchAsync` to `SeasonApiClient` (blockedBy: T9)
- [ ] T11: Build matchday detail page with match selector and planned match display (blockedBy: T10, Story 1 T3)
- [ ] T12: Create `PlannedMatchBuilder` in test commons (blockedBy: T1)
- [ ] T13: Write unit tests for `PlannedMatch` domain and CQRS handlers (blockedBy: T7, T8, T12)
- [ ] T14: Write integration tests for POST and GET endpoints (blockedBy: T9)
