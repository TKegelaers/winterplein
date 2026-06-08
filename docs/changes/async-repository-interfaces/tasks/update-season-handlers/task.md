# update-season-handlers

## Scope

Update all 8 season handlers to `async Task<T>` and `await` repository calls. Add `CancellationToken ct` as an additional method parameter.

Handlers to update:

- `CreateSeasonCommandHandler.Handle` — await `AddAsync`
- `GetSeasonsQueryHandler.Handle` — await `GetAllAsync`
- `GetSeasonByIdQueryHandler.Handle` — await `GetByIdAsync`
- `UpdateSeasonCommandHandler.Handle` — await `GetByIdAsync` and `UpdateAsync`
- `DeleteSeasonCommandHandler.Handle` — await `DeleteAsync`
- `AddSeasonPlayerCommandHandler.Handle` — await `GetByIdAsync` (season + player) and `UpdateAsync`
- `RemoveSeasonPlayerCommandHandler.Handle` — await `GetByIdAsync` and `UpdateAsync`
- `GetSeasonPlayersQueryHandler.Handle` — await `GetByIdAsync`

## Domain model changes

None.

## Test cases

No new tests. Existing tests are updated in T6.

## Affected files

- modify: `src/Winterplein.Application/Seasons/CreateSeasonCommandHandler.cs`
- modify: `src/Winterplein.Application/Seasons/GetSeasonsQueryHandler.cs`
- modify: `src/Winterplein.Application/Seasons/GetSeasonByIdQueryHandler.cs`
- modify: `src/Winterplein.Application/Seasons/UpdateSeasonCommandHandler.cs`
- modify: `src/Winterplein.Application/Seasons/DeleteSeasonCommandHandler.cs`
- modify: `src/Winterplein.Application/Seasons/AddSeasonPlayerCommandHandler.cs`
- modify: `src/Winterplein.Application/Seasons/RemoveSeasonPlayerCommandHandler.cs`
- modify: `src/Winterplein.Application/Seasons/GetSeasonPlayersQueryHandler.cs`
