# create-dbcontext-and-configurations

## Scope

Create `WinterpleinDbContext` and one `IEntityTypeConfiguration<T>` class per entity. The context registers configurations via `ApplyConfigurationsFromAssembly`.

Configuration rules per entity:

- `PlayerConfiguration`: `Name` as owned type (maps `FirstName`/`LastName` columns), `Gender` stored as string via value conversion.
- `SeasonConfiguration`: `DateOnly`/`TimeOnly` columns mapped to `date`/`time` SQL types, `_players` collection accessed via `UsePropertyAccessMode(PropertyAccessMode.Field)`, `SeasonPlayers` join table for the many-to-many relationship with `Players`.
- `TeamConfiguration`: two FK references to `Players` (`Player1Id`, `Player2Id`), both with `DeleteBehavior.Restrict`.
- `MatchConfiguration`: FK references to `Teams` (`Team1Id`, `Team2Id`), both with `DeleteBehavior.Restrict`.

## Domain model changes

No changes to domain entities. All mapping concerns are isolated in the configuration classes.

## Test cases

None for this task — correctness is verified by the migration task (migration must generate the expected tables and columns).

## Affected files

- create: `src/Winterplein.Infrastructure/Persistence/WinterpleinDbContext.cs`
- create: `src/Winterplein.Infrastructure/Persistence/Configurations/PlayerConfiguration.cs`
- create: `src/Winterplein.Infrastructure/Persistence/Configurations/SeasonConfiguration.cs`
- create: `src/Winterplein.Infrastructure/Persistence/Configurations/TeamConfiguration.cs`
- create: `src/Winterplein.Infrastructure/Persistence/Configurations/MatchConfiguration.cs`
