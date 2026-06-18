# schedule-api-endpoint

## Scope

Expose schedule generation over HTTP and add the client method.

- `POST /api/seasons/{id:int}/schedule/generate` on `SeasonsController`:
  - Request: no body (season id from route).
  - Response 200 OK: `GenerateScheduleResponse`.
  - Response 404 Not Found: unknown season.
  - Uses `bus.InvokeAsync<GenerateScheduleResponse?>(new GenerateScheduleCommand(id))`; `null` -> `NotFound()`, else `Ok(result)` (mirrors `GetMatchPool`).
- `SeasonApiClient.GenerateScheduleAsync(int seasonId)`: `PostAsJsonAsync` (empty body) to the endpoint, returns `GenerateScheduleResponse?` (null on 404), using shared `_json`.

## Domain model changes

None.

## Test cases

- SeasonScheduleTests.cs (IntegrationTests/Seasons, real SQL Server + Respawn)
  - GenerateSchedule_Returns200_WithPersistedPlannedMatches (>= 4 players, matchdays filled)
  - GenerateSchedule_IsIdempotent_OnRerun (second POST adds nothing, counts stable, dates unchanged)
  - GenerateSchedule_Returns404_ForUnknownSeason
  - GenerateSchedule_Returns200_EmptyPlan_ForFewerThanFourPlayers

## Affected files

- modify: src/Winterplein.WebApi/Controllers/SeasonsController.cs
- modify: src/Winterplein.Client/Services/SeasonApiClient.cs
- create: tests/Winterplein.IntegrationTests/Seasons/SeasonScheduleTests.cs
  </content>
