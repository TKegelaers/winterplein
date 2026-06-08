# ef-core-test-infrastructure

## Problem Statement

After EF Core repositories replace the in-memory repositories in the API, the integration test factory (`WinterpleinApiFactory`) still registers the old in-memory repository swaps, which are no longer compatible with the EF-based DI registration. Integration tests will fail or bypass EF Core entirely.

## Proposed Solution

Replace the integration test factory's in-memory repository swaps with a SQLite in-memory `WinterpleinDbContext`. This makes integration tests exercise the full EF Core pipeline end-to-end while remaining fast (no real SQL Server required). Each test factory instance gets an isolated, empty in-memory database.

## Business Requirements

**Given** an integration test makes an HTTP request to the API
**When** the request touches a repository
**Then** the request flows through EF Core and SQLite rather than in-memory collections

## Acceptance Criteria

- [ ] `Microsoft.EntityFrameworkCore.Sqlite` NuGet package added to `Winterplein.IntegrationTests.csproj`
- [ ] `Winterplein.Infrastructure` project reference added to `Winterplein.IntegrationTests.csproj`
- [ ] `WinterpleinApiFactory` refactored: opens a persistent `SqliteConnection("DataSource=:memory:")`, removes the existing `WinterpleinDbContext` descriptor, registers a new `WinterpleinDbContext` with `UseSqlite(connection)`, calls `context.Database.EnsureCreated()` after build
- [ ] `SqliteConnection` lifetime managed in factory's `Dispose`/`DisposeAsync`
- [ ] All existing integration tests (`PlayersControllerTests`, `MatchesControllerTests`, season tests) pass without modification
- [ ] `dotnet test` — all unit and integration tests pass

## Potential Pitfalls

- SQLite in-memory requires a **persistent open connection** — `new SqliteConnection("DataSource=:memory:")` + `.Open()` before passing to `UseSqlite(connection)`; closing the connection destroys the database
- Use `EnsureCreated()` (not `MigrateAsync()`) in the test factory — creates schema from the EF model without running migrations
- SQLite is preferred over EF Core's `InMemory` provider because it enforces relational constraints that `InMemory` silently ignores
- `DateOnly`, `TimeOnly`, and `DayOfWeek`-as-string are supported by the EF Core SQLite provider in .NET 8+; SQL Server–specific hints like `nvarchar(200)` are silently ignored by SQLite
