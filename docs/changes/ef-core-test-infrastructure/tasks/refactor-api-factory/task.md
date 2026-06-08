# refactor-api-factory

## Scope

Replace the in-memory repository swaps in `WinterpleinApiFactory` with a SQLite in-memory `WinterpleinDbContext`.

- Open a persistent `SqliteConnection("DataSource=:memory:")` at factory construction time
- In `ConfigureWebHost`: remove all existing `IPlayerRepository`, `ISeasonRepository`, and `WinterpleinDbContext` descriptors; register a new `WinterpleinDbContext` configured with `UseSqlite(connection)`
- After `WebApplication` builds: resolve `WinterpleinDbContext` and call `Database.EnsureCreated()`
- Implement `IAsyncDisposable` (or override `DisposeAsync`) on the factory to close and dispose the `SqliteConnection`

## Domain model changes (optional)

None.

## Test cases

No new test files — all existing tests must pass unchanged:

- `PlayersControllerTests` — all existing scenarios pass against SQLite
- `MatchesControllerTests` — all existing scenarios pass against SQLite
- `Seasons/SeasonsControllerTests` — all existing scenarios pass against SQLite

Run `dotnet test tests/Winterplein.IntegrationTests` to verify.

## Affected files

- modify: tests/Winterplein.IntegrationTests/WinterpleinApiFactory.cs
