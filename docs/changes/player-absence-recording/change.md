# player-absence-recording

## Problem Statement

The schedule generator assumes all enrolled players are available on every matchday. There is no way to mark a player as absent on specific dates, which causes the scheduler to generate matches with unavailable players.

## Proposed Solution

Implement a `PlayerAbsence` entity and per-season/per-player absence management. A PUT-based "replace all absences" API replaces the full absence set per player per season. The UI provides a checkbox dialog per player listing all season matchdays.

## Business Requirements

**Given** a player is enrolled in a season
**When** a user marks the player absent on specific matchdays
**Then** those absences are stored and replace any previous absences for that player in that season

## Acceptance Criteria

- [ ] `PlayerAbsence` entity: `Id`, `SeasonId`, `PlayerId`, `Date`; constructor validates non-default date
- [ ] `PlayerAbsenceDto` and `SetPlayerAbsencesRequest` in `Winterplein.Shared/DTOs/`
- [ ] `IPlayerAbsenceRepository`: `GetBySeasonAndPlayer`, `GetBySeason`, `GetBySeasonAndDate`, `ReplaceForSeasonAndPlayer`
- [ ] `InMemoryPlayerAbsenceRepository` using `ConcurrentDictionary`, registered as Singleton
- [ ] `SetPlayerAbsencesCommand` handler: validates season, player enrollment, and that all dates are valid matchdays; replaces absences
- [ ] `GetPlayerAbsencesQuery` handler: returns null for unknown season/player
- [ ] `PUT /api/seasons/{id}/players/{playerId}/absences` and `GET /api/seasons/{id}/players/{playerId}/absences`
- [ ] Player absence dialog on season detail page: MudDialog with matchday checkboxes per player; absence count badge on trigger button
- [ ] `PlayerAbsenceBuilder` in test commons; unit and integration tests

## Technical Notes

- PUT with full replacement semantics (`SetPlayerAbsencesRequest` with all dates) avoids add/remove choreography — client always sends the complete set
- `GetBySeasonAndDate` is the key query for absence-aware schedule generation (Story 2 of this epic)
- `PlayerAbsence` stores IDs only — no reference to `Player` or `Season` objects; validation happens at command time
