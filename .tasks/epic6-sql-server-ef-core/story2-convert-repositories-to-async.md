# Story 2 — Convert Repositories to Async

**Epic:** Epic 6 — SQL Server Persistence with EF Core
**Dependencies:** Story 1 (domain entities must be EF Core compatible before handlers are updated)

## Description

Make `IPlayerRepository` and `ISeasonRepository` fully async with `Task<T>` return types and `CancellationToken` parameters. Update all 13 handlers to `await` repository calls instead of wrapping synchronous results in `Task.FromResult`. Update the in-memory implementations to satisfy the new interfaces. This is required before EF Core repositories can be introduced in Story 3, and keeps the application functional throughout the transition.

## Acceptance Criteria

### Repository Interfaces (`Winterplein.Application/Interfaces/`)

**`IPlayerRepository`**

- `Task<IReadOnlyList<Player>> GetAllAsync(CancellationToken ct = default)`
- `Task<Player?> GetByIdAsync(int id, CancellationToken ct = default)`
- `Task<int> CountAsync(CancellationToken ct = default)` — replaces the `int Count` property
- `Task<Player> AddAsync(Player player, CancellationToken ct = default)` — replaces `Player Add(Name name, Gender gender)`
- `Task RemoveAsync(int id, CancellationToken ct = default)`

**`ISeasonRepository`**

- `Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken ct = default)`
- `Task<Season?> GetByIdAsync(int id, CancellationToken ct = default)`
- `Task<Season> AddAsync(Season season, CancellationToken ct = default)`
- `Task<bool> UpdateAsync(Season season, CancellationToken ct = default)`
- `Task<bool> DeleteAsync(int id, CancellationToken ct = default)`

### Infrastructure (`Winterplein.Infrastructure/Persistence/`)

**`InMemoryPlayerRepository`**

- Implements new async `IPlayerRepository`; all methods wrap synchronous logic in `Task.FromResult`
- `AddAsync(Player player)`: assign the next auto-increment ID internally, return a new `Player` with that ID

**`InMemorySeasonRepository`**

- Implements new async `ISeasonRepository`; all methods wrap synchronous logic in `Task.FromResult`

### Handlers — Player & Match (`Winterplein.Application/`)

- `AddPlayerCommandHandler`: construct `new Player(0, new Name(cmd.FirstName, cmd.LastName), cmd.Gender.ToDomain())`, call `await repo.AddAsync(player, ct)`; return mapped DTO from the returned player
- `RemovePlayerCommandHandler`: `await repo.RemoveAsync(cmd.Id, ct)`
- `GetAllPlayersQueryHandler`: `await repo.GetAllAsync(ct)`
- `GetMatchCountQueryHandler`: `await repo.CountAsync(ct)`
- `GenerateMatchesCommandHandler`: `await repo.GetAllAsync(ct)`
- All 5 handlers return `Task<T>` and are `async`

### Handlers — Season (`Winterplein.Application/Seasons/`)

All 8 season handlers updated to `async Task<T>`, replacing synchronous repo calls with their `Async` equivalents and passing `CancellationToken`

### Unit Tests (`tests/Winterplein.UnitTests/`)

- All handler test mock setups changed from `.Returns(value)` to `.ReturnsAsync(value)`
- `AddPlayerCommandHandler` tests: mock `AddAsync` to accept any `Player` and return a `Player` with an assigned `Id`
- Tests for async-returning handlers use `await` correctly

### Build & Test

- `dotnet build` succeeds
- `dotnet test` — all unit and integration tests pass

## Technical Notes

- `IPlayerRepository.Add(Name, Gender)` → `AddAsync(Player, CancellationToken)`: the handler now constructs the `Player` with `Id = 0`; the in-memory repo assigns the next ID and returns a new `Player` instance. EF Core (Story 3) will assign the DB-generated ID on `SaveChangesAsync`
- `int Count` property → `CountAsync()` method: C# properties cannot be `async` — this is a breaking interface change but is fully contained within `Application`, `Infrastructure`, and `tests`
- After Epic 5, handlers are static `Handle` methods — `CancellationToken` is added as an additional method parameter (Wolverine injects it automatically from the message context)
- Integration tests call HTTP endpoints and do not reference repository interfaces directly — no changes needed in this story

## Tasks

- [ ] T1: Redefine `IPlayerRepository` — all-async methods, new `AddAsync(Player, CancellationToken)` signature, remove `Count` property
- [ ] T2: Redefine `ISeasonRepository` — all-async methods with `CancellationToken`
- [ ] T3: Update `InMemoryPlayerRepository` to implement async interface; `AddAsync` assigns next ID and returns new `Player` (blockedBy: T1)
- [ ] T4: Update `InMemorySeasonRepository` to implement async interface (blockedBy: T2)
- [ ] T5: Update `AddPlayerCommandHandler` — construct `Player(0, ...)`, call `await repo.AddAsync(player, ct)` (blockedBy: T1)
- [ ] T6: Update `RemovePlayerCommandHandler` — call `await repo.RemoveAsync(cmd.Id, ct)` (blockedBy: T1)
- [ ] T7: Update `GetAllPlayersQueryHandler` — call `await repo.GetAllAsync(ct)` (blockedBy: T1)
- [ ] T8: Update `GetMatchCountQueryHandler` — call `await repo.CountAsync(ct)` (blockedBy: T1)
- [ ] T9: Update `GenerateMatchesCommandHandler` — call `await repo.GetAllAsync(ct)` (blockedBy: T1)
- [ ] T10: Update all 8 Season handlers to use async repo methods with `CancellationToken` (blockedBy: T2)
- [ ] T11: Update Player/Match handler unit tests — `.ReturnsAsync()`, new `AddAsync` mock signature (blockedBy: T5, T6, T7, T8, T9)
- [ ] T12: Update Season handler unit tests — `.ReturnsAsync()` (blockedBy: T10)
- [ ] T13: `dotnet build` + `dotnet test` — all green (blockedBy: T3, T4, T11, T12)
