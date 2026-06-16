# integration-tests-real-database

## Problem Statement

Integration tests run against a SQLite in-memory database (`WinterpleinApiFactory`), which does not exercise the real SQL Server provider used in production. SQLite and SQL Server differ in type mapping, constraint behavior, and SQL translation, so tests can pass while production behavior diverges. We want integration tests to run against a real SQL Server database so they validate the actual provider, and to allow inspecting the database state after a run.

## Proposed Solution

Replace the SQLite in-memory setup with a real SQL Server database named `Winterplein_integrationTests`, provisioned locally by the developer. `WinterpleinApiFactory` registers the SQL Server provider using a connection string from a test `appsettings.json`, and applies EF Core migrations on startup so the schema matches production. Before each test, the database is cleared with Respawn so every test starts from a known empty state; data left by the last test that ran remains in the database for inspection. Tests that need data seed it directly through EF Core using fluent seed builders (one per entity), following the established builder pattern. Integration tests run serially since they share one physical database.

## Business Requirements

**Given** the integration test suite
**When** a test runs
**Then** it executes against the real SQL Server database `Winterplein_integrationTests`, not SQLite

**Given** a test that requires pre-existing data
**When** the test sets up
**Then** it seeds that data directly into the database via EF Core before exercising the API

**Given** any integration test
**When** it begins
**Then** all existing data is cleared first, so the test starts from an empty database

**Given** a completed test run
**When** the developer inspects `Winterplein_integrationTests`
**Then** the data written by the last-run test is still present (data is cleared before, not after, each test)

## Acceptance Criteria

- [ ] Integration tests connect to SQL Server `Winterplein_integrationTests` using a connection string from a test `appsettings.json`
- [ ] SQLite (`Microsoft.Data.Sqlite` / `UseSqlite`) is removed from the integration test project
- [ ] EF Core migrations are applied to the test database on factory startup
- [ ] Respawn clears all data before each test
- [ ] Data inserted by a test remains in the database after the run completes
- [ ] Reusable fluent seed builders exist for the entities tests need (e.g. player, season), each exposing `With*` methods and an `async Task<T> Seed(WinterpleinDbContext)` method
- [ ] Integration tests are configured to run serially (no parallelization against the shared database)
- [ ] All existing integration tests pass against the real database

## Testing Plan

- Provision `Winterplein_integrationTests` locally; run `dotnet test tests/Winterplein.IntegrationTests` and confirm all tests pass
- Confirm a fresh DB (only schema, no data) lets the suite pass from clean
- Run the suite twice in a row; confirm clear-before-test makes results stable
- After a run, inspect the database and confirm the last test's data is still present

## Refactors

- `WinterpleinApiFactory`: remove SQLite connection handling; register `UseSqlServer` from config; apply migrations on startup; expose a scope/`DbContext` accessor for seeding and Respawn clearing
- Keep the per-class factory model; add a shared clear-before-test hook (base class / `IAsyncLifetime`) so each test runs Respawn before its body
- Existing tests keep building state via API calls where they already do; only tests needing pre-existing state adopt seed builders

## Potential Pitfalls

- Respawn must delete tables in FK-dependency order (incl. the `SeasonPlayers` join table) or deletes fail
- Migrations must be current; a drifted model vs. migrations will surface as schema errors in tests
- Shared physical DB + parallel tests would clash — parallelization must be disabled and verified
- Connection string in `appsettings.json` is local-only; CI will need its own SQL Server instance/connection string before these tests can run there
- A previously failed/aborted run leaves data behind, but clear-before-test on the next run handles it
