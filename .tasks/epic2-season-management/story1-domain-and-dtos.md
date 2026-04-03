# Story 1 — Define Season Domain & DTOs

**Epic:** Epic 2 — Season Management
**Dependencies:** Epic 1 Story 1 (Done)

## Description

Implement the `Season` domain entity with computed matchday enumeration and the shared DTOs used to transfer season data between the API and the Blazor client.

## Acceptance Criteria

### Domain entity (`Winterplein.Domain/Entities/Season.cs`)

- `Id` (int), `Name` (string)
- `StartDate` (DateOnly), `EndDate` (DateOnly)
- `Weekday` (DayOfWeek), `StartHour` (TimeOnly), `EndHour` (TimeOnly)
- `GetMatchdays()` → `IReadOnlyList<DateOnly>` — enumerates all dates in [StartDate, EndDate] that fall on `Weekday`
- Validation: `EndDate > StartDate`, `EndHour > StartHour`, `Name` not empty

### Shared DTOs (`Winterplein.Shared/DTOs/`)

- **`SeasonDto`** — `Id` (int) + all entity fields + `Matchdays` (List\<DateOnly\>), `MatchdayCount` (int)
- **`CreateSeasonRequest`** — `Name`, `StartDate`, `EndDate`, `Weekday`, `StartHour`, `EndHour`
- **`UpdateSeasonRequest`** — same fields as `CreateSeasonRequest`

## Technical Notes

- Matchdays are **computed, not stored** — `GetMatchdays()` iterates the date range, no persistence needed
- DTOs are records; domain entity is a plain class with no framework dependencies
- `Season` constructor or factory method should enforce validation and throw `ArgumentException` on invalid input

## Tasks

- [ ] T1: Create `Season` entity with all properties and `GetMatchdays()` method
- [ ] T2: Add validation logic to `Season` (empty name, EndDate <= StartDate, EndHour <= StartHour) (blockedBy: T1)
- [ ] T3: Create `SeasonDto` record with `Id` (int), `Matchdays` and `MatchdayCount` (blockedBy: T1)
- [ ] T4: Create `CreateSeasonRequest` and `UpdateSeasonRequest` records
- [ ] T5: Add `Players` collection to `Season` entity — private `List<Player> _players`, exposed as `IReadOnlyList<Player> Players`; `AddPlayer(Player)` throws if null or already enrolled; `RemovePlayer(int playerId)` throws if not found or if removal would leave fewer than 4 players (blockedBy: T1)
- [ ] T6: Add `List<PlayerDto> Players` to `SeasonDto` record (blockedBy: T3)
- [ ] T7: Create `AddSeasonPlayerRequest(int PlayerId)` record in `Winterplein.Shared/DTOs/`
