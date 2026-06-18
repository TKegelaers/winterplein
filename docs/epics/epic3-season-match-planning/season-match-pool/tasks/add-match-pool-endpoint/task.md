# add-match-pool-endpoint

## Scope

Expose the match pool over HTTP and add the client method.

- Add `GET /api/seasons/{id:int}/match-pool` to `SeasonsController`:
  - `var pool = await bus.InvokeAsync<GenerateMatchesResponse?>(new GetSeasonMatchPoolQuery(id));`
  - `return pool == null ? NotFound() : Ok(pool);`
- Add `GetMatchPoolAsync(int seasonId)` to `SeasonApiClient`:
  - `GET /api/seasons/{seasonId}/match-pool`.
  - Return `null` on `HttpStatusCode.NotFound`; otherwise deserialize `GenerateMatchesResponse` (reuse the existing `_json` options).
  - Signature: `Task<GenerateMatchesResponse?> GetMatchPoolAsync(int seasonId)`.

## Domain model changes

None.

## Test cases

- SeasonMatchPoolTests.cs (`tests/Winterplein.IntegrationTests/Seasons/`) — `IntegrationTestBase`, HTTP round-trips
  - GetMatchPool_Returns200_WithMatches_ForFourOrMorePlayers (create season, create + enrol 4 players, assert 200 and `TotalCount == 3`)
  - GetMatchPool_Returns200_EmptyResponse_ForFewerThanFourPlayers (assert 200, `Matches` empty, `TotalCount` 0)
  - GetMatchPool_Returns404_ForUnknownSeason

May reuse the `CreateSeason` / `CreatePlayer` helper pattern from `SeasonsControllerTests` (or `SeasonSeedBuilder`).

## Affected files

- modify: src/Winterplein.WebApi/Controllers/SeasonsController.cs
- modify: src/Winterplein.Client/Services/SeasonApiClient.cs
- create: tests/Winterplein.IntegrationTests/Seasons/SeasonMatchPoolTests.cs
