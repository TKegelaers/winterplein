# season-domain-and-dtos

## Problem Statement

The application supports only ad-hoc match generation from a flat player list. There is no concept of a season — a named, time-bounded period with a fixed weekday, start/end times, and an enrolled player roster.

## Proposed Solution

Implement the `Season` domain entity with computed matchday enumeration and the shared DTOs for season data transfer. The `Season` entity holds all scheduling metadata and computes its matchdays on-the-fly.

## Business Requirements

**Given** a season with a start date, end date, and a target weekday
**When** `GetMatchdays()` is called
**Then** it returns all dates in [StartDate, EndDate] that fall on the given weekday

**Given** invalid season parameters
**When** the `Season` is constructed
**Then** an `ArgumentException` is thrown (empty name, EndDate ≤ StartDate, EndHour ≤ StartHour)

## Acceptance Criteria

- [ ] `Season` entity in `Winterplein.Domain/Entities/`: `Id`, `Name`, `StartDate`, `EndDate`, `Weekday`, `StartHour`, `EndHour`; `GetMatchdays()` → `IReadOnlyList<DateOnly>`; constructor validation
- [ ] `Season` has a private `List<Player> _players` backing field exposed as `IReadOnlyList<Player> Players`; `AddPlayer` and `RemovePlayer` with appropriate validation
- [ ] `SeasonDto`, `CreateSeasonRequest`, `UpdateSeasonRequest`, `AddSeasonPlayerRequest` in `Winterplein.Shared/DTOs/`

## Technical Notes

- Matchdays are computed, not stored — `GetMatchdays()` iterates the date range; no persistence needed
- `Season` constructor enforces all validation; DTOs are records with no domain logic
- `RemovePlayer` throws if removal would leave fewer than 4 enrolled players
