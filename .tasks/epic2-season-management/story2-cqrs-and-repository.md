# Story 2 — CQRS Handlers & In-Memory Repository

**Epic:** Epic 2 — Season Management
**Dependencies:** Story 1

## Description

Implement the CQRS commands/queries with MediatR handlers and an in-memory repository for season persistence.

## Acceptance Criteria

### Application layer (`Winterplein.Application/`)

- **`ISeasonRepository`** interface in `Interfaces/` — GetAll, GetById, Add, Update, Delete
- **Commands:**
  - `CreateSeasonCommand` (Name, StartDate, EndDate, Weekday, StartHour, EndHour) → `int`
  - `UpdateSeasonCommand` (Id: int + same fields as Create) → `bool`
  - `DeleteSeasonCommand` (Id: int) → `bool`
- **Queries:**
  - `GetSeasonsQuery` → `List<Season>`
  - `GetSeasonByIdQuery` (Id: int) → `Season?`

### Infrastructure layer (`Winterplein.Infrastructure/Persistence/`)

- **`InMemorySeasonRepository`** backed by `ConcurrentDictionary<int, Season>`
- Registered as Singleton in DI

## Technical Notes

- Commands and queries live in `Winterplein.Application/Seasons/`
- Handlers are in the same folder as their command/query (one file per pair)
- `ISeasonRepository` is the only interface handlers depend on — no direct infrastructure coupling

## Tasks

- [ ] T1: Define `ISeasonRepository` interface with GetAll, GetById, Add, Update, Delete
- [ ] T2: Create `CreateSeasonCommand` + handler — builds `Season`, calls `Add`, returns new `int` Id (blocks: T1, Story 1 T1)
- [ ] T3: Create `GetSeasonsQuery` + handler — returns all seasons (blocks: T1)
- [ ] T4: Create `GetSeasonByIdQuery` (Id: int) + handler — returns season or null (blocks: T1)
- [ ] T5: Create `UpdateSeasonCommand` (Id: int) + handler — updates existing season, returns false if not found (blocks: T1)
- [ ] T6: Create `DeleteSeasonCommand` (Id: int) + handler — removes season, returns false if not found (blocks: T1)
- [ ] T7: Implement `InMemorySeasonRepository` using `ConcurrentDictionary<int, Season>` (blocks: T1)
- [ ] T8: Register `InMemorySeasonRepository` as Singleton for `ISeasonRepository` in DI (blocks: T7)
- [ ] T9: Add `Player? GetById(int id)` to `IPlayerRepository` and implement in `InMemoryPlayerRepository`
- [ ] T10: Create `AddSeasonPlayerCommand(SeasonId, PlayerId)` + handler — loads season + player via repositories, calls `season.AddPlayer(player)`, calls `Update`, returns `SeasonDto?` (null if season or player not found) (blocks: T1, T9, Story 1 T5)
- [ ] T11: Create `RemoveSeasonPlayerCommand(SeasonId, PlayerId)` + handler — loads season, calls `season.RemovePlayer(playerId)`, calls `Update`, returns `bool` false if season not found (blocks: T1, Story 1 T5)
- [ ] T12: Create `GetSeasonPlayersQuery(SeasonId)` + handler — returns `List<PlayerDto>?` (null if season not found) (blocks: T1, Story 1 T5, T6)
