# Story 4 — Update Test Infrastructure for EF Core

**Epic:** Epic 6 — SQL Server Persistence with EF Core
**Dependencies:** Story 2 (async repository interfaces), Story 3 (WinterpleinDbContext must exist)

## Description

Replace the integration test factory's in-memory repository DI swaps with a SQLite in-memory `WinterpleinDbContext` so integration tests exercise the full EF Core pipeline end-to-end. Unit tests already use async Moq setups from Story 2 and require no further changes. After this story the entire test suite — unit and integration — runs green against the new persistence layer.

## Acceptance Criteria

### Test Project (`tests/Winterplein.IntegrationTests/`)

**`Winterplein.IntegrationTests.csproj`**

- Add `Microsoft.EntityFrameworkCore.Sqlite` NuGet package
- Add `<ProjectReference>` to `Winterplein.Infrastructure` (for `WinterpleinDbContext`)

**`WinterpleinApiFactory.cs`**

- Remove the existing approach of replacing `IPlayerRepository` and `ISeasonRepository` singleton registrations
- In `ConfigureWebHost`:
  - Create a `SqliteConnection("DataSource=:memory:")` and open it immediately
  - Remove the existing `WinterpleinDbContext` service descriptor from `services`
  - Register a new `WinterpleinDbContext` using `UseSqlite(connection)`
  - After building the host, resolve `WinterpleinDbContext` from a scope and call `context.Database.EnsureCreated()`
- Store the open `SqliteConnection` as a field; dispose it in `Dispose` / `DisposeAsync`
- `WinterpleinApiFactory` implements `IAsyncDisposable` (or overrides `Dispose`) to close the connection after all tests complete

### Integration Tests

- All existing tests in `PlayersControllerTests`, `MatchesControllerTests`, and Season integration tests pass without modification — they test HTTP behaviour, not repository internals
- Each test class that instantiates a fresh `WinterpleinApiFactory` receives an isolated, empty in-memory database
- Any failures caused by SQLite type mapping differences are fixed (e.g. provider-specific column type hints are acceptable to leave as-is — SQLite ignores them)

### Unit Tests

- No changes required — async mock setups were completed in Story 2

### Full Test Run

- `dotnet test` — all unit and integration tests pass

## Technical Notes

- SQLite in-memory requires a **persistent open connection** — create `new SqliteConnection("DataSource=:memory:")`, call `.Open()`, and pass the open connection to `UseSqlite(connection)`. If the connection closes, the database is destroyed
- SQLite is preferred over EF Core's `InMemory` provider because SQLite enforces relational constraints (foreign keys, unique constraints) that `InMemory` silently ignores — this makes tests more representative of production behaviour
- `EnsureCreated()` creates the schema from the EF model without running migrations — correct for testing. Do **not** call `MigrateAsync()` in the factory
- `DateOnly`, `TimeOnly`, and `DayOfWeek`-as-string are supported by the EF Core SQLite provider in .NET 8+. SQL Server–specific hints like `nvarchar(200)` are silently ignored by SQLite — this is acceptable
- The factory's `SqliteConnection` must outlive all requests made by the test HTTP client; dispose it only in the factory's own dispose method, not after `EnsureCreated()`

## Tasks

- [ ] T1: Add `Microsoft.EntityFrameworkCore.Sqlite` package and `Winterplein.Infrastructure` project reference to `Winterplein.IntegrationTests.csproj`
- [ ] T2: Refactor `WinterpleinApiFactory` — open `SqliteConnection`, replace DbContext registration with `UseSqlite(connection)`, call `EnsureCreated()`, manage connection lifetime in Dispose (blockedBy: T1, Story 3 T2)
- [ ] T3: Run all integration tests — fix any failures from SQLite behavioural differences (blockedBy: T2)
- [ ] T4: Run full `dotnet test` — confirm all unit and integration tests green (blockedBy: T3)
