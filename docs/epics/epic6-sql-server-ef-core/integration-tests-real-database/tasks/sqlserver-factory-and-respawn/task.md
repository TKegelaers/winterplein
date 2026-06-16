# sqlserver-factory-and-respawn

## Scope

Switch the integration test infrastructure from SQLite in-memory to a real SQL Server database, apply migrations on startup, and add a Respawn-based clear-before-each-test hook with parallelization disabled.

- Remove `Microsoft.EntityFrameworkCore.Sqlite` from the IntegrationTests project.
- Add `Microsoft.EntityFrameworkCore.SqlServer` and `Respawn` (and `Microsoft.Extensions.Configuration.Json` if needed) via the `dotnet` CLI.
- Add `tests/Winterplein.IntegrationTests/appsettings.json` with `ConnectionStrings:WinterpleinDb` pointing at SQL Server DB `Winterplein_integrationTests`; ensure it is copied to the output directory.
- Rewrite `WinterpleinApiFactory`:
  - Remove the SQLite connection and `UseSqlite`.
  - Read the connection string from the test `appsettings.json`.
  - In the DI override, remove existing `WinterpleinDbContext`/options descriptors and register `UseSqlServer(connectionString)`.
  - In `CreateHost`, call `db.Database.Migrate()` (not `EnsureCreated()`).
  - Expose the connection string and a way to obtain a `WinterpleinDbContext` scope (e.g. `CreateDbContext()` / scope accessor) for seeding and Respawn.
- Create `IntegrationTestBase` (abstract, `IAsyncLifetime`): owns the factory + `HttpClient`; `InitializeAsync` runs Respawn `ResetAsync` against the DB before each test (ignore `__EFMigrationsHistory`); `DisposeAsync` disposes the factory.
- Add assembly-level `[assembly: CollectionBehavior(DisableTestParallelization = true)]`.

## Domain model changes

None.

## Test cases

No new test classes here; validated by the existing suite once migrated (next task) and by:

- Build succeeds with SqlServer + Respawn packages, no Sqlite reference.
- Manual: `dotnet test` against a freshly-provisioned `Winterplein_integrationTests` (schema only) passes; running twice in a row is stable.

## Affected files

- modify: tests/Winterplein.IntegrationTests/Winterplein.IntegrationTests.csproj
- create: tests/Winterplein.IntegrationTests/appsettings.json
- modify: tests/Winterplein.IntegrationTests/WinterpleinApiFactory.cs
- create: tests/Winterplein.IntegrationTests/IntegrationTestBase.cs
- create: tests/Winterplein.IntegrationTests/AssemblyInfo.cs
