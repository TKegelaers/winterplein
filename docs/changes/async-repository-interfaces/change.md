# async-repository-interfaces

## Problem Statement

`IPlayerRepository` and `ISeasonRepository` use synchronous method signatures with no `CancellationToken` support. EF Core repositories require async methods (`Task<T>` returns) and cancellation token parameters. The current synchronous interfaces block introducing EF Core persistence without breaking all 13 handlers that use them.

## Proposed Solution

Redefine both repository interfaces to be fully async with `Task<T>` return types and `CancellationToken` parameters. Update all 13 handlers to `await` the repository calls. Update the in-memory implementations to satisfy the new interfaces by wrapping synchronous logic in `Task.FromResult`.

## Business Requirements

**Given** the repository interfaces use synchronous signatures
**When** EF Core repositories are introduced in the next story
**Then** the EF repositories can implement the same async interfaces without handler changes

## Acceptance Criteria

- [ ] `IPlayerRepository`: `GetAllAsync`, `GetByIdAsync`, `CountAsync`, `AddAsync(Player, CancellationToken)`, `RemoveAsync` — all return `Task<T>` with optional `CancellationToken`
- [ ] `ISeasonRepository`: `GetAllAsync`, `GetByIdAsync`, `AddAsync`, `UpdateAsync`, `DeleteAsync` — all return `Task<T>` with optional `CancellationToken`
- [ ] `InMemoryPlayerRepository` implements the new async interface; `AddAsync` assigns next auto-increment ID and returns a new `Player` with that ID
- [ ] `InMemorySeasonRepository` implements the new async interface
- [ ] All 5 player/match handlers (`AddPlayer`, `RemovePlayer`, `GetAllPlayers`, `GetMatchCount`, `GenerateMatches`) updated to `async Task<T>` with `await`
- [ ] All 8 season handlers updated to `async Task<T>` with `await`
- [ ] All handler unit tests updated: mock setups changed from `.Returns()` to `.ReturnsAsync()`
- [ ] `dotnet build` succeeds; `dotnet test` — all unit and integration tests pass

## Potential Pitfalls

- `IPlayerRepository.Add(Name, Gender)` → `AddAsync(Player, CancellationToken)`: handler now constructs `Player` with `Id = 0`; in-memory repo assigns next ID and returns a new instance with that ID assigned
- `int Count` property → `CountAsync()` method: C# properties cannot be `async` — this is a breaking interface change but is fully contained within `Application`, `Infrastructure`, and `tests`
- After Epic 5, handlers are static `Handle` methods — `CancellationToken` is added as an additional method parameter; Wolverine injects it automatically from the message context
- Integration tests call HTTP endpoints and do not reference repository interfaces directly — no changes needed in this story
