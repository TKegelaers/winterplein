# Story 2 — Generate Season Schedule

**Epic:** Epic 3 — Season Match Planning
**Dependencies:** Story 1

## Description

Automatically assign one unique random match from the season's match pool to every open (unplanned) matchday at once. A single "Generate Schedule" action replaces all manual per-matchday match selection. The PlannedMatch entity stores a full match snapshot so it remains valid even if the player list changes later.

## Acceptance Criteria

### Domain (`Winterplein.Domain/Entities/PlannedMatch.cs`)

- `Id` (int), `SeasonId` (int), `Date` (DateOnly), `Match` (Match)
- Constructor accepts `int id`, `int seasonId`, `DateOnly date`, `Match match`
- Validation: `match` must not be null, `date` must not be default

### DTOs (`Winterplein.Shared/DTOs/`)

- **`PlannedMatchDto`** — `record PlannedMatchDto(int Id, int SeasonId, DateOnly Date, MatchDto Match)`
- **`GenerateScheduleResponse`** — `record GenerateScheduleResponse(List<PlannedMatchDto> PlannedMatches, int PlannedCount, int OpenCount)` — returns the newly created planned matches, how many were planned, and how many matchdays remain open (when the pool was exhausted)

### CQRS & Repository (`Winterplein.Application/PlannedMatches/`)

- **`IPlannedMatchRepository`** in `Interfaces/` — `GetAllBySeason(int seasonId)`, `Add(PlannedMatch)`, `Delete(int seasonId, DateOnly date)` → `bool`, `DeleteAllBySeason(int seasonId)`
- **`GenerateScheduleCommand(int SeasonId)`** → `GenerateScheduleResponse?`
  - Loads season via `ISeasonRepository` — returns null if not found
  - Gets all matchdays via `season.GetMatchdays()`
  - Generates the match pool via `IMatchGeneratorService.GenerateAllMatches(season.Players)`
  - Loads already-planned matches via `IPlannedMatchRepository.GetAllBySeason(seasonId)`
  - Determines open matchdays (matchdays without a planned match)
  - Filters pool to exclude matches already planned (comparing `Match.Id` from pool against `Match.Id` in existing planned match snapshots)
  - Shuffles available matches using `Random.Shared` (Fisher-Yates) and assigns one per open matchday until matchdays or matches are exhausted
  - Persists all new `PlannedMatch` entities via `repository.Add()`
  - Returns `GenerateScheduleResponse` with the new planned matches, count planned, and count of remaining open matchdays
- **`PlannedMatchMapper`** in `Mappers/` — `ToDto(PlannedMatch)` using existing `MatchMapper`

### Infrastructure (`Winterplein.Infrastructure/Persistence/`)

- **`InMemoryPlannedMatchRepository`** backed by `ConcurrentDictionary<int, PlannedMatch>` (keyed by auto-incrementing `PlannedMatch.Id`, same pattern as `InMemorySeasonRepository`)
  - `GetAllBySeason` filters by `SeasonId`
  - `Add` assigns a new ID and stores the entity
  - `Delete(seasonId, date)` removes the match for the given matchday, returns `true` if found and removed
  - `DeleteAllBySeason(seasonId)` removes all planned matches for a season
- Registered as Singleton in DI

### API Endpoints (on `SeasonsController`)

| Method | Route                                 | Request | Response                             |
| ------ | ------------------------------------- | ------- | ------------------------------------ |
| POST   | `/api/seasons/{id}/schedule/generate` | —       | `GenerateScheduleResponse` 200 / 404 |

- Returns 404 if season not found
- Returns 200 with `PlannedCount: 0, OpenCount: <matchday count>` if fewer than 4 enrolled players (empty pool)
- Returns 200 with `PlannedCount: 0, OpenCount: 0` if all matchdays are already planned
- Idempotent — calling again only fills remaining open matchdays

### Blazor UI (`Winterplein.Client/`)

- Add `GenerateScheduleAsync(int seasonId)` method to `SeasonApiClient` — `POST /api/seasons/{id}/schedule/generate`, returns `GenerateScheduleResponse?` (null on 404)
- **Season Detail Page** (`/seasons/{id}`) — add a "Generate Schedule" `MudButton` (Color.Primary, Variant.Filled)
  - On click: calls `GenerateScheduleAsync`, shows `Snackbar` with result:
    - `PlannedCount > 0`: "Scheduled {n} matches"
    - `PlannedCount: 0, OpenCount: 0`: "All matchdays are already scheduled"
    - `PlannedCount: 0, OpenCount > 0`: "Not enough matches to fill all matchdays"
  - After success: refreshes the schedule view (Story 3 dependency for display)
  - Disabled while loading to prevent double-clicks

### Tests

- **`PlannedMatchBuilder`** in `tests/Winterplein.UnitTests.Common/Builders/` — fluent builder with `.WithId()`, `.WithSeasonId()`, `.WithDate()`, `.WithMatch()`
- **Unit tests** for `PlannedMatch` domain: constructor succeeds with valid args, throws on null match, throws on default date
- **Unit tests** for `GenerateScheduleCommandHandler`:
  - Returns null when season not found
  - Returns `PlannedCount: 0` when fewer than 4 players (empty pool)
  - Plans one unique match per open matchday (happy path)
  - Skips already-planned matchdays (pre-seed 1 planned match, verify only open matchdays get filled)
  - Does not reuse matches already planned in this season (uniqueness constraint)
  - Handles more matchdays than available matches (fills as many as possible, correct `OpenCount`)
  - Returns `PlannedCount: 0, OpenCount: 0` when all matchdays already planned
- **Integration tests**: POST generate → 200 with planned matches; POST unknown season → 404; POST with too few players → 200 with `PlannedCount: 0`; POST twice → second call is a no-op

## Technical Notes

- The `PlannedMatch` stores a full `Match` snapshot (with `Team` and `Player` objects) — not just a match ID. This ensures the planned match remains readable even if the enrolled player list changes later.
- Uniqueness is tracked by comparing `Match.Id` values from the current pool against the `Match.Id` stored in existing `PlannedMatch` snapshots. If the player list changes between generations, pool IDs shift and there are no false collisions — the snapshot-stored matches remain valid for display.
- Randomization uses `Random.Shared` (thread-safe in .NET 6+). Unit tests verify structural invariants (correct count, uniqueness, all matches come from pool) rather than specific random outputs.
- Handlers live in `Winterplein.Application/PlannedMatches/`

## Tasks

- [ ] T1: Create `PlannedMatch` entity with properties and constructor validation
- [ ] T2: Create `PlannedMatchDto` and `GenerateScheduleResponse` records in `Winterplein.Shared/DTOs/`
- [ ] T3: Define `IPlannedMatchRepository` interface with `GetAllBySeason`, `Add`, `Delete`, `DeleteAllBySeason` (blockedBy: T1)
- [ ] T4: Implement `InMemoryPlannedMatchRepository` using `ConcurrentDictionary` (blockedBy: T3)
- [ ] T5: Register `InMemoryPlannedMatchRepository` as Singleton in DI (blockedBy: T4)
- [ ] T6: Create `PlannedMatchMapper.ToDto()` using existing `MatchMapper` (blockedBy: T1, T2)
- [ ] T7: Create `GenerateScheduleCommand` + handler with random unique assignment logic (blockedBy: T3, T6, Story 1 T1)
- [ ] T8: Implement POST `/api/seasons/{id:int}/schedule/generate` action on `SeasonsController` (blockedBy: T5, T7)
- [ ] T9: Add `GenerateScheduleAsync` to `SeasonApiClient` (blockedBy: T8)
- [ ] T10: Add "Generate Schedule" button to season detail page with snackbar feedback (blockedBy: T9)
- [ ] T11: Create `PlannedMatchBuilder` in test commons (blockedBy: T1)
- [ ] T12: Write unit tests for `PlannedMatch` domain and `GenerateScheduleCommandHandler` (blockedBy: T7, T11)
- [ ] T13: Write integration tests for POST generate endpoint (blockedBy: T8)
