# update-player-handlers

## Scope

Update all 5 player/match handlers to `async Task<T>` and `await` the repository calls. Add `CancellationToken ct` as an additional method parameter (Wolverine injects it automatically).

Handlers to update:

- `AddPlayerCommandHandler.Handle` — construct `Player` with `Id = 0` before calling `AddAsync`; await result
- `RemovePlayerCommandHandler.Handle` — await `RemoveAsync`
- `GetAllPlayersQueryHandler.Handle` — await `GetAllAsync`
- `GetMatchCountQueryHandler.Handle` — await `CountAsync`
- `GenerateMatchesCommandHandler.Handle` — await `GetAllAsync`

## Domain model changes

None.

## Test cases

No new tests. Existing tests are updated in T6.

## Affected files

- modify: `src/Winterplein.Application/Commands/AddPlayer/AddPlayerCommandHandler.cs`
- modify: `src/Winterplein.Application/Commands/RemovePlayer/RemovePlayerCommandHandler.cs`
- modify: `src/Winterplein.Application/Queries/GetAllPlayers/GetAllPlayersQueryHandler.cs`
- modify: `src/Winterplein.Application/Queries/GetMatchCount/GetMatchCountQueryHandler.cs`
- modify: `src/Winterplein.Application/Commands/GenerateMatches/GenerateMatchesCommandHandler.cs`
