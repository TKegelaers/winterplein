# Story 2 — Implement Season CQRS & Repository

**Epic:** Epic 2 — Season Management
**Dependencies:** Story 1

## Description

Implement the CQRS commands/queries with Wolverine native handlers and an in-memory repository for season persistence.

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

- [x] T1: Define `ISeasonRepository` interface with GetAll, GetById, Add, Update, Delete
- [x] T2: Create `CreateSeasonCommand` + handler — builds `Season`, calls `Add`, returns new `int` Id (blockedBy: T1, Story 1 T1)
- [x] T3: Create `GetSeasonsQuery` + handler — returns all seasons (blockedBy: T1)
- [x] T4: Create `GetSeasonByIdQuery` (Id: int) + handler — returns season or null (blockedBy: T1)
- [x] T5: Create `UpdateSeasonCommand` (Id: int) + handler — updates existing season, returns false if not found (blockedBy: T1)
- [x] T6: Create `DeleteSeasonCommand` (Id: int) + handler — removes season, returns false if not found (blockedBy: T1)
- [x] T7: Implement `InMemorySeasonRepository` using `ConcurrentDictionary<int, Season>` (blockedBy: T1)
- [x] T8: Register `InMemorySeasonRepository` as Singleton for `ISeasonRepository` in DI (blockedBy: T7)
- [x] T9: Add `Player? GetById(int id)` to `IPlayerRepository` and implement in `InMemoryPlayerRepository`
- [x] T10: Create `AddSeasonPlayerCommand(SeasonId, PlayerId)` + handler — loads season + player via repositories, calls `season.AddPlayer(player)`, calls `Update`, returns `SeasonDto?` (null if season or player not found) (blockedBy: T1, T9, Story 1 T5)
- [x] T11: Create `RemoveSeasonPlayerCommand(SeasonId, PlayerId)` + handler — loads season, calls `season.RemovePlayer(playerId)`, calls `Update`, returns `bool` false if season not found (blockedBy: T1, Story 1 T5)
- [x] T12: Create `GetSeasonPlayersQuery(SeasonId)` + handler — returns `List<PlayerDto>?` (null if season not found) (blockedBy: T1, Story 1 T5, T6)
