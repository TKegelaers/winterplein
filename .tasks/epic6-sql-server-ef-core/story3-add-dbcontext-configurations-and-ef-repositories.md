# Story 3 — Add DbContext, Configurations, and EF Repositories

**Epic:** Epic 6 — SQL Server Persistence with EF Core
**Dependencies:** Story 1 (entities must be EF Core compatible), Story 2 (async repository interfaces must exist)

## Description

Create `WinterpleinDbContext` with entity type configurations for all domain entities, implement `EfPlayerRepository` and `EfSeasonRepository` using the async interfaces from Story 2, add a connection string to `appsettings.json`, wire up DI, and run the initial EF Core migration against LocalDB. After this story, the API persists data to SQL Server and survives restarts.

## Acceptance Criteria

### NuGet Packages

- `Winterplein.Infrastructure.csproj`: add `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools`
- `Winterplein.Api.csproj`: add `Microsoft.EntityFrameworkCore.Design` (required by the `dotnet ef` CLI)

### DbContext (`Winterplein.Infrastructure/Persistence/`)

**`WinterpleinDbContext`**

- Inherits `DbContext`
- `DbSet<Player> Players`
- `DbSet<Season> Seasons`
- `DbSet<Team> Teams`
- `DbSet<Match> Matches`
- `OnModelCreating` calls `modelBuilder.ApplyConfigurationsFromAssembly(typeof(WinterpleinDbContext).Assembly)`

### Entity Configurations (`Winterplein.Infrastructure/Persistence/Configurations/`)

**`PlayerConfiguration`** (`IEntityTypeConfiguration<Player>`)

- Table `"Players"`, `Id` as primary key with identity
- `Name` as owned type (`OwnsOne`) — `FirstName` and `LastName` map to columns on the `Players` table
- `Gender` stored as string (`HasConversion<string>()`)

**`SeasonConfiguration`** (`IEntityTypeConfiguration<Season>`)

- Table `"Seasons"`, `Id` as primary key with identity
- `Name` as `nvarchar(200)`, required
- `StartDate`, `EndDate` as `date` columns; `Weekday` stored as string with conversion; `StartHour`, `EndHour` as `time` columns
- Many-to-many with `Player` via `_players` backing field: `HasMany(s => s.Players).WithMany()`, `UsePropertyAccessMode(PropertyAccessMode.Field)`, join table named `"SeasonPlayers"`

**`TeamConfiguration`** (`IEntityTypeConfiguration<Team>`)

- Table `"Teams"`, `Id` as primary key with identity
- `Player1Id` and `Player2Id` as foreign keys to the `Players` table
- `OnDelete(DeleteBehavior.Restrict)` for both (avoids SQL Server multiple cascade paths error)

**`MatchConfiguration`** (`IEntityTypeConfiguration<Match>`)

- Table `"Matches"`, `Id` as primary key with identity
- `Team1Id` and `Team2Id` as foreign keys to the `Teams` table
- `OnDelete(DeleteBehavior.Restrict)` for both

### EF Repositories (`Winterplein.Infrastructure/Persistence/`)

**`EfPlayerRepository`** — implements async `IPlayerRepository`

- `GetAllAsync`: `await _db.Players.ToListAsync(ct)`
- `GetByIdAsync`: `await _db.Players.FindAsync([id], ct)`
- `CountAsync`: `await _db.Players.CountAsync(ct)`
- `AddAsync`: `_db.Players.Add(player); await _db.SaveChangesAsync(ct); return player;` — EF assigns `Id` in-place
- `RemoveAsync`: find player, throw `KeyNotFoundException` if not found, `_db.Players.Remove(player)`, `SaveChangesAsync`

**`EfSeasonRepository`** — implements async `ISeasonRepository`

- `GetAllAsync`: `await _db.Seasons.Include(s => s.Players).ToListAsync(ct)`
- `GetByIdAsync`: `await _db.Seasons.Include(s => s.Players).FirstOrDefaultAsync(s => s.Id == id, ct)`
- `AddAsync`: `_db.Seasons.Add(season); await _db.SaveChangesAsync(ct); return season;`
- `UpdateAsync`: load existing with `Include(s => s.Players)`; return `false` if not found; copy scalar properties via `_db.Entry(existing).CurrentValues.SetValues(season)`; sync `Players` collection on the tracked entity; `SaveChangesAsync`; return `true`
- `DeleteAsync`: find season (no `Include`); return `false` if not found; `_db.Seasons.Remove(season)`; `SaveChangesAsync`; return `true`

### DI & Configuration (`Winterplein.Api/`)

**`appsettings.json`**

```json
"ConnectionStrings": {
  "WinterpleinDb": "Server=(localdb)\\mssqllocaldb;Database=Winterplein;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

**`Program.cs`**

- Add `builder.Services.AddDbContext<WinterpleinDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("WinterpleinDb")));`
- Change `IPlayerRepository`: `AddSingleton<InMemoryPlayerRepository>` → `AddScoped<IPlayerRepository, EfPlayerRepository>`
- Change `ISeasonRepository`: `AddSingleton<InMemorySeasonRepository>` → `AddScoped<ISeasonRepository, EfSeasonRepository>`
- `IMatchGeneratorService` remains `Singleton`

### Migration & Smoke Test

- `dotnet ef migrations add InitialCreate --project src/Winterplein.Infrastructure --startup-project src/Winterplein.Api` succeeds
- Generated migration creates tables: `Players`, `Seasons`, `Teams`, `Matches`, `SeasonPlayers`
- `dotnet ef database update` applies migration to LocalDB without errors
- Manual smoke test: `POST /api/players` creates a player; restart API; `GET /api/players` returns the persisted player

## Technical Notes

- `Season._players` is a private backing field — configure `.UsePropertyAccessMode(PropertyAccessMode.Field)` in `SeasonConfiguration` so EF Core populates it directly on materialization; the `IReadOnlyList<Player> Players` property is read-only and EF never sets it
- Season–Player many-to-many uses `HasMany(s => s.Players).WithMany()` with no inverse navigation on `Player`; the join table name must be set explicitly to `"SeasonPlayers"` to keep naming consistent with the project's conventions
- `TeamConfiguration` configures two FKs to the same `Players` table — both must use `DeleteBehavior.Restrict` to avoid the SQL Server "multiple cascade paths" error
- `EfSeasonRepository.UpdateAsync` receives a detached `Season` object constructed by the handler; use `_db.Entry(existing).CurrentValues.SetValues(season)` to copy scalar properties onto the tracked entity, then manually reconcile the `Players` navigation collection (remove players no longer present, add new ones) before calling `SaveChangesAsync`
- EF Core assigns the DB-generated `Id` back to the entity object in-place after `SaveChangesAsync` — callers receive the same object reference with `Id` populated
- The in-memory repositories remain in the codebase; they are still used by the integration test infrastructure until Story 4 replaces the test wiring

## Tasks

- [ ] T1: Add `Microsoft.EntityFrameworkCore.SqlServer` and `Microsoft.EntityFrameworkCore.Tools` to `Winterplein.Infrastructure.csproj`; add `Microsoft.EntityFrameworkCore.Design` to `Winterplein.Api.csproj`
- [ ] T2: Create `WinterpleinDbContext` with `DbSet` properties and `ApplyConfigurationsFromAssembly` (blockedBy: T1)
- [ ] T3: Create `PlayerConfiguration` — `Name` as owned type, `Gender` as string conversion (blockedBy: T2, Story 1 T1)
- [ ] T4: Create `SeasonConfiguration` — date/time column types, backing-field access, `SeasonPlayers` join table (blockedBy: T2, Story 1 T2)
- [ ] T5: Create `TeamConfiguration` — `Player1Id`/`Player2Id` FKs, `OnDelete(Restrict)` (blockedBy: T2, Story 1 T4)
- [ ] T6: Create `MatchConfiguration` — `Team1Id`/`Team2Id` FKs, `OnDelete(Restrict)` (blockedBy: T2, Story 1 T3)
- [ ] T7: Implement `EfPlayerRepository` (blockedBy: T3, Story 2 T1)
- [ ] T8: Implement `EfSeasonRepository` (blockedBy: T4, Story 2 T2)
- [ ] T9: Add `ConnectionStrings:WinterpleinDb` to `appsettings.json`
- [ ] T10: Update `Program.cs` — register `WinterpleinDbContext` with `UseSqlServer`, swap repo DI to `Scoped` EF repos (blockedBy: T2, T7, T8, T9)
- [ ] T11: Create initial EF Core migration `InitialCreate` (blockedBy: T3, T4, T5, T6, T10)
- [ ] T12: Apply migration to LocalDB; smoke-test API manually — create player, restart, verify persistence (blockedBy: T11)
