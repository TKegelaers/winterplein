# wire-up-di-and-connection-string

## Scope

Update `Program.cs` and `appsettings.json` to use the EF Core implementations:

- Add `ConnectionStrings:WinterpleinDb` to `appsettings.json` pointing to LocalDB (`Data Source=.;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name="Winterplein API";Command Timeout=0`).
- Register `WinterpleinDbContext` with `AddDbContext<WinterpleinDbContext>(opts => opts.UseSqlServer(...))`.
- Replace `AddSingleton<IPlayerRepository, InMemoryPlayerRepository>` with `AddScoped<IPlayerRepository, EfPlayerRepository>`.
- Replace `AddSingleton<ISeasonRepository, InMemorySeasonRepository>` with `AddScoped<ISeasonRepository, EfSeasonRepository>`.
- The in-memory repository classes remain in the codebase (they are still used by integration tests).

## Domain model changes

None.

## Test cases

None — verified by the smoke test in the migration task.

## Affected files

- modify: `src/Winterplein.Api/Program.cs`
- modify: `src/Winterplein.Api/appsettings.json`
