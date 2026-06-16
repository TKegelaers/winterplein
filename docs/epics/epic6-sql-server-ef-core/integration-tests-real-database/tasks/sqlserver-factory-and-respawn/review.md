# Review: sqlserver-factory-and-respawn (T1)

Outcome: Needs minor rework (no Critical issues). All T1 acceptance criteria are met; findings below are Warnings/Suggestions.

## Completeness

All required changes are present and verified:

- `Microsoft.EntityFrameworkCore.Sqlite` removed; `Microsoft.EntityFrameworkCore.SqlServer` 9.0.6, `Respawn` 7.0.0, `Microsoft.Extensions.Configuration.Json` added. No residual Sqlite/`UseSqlite`/`EnsureCreated` references anywhere in the project.
- `appsettings.json` created with `ConnectionStrings:WinterpleinDb` → `Winterplein_integrationTests`, and copied to output (`CopyToOutputDirectory=PreserveNewest`).
- `WinterpleinApiFactory` reads the connection string from config, removes the production DbContext/options descriptors, registers `UseSqlServer`, and calls `db.Database.Migrate()` (not `EnsureCreated()`) in `CreateHost`. `ConnectionString` and `CreateDbContext()` are exposed.
- `IntegrationTestBase` is abstract, implements `IAsyncLifetime`, runs Respawn `ResetAsync` in `InitializeAsync` ignoring `__EFMigrationsHistory`, and disposes the factory in `DisposeAsync`.
- `AssemblyInfo.cs` adds `[assembly: CollectionBehavior(DisableTestParallelization = true)]`.
- Build succeeds (only pre-existing NU1608 warnings unrelated to this task).

The 10 failing existing tests are due to those classes not yet inheriting `IntegrationTestBase` — that is T3's scope and explicitly out of scope here.

## Findings

### Warning — `CreateDbContext()` leaks the `IServiceScope`

`WinterpleinApiFactory.CreateDbContext()` creates a scope via `Services.CreateScope()` but returns only the resolved `WinterpleinDbContext`, discarding the `IServiceScope`. The XML doc claims "the underlying scope is disposed when the context is disposed," which is incorrect: in MS DI, disposing a scoped service does not dispose its owning scope. Each call leaks a scope (and any other scoped disposables) until the root provider is torn down.

This is the accessor T2's seed builders and the Respawn path are expected to use repeatedly, so the leak will accumulate per test class. Recommend either returning a small disposable wrapper that owns both the scope and the context, or exposing the scope to the caller, and correcting the XML doc to match actual disposal semantics.

### Suggestion — Use `DisposeAsync` for the factory

`IntegrationTestBase.DisposeAsync` calls the synchronous `Factory.Dispose()` and returns `Task.CompletedTask`. `WebApplicationFactory<T>` implements `IAsyncDisposable`; calling `await Factory.DisposeAsync()` is the cleaner async-correct teardown. Non-blocking.

### Suggestion — Respawner created per test

`IntegrationTestBase.InitializeAsync` calls `Respawner.CreateAsync` on every test (it inspects the schema each time). Functionally correct, but the checkpoint could be created once and reused for a minor speedup. Non-blocking.
