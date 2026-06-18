# add-match-pool-query-handler

## Scope

Add the read-side query that computes a season's match pool on demand.

- Create `GetSeasonMatchPoolQuery(int SeasonId)` query record in `Winterplein.Application.IO/Queries/`.
- Create `GetSeasonMatchPoolQueryHandler` (static `Handle`) under `Winterplein.Application/QueryHandlers/GetSeasonMatchPool/`:
  - Inject `ISeasonRepository` and `IMatchGeneratorService` plus `CancellationToken`.
  - Load the season via `GetByIdAsync`; return `null` when not found.
  - When `season.Players.Count < 4`, return `new GenerateMatchesResponse([], 0)`.
  - Otherwise call `GenerateAllMatches(season.Players)` and map to `GenerateMatchesResponse`.
  - Return type: `Task<GenerateMatchesResponse?>`.
- Add a `ToResponse(this IReadOnlyList<Match> matches)` (or `List<Match>`) factory to `MatchMapper` that builds `GenerateMatchesResponse` (matches mapped via `ToDto()`, `TotalCount` = count).
- Refactor `GenerateMatchesCommandHandler` to use the new `ToResponse` mapper.

## Domain model changes

None. Reuses existing `GenerateMatchesResponse` / `MatchDto` / `TeamDto` / `PlayerDto`.

## Test cases

- SeasonMatchPoolHandlerTests.cs (`tests/Winterplein.Application.UnitTests/Seasons/`)
  - GetSeasonMatchPoolQueryHandler_ReturnsMatches_ForFourOrMorePlayers
  - GetSeasonMatchPoolQueryHandler_ReturnsEmptyResponse_ForFewerThanFourPlayers
  - GetSeasonMatchPoolQueryHandler_ReturnsNull_ForUnknownSeason

Use `Mock<ISeasonRepository>`, `Mock<IMatchGeneratorService>` (or the real `MatchGeneratorService`), `SeasonBuilder` and `PlayerBuilder`. The 4+ case can build a season with 4 players and assert `TotalCount == 3` and `Matches` count == 3.

## Affected files

- create: src/Winterplein.Application.IO/Queries/GetSeasonMatchPoolQuery.cs
- create: src/Winterplein.Application/QueryHandlers/GetSeasonMatchPool/GetSeasonMatchPoolQueryHandler.cs
- modify: src/Winterplein.Application/Mappers/MatchMapper.cs
- modify: src/Winterplein.Application/CommandHandlers/GenerateMatches/GenerateMatchesCommandHandler.cs
- create: tests/Winterplein.Application.UnitTests/Seasons/SeasonMatchPoolHandlerTests.cs
