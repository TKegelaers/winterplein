# run-initial-migration

## Scope

Generate and apply the initial EF Core migration:

- Run `dotnet ef migrations add InitialCreate --project src/Winterplein.Infrastructure --startup-project src/Winterplein.Api`.
- Verify the generated migration creates tables: `Players`, `Seasons`, `Teams`, `Matches`, `SeasonPlayers`.
- Run `dotnet ef database update --project src/Winterplein.Infrastructure --startup-project src/Winterplein.Api` to apply the migration against LocalDB.
- Smoke test: create a player via `POST /api/players`, restart the API, confirm `GET /api/players` returns the player.

## Domain model changes

None.

## Test cases

No automated tests — this task is verified by running the migration and smoke test manually.

## Affected files

- create: `src/Winterplein.Infrastructure/Migrations/<timestamp>_InitialCreate.cs`
- create: `src/Winterplein.Infrastructure/Migrations/<timestamp>_InitialCreate.Designer.cs`
- create: `src/Winterplein.Infrastructure/Migrations/WinterpleinDbContextModelSnapshot.cs`
