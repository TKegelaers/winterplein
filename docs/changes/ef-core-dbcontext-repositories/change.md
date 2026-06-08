# ef-core-dbcontext-repositories

## Problem Statement

The application currently stores all data in-memory via `InMemoryPlayerRepository` and `InMemorySeasonRepository`, which means all data is lost on every API restart. There is no SQL Server persistence layer.

## Proposed Solution

Create `WinterpleinDbContext` with entity type configurations for all domain entities, implement `EfPlayerRepository` and `EfSeasonRepository` using the async interfaces from the previous story, add a LocalDB connection string to `appsettings.json`, wire up DI in `Program.cs`, and run the initial EF Core migration. After this story the API persists data to SQL Server and survives restarts.

## Business Requirements

**Given** a player is created via `POST /api/players`
**When** the API is restarted
**Then** `GET /api/players` returns the previously created player

**Given** a season is created via `POST /api/seasons`
**When** the API is restarted
**Then** `GET /api/seasons` returns the previously created season with its enrolled players

## Acceptance Criteria

- [ ] `Microsoft.EntityFrameworkCore.SqlServer` and `Microsoft.EntityFrameworkCore.Tools` added to `Winterplein.Infrastructure.csproj`
- [ ] `Microsoft.EntityFrameworkCore.Design` added to `Winterplein.Api.csproj`
- [ ] `WinterpleinDbContext` created with `DbSet<Player>`, `DbSet<Season>`, `DbSet<Team>`, `DbSet<Match>`; applies configurations via `ApplyConfigurationsFromAssembly`
- [ ] `PlayerConfiguration`: `Name` as owned type, `Gender` as string conversion
- [ ] `SeasonConfiguration`: date/time column types, backing-field access for `_players`, `SeasonPlayers` join table
- [ ] `TeamConfiguration` and `MatchConfiguration`: FK relationships with `OnDelete(Restrict)`
- [ ] `EfPlayerRepository` and `EfSeasonRepository` implement the async repository interfaces
- [ ] `appsettings.json` contains `ConnectionStrings:WinterpleinDb` pointing to LocalDB
- [ ] `Program.cs` registers `WinterpleinDbContext` with `UseSqlServer` and swaps repo DI to scoped EF repos
- [ ] `dotnet ef migrations add InitialCreate` succeeds; generates tables `Players`, `Seasons`, `Teams`, `Matches`, `SeasonPlayers`
- [ ] `dotnet ef database update` applies migration without errors
- [ ] Smoke test: create player → restart API → `GET /api/players` returns persisted player

## Potential Pitfalls

- `Season._players` is a private backing field — configure `.UsePropertyAccessMode(PropertyAccessMode.Field)` in `SeasonConfiguration` so EF Core populates it directly on materialization
- `TeamConfiguration` configures two FKs to the same `Players` table — both must use `DeleteBehavior.Restrict` to avoid SQL Server "multiple cascade paths" error
- `EfSeasonRepository.UpdateAsync` receives a detached `Season` object — use `_db.Entry(existing).CurrentValues.SetValues(season)` to copy scalar properties, then manually reconcile the `Players` collection before `SaveChangesAsync`
- EF Core assigns the DB-generated `Id` back to the entity object in-place after `SaveChangesAsync`
- The in-memory repositories remain in the codebase until the next story replaces the test wiring
