# implement-ef-repositories

## Scope

Implement `EfPlayerRepository` and `EfSeasonRepository` in `Winterplein.Infrastructure/Persistence/`, each taking `WinterpleinDbContext` via constructor injection. Both classes implement the existing synchronous repository interfaces (`IPlayerRepository`, `ISeasonRepository`).

Key implementation notes:

- `EfPlayerRepository.Add`: construct `Player` with `id = 0`, add to context, call `SaveChanges`, return entity (EF writes DB-generated id back in-place).
- `EfPlayerRepository.Remove`: load entity, remove, save.
- `EfSeasonRepository.Add`: same id-0 pattern; include `Players` navigation.
- `EfSeasonRepository.Update`: load existing season by id, copy scalar values via `Entry.CurrentValues.SetValues`, reconcile the `Players` collection (remove missing, add new), then `SaveChanges`.
- `EfSeasonRepository.GetById` and `GetAll`: eager-load `Players` via `Include`.

## Domain model changes

None.

## Test cases

- `EfPlayerRepositoryTests`
  - `Add_PersistsPlayer_AndAssignsId`
  - `GetById_ReturnsPlayer_WhenExists`
  - `GetById_ReturnsNull_WhenNotFound`
  - `Remove_DeletesPlayer`
  - `Count_ReflectsStoredPlayers`

- `EfSeasonRepositoryTests`
  - `Add_PersistsSeason_WithPlayers`
  - `GetById_ReturnsSeason_WithPlayers`
  - `Update_UpdatesScalarsAndPlayers`
  - `Delete_RemovesSeason`
  - `GetAll_ReturnsAllSeasons`

Use an in-memory SQLite or `UseInMemoryDatabase` provider for unit tests to avoid a real SQL Server dependency.

## Affected files

- create: `src/Winterplein.Infrastructure/Persistence/EfPlayerRepository.cs`
- create: `src/Winterplein.Infrastructure/Persistence/EfSeasonRepository.cs`
- create: `tests/Winterplein.UnitTests/Infrastructure/EfPlayerRepositoryTests.cs`
- create: `tests/Winterplein.UnitTests/Infrastructure/EfSeasonRepositoryTests.cs`
