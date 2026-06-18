# api-and-client

## Scope

Expose the three operations over HTTP and add matching `SeasonApiClient` methods.

**SeasonsController** (`api/seasons`)

- `GET {id:int}/schedule` → `bus.InvokeAsync<SeasonScheduleResponse?>(new GetSeasonScheduleQuery(id))`; `?? NotFound()` else `Ok(result)`. (200 / 404)
- `DELETE {id:int}/matchdays/{date}/planned-match` → `await bus.InvokeAsync(new ClearPlannedMatchCommand(id, date)); return NoContent();` — `date` bound as `DateOnly` route param (first such route; ASP.NET Core 10 binds natively). (204 / 404 via KeyNotFound)
- `DELETE {id:int}/schedule` → `await bus.InvokeAsync(new ClearAllPlannedMatchesCommand(id)); return NoContent();` (204 / 404)

**SeasonApiClient**

- `Task<SeasonScheduleResponse?> GetScheduleAsync(int seasonId)` — GET, 404 → null (style of `GetMatchPoolAsync`).
- `Task<bool> ClearPlannedMatchAsync(int seasonId, DateOnly date)` — DELETE, 404 → false (style of `RemovePlayerFromSeasonAsync`); format date as ISO `yyyy-MM-dd` in the URL.
- `Task<bool> ClearAllPlannedMatchesAsync(int seasonId)` — DELETE `/schedule`, 404 → false.

## Domain model changes

None.

## Test cases

Integration tests (real SQL Server + Respawn), extend/add in `Seasons/SeasonScheduleTests.cs`:

- GetSchedule_Returns200_WithEntryPerMatchday (each matchday present, ordered by date)
- GetSchedule_MarksPlannedAndOpen_AfterPartialGenerate (IsPlanned true for filled, false for open)
- GetSchedule_Returns404_ForUnknownSeason
- ClearPlannedMatch_Returns204_AndRemovesMatch (DateOnly route binds; entry becomes open afterwards)
- ClearPlannedMatch_Returns404_WhenNoMatchAtDate
- ClearPlannedMatch_Returns404_ForUnknownSeason
- ClearAll_Returns204_AndRemovesEveryMatch
- ClearAll_Returns204_WhenAlreadyEmpty (idempotent)
- ClearAll_Returns404_ForUnknownSeason
- RoundTrip_GenerateClearOneRegenerate_RefillsClearedMatchday (acceptance: generate → clear one matchday → re-generate fills that matchday again)

## Affected files

- modify: src/Winterplein.WebApi/Controllers/SeasonsController.cs
- modify: src/Winterplein.Client/Services/SeasonApiClient.cs
- modify: tests/Winterplein.IntegrationTests/Seasons/SeasonScheduleTests.cs
