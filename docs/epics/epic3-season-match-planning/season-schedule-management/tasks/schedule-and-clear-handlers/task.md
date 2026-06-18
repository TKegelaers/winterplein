# schedule-and-clear-handlers

## Scope

Add the three Wolverine handlers (static `Handle`, discovered by convention).

**GetSeasonScheduleQueryHandler** (`QueryHandlers/GetSeasonSchedule/`)

- Deps: `ISeasonRepository`, `IPlannedMatchRepository`.
- `GetByIdAsync(SeasonId)`; if null → return `null` (→ 404).
- Build a date→planned map from `GetAllBySeasonAsync`. For each date in `season.GetMatchdays()` (already ordered) emit `MatchdayScheduleEntryDto(date, plannedOrNull?.ToDto(), IsPlanned: planned != null)`. Return `SeasonScheduleResponse(entries)`.
- Returns `Task<SeasonScheduleResponse?>`.

**ClearPlannedMatchCommandHandler** (`CommandHandlers/ClearPlannedMatch/`) — void

- Deps: `ISeasonRepository`, `IPlannedMatchRepository`.
- `GetByIdAsync(SeasonId)`; null → `throw KeyNotFoundException`.
- `DeleteBySeasonAndDateAsync(SeasonId, Date)`; if `false` → `throw KeyNotFoundException`.

**ClearAllPlannedMatchesCommandHandler** (`CommandHandlers/ClearAllPlannedMatches/`) — void

- Deps: `ISeasonRepository`, `IPlannedMatchRepository`.
- `GetByIdAsync(SeasonId)`; null → `throw KeyNotFoundException`.
- `DeleteAllBySeasonAsync(SeasonId)` (idempotent; no throw when already empty).

Reuse `PlannedMatchMapper.ToDto()` for the entry projection.

## Domain model changes

None. Uses existing `Season.GetMatchdays()` and `PlannedMatch`.

## Test cases

Application unit tests (mock both repos, style of `GenerateScheduleHandlerTests`):

- GetSeasonScheduleHandlerTests.cs
  - BuildsEntry_PerMatchday_InDateOrder
  - MarksMatchday_Planned_WhenPlannedMatchExists
  - MarksMatchday_Open_WhenNoPlannedMatch (PlannedMatch null, IsPlanned false)
  - ReturnsNull_ForUnknownSeason
- ClearPlannedMatchHandlerTests.cs
  - Deletes_PlannedMatch_AtDate
  - Throws_KeyNotFound_WhenSeasonUnknown
  - Throws_KeyNotFound_WhenNoPlannedMatchAtDate (repo returns false)
- ClearAllPlannedMatchesHandlerTests.cs
  - Deletes_AllPlannedMatches_ForSeason
  - DoesNotThrow_WhenSeasonAlreadyEmpty (idempotent)
  - Throws_KeyNotFound_WhenSeasonUnknown

## Affected files

- create: src/Winterplein.Application/QueryHandlers/GetSeasonSchedule/GetSeasonScheduleQueryHandler.cs
- create: src/Winterplein.Application/CommandHandlers/ClearPlannedMatch/ClearPlannedMatchCommandHandler.cs
- create: src/Winterplein.Application/CommandHandlers/ClearAllPlannedMatches/ClearAllPlannedMatchesCommandHandler.cs
- create: tests/Winterplein.Application.UnitTests/Seasons/GetSeasonScheduleHandlerTests.cs
- create: tests/Winterplein.Application.UnitTests/Seasons/ClearPlannedMatchHandlerTests.cs
- create: tests/Winterplein.Application.UnitTests/Seasons/ClearAllPlannedMatchesHandlerTests.cs
