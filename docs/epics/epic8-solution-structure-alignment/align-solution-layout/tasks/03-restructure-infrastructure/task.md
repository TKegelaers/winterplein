# 03-restructure-infrastructure

## Scope

Align `Winterplein.Infrastructure` internal folders with the reference layout
while keeping EF Core migrations intact and discoverable.

- Organise into reference-aligned folders: DbContext at a clear location,
  `Configurations/` for the EF entity configurations (already exists under
  `Persistence/Configurations/`), and a repositories folder for the EF and
  in-memory repositories.
- Keep `WinterpleinDbContext` in the same assembly and keep
  `ApplyConfigurationsFromAssembly(typeof(WinterpleinDbContext).Assembly)` working.
- **Keep `Migrations/` (the `InitialCreate` migration + `WinterpleinDbContextModelSnapshot`)
  in this project unchanged** — do not regenerate or move them out of the assembly.
- Update the `Ports` namespace using in repositories (the rename from task 02).
- Update any `using Winterplein.Infrastructure.Persistence` references in the host
  and tests to the new namespace if the folder name changes.

## Domain model changes (optional)

No domain model changes. EF mapping and migration history are unchanged. Folder
reorganisation only — verify the migration snapshot still matches the model
(no `dotnet ef migrations add` should produce a diff).

## Test cases

No new tests. Guard:

- `dotnet build` green, no new warnings.
- `dotnet test` green — `EfPlayerRepositoryTests` / `EfSeasonRepositoryTests` and
  the integration suite confirm the DbContext, configurations, and migrations
  still resolve and apply.

## Affected files

- move/rename: `src/Winterplein.Infrastructure/Persistence/*` into reference-aligned
  folders (DbContext, `Configurations/`, repositories) with namespace updates
- keep: `src/Winterplein.Infrastructure/Migrations/*` (unchanged, same assembly)
- modify: `src/Winterplein.Api/Program.cs` — `using` for DbContext/repository namespaces
- modify: `tests/Winterplein.IntegrationTests/WinterpleinApiFactory.cs` and repo tests
  — `using Winterplein.Infrastructure.*` namespace updates
