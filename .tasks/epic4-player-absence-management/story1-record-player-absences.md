# Story 1 — Record Player Absences

**Epic:** Epic 4 — Player Absence Management
**Dependencies:** Epic 2 (Season entity with Players, ISeasonRepository, Season API + UI)

## Description

Allow a user to mark or unmark a player as absent on specific matchdays within a season. Absences are stored per season, per player, per date. The UI provides a simple toggle per matchday for each enrolled player.

## Acceptance Criteria

### Domain (`Winterplein.Domain/Entities/PlayerAbsence.cs`)

- `Id` (int), `SeasonId` (int), `PlayerId` (int), `Date` (DateOnly)
- Constructor accepts `int id`, `int seasonId`, `int playerId`, `DateOnly date`
- Validation: `date` must not be default

### DTOs (`Winterplein.Shared/DTOs/`)

- **`PlayerAbsenceDto`** — `record PlayerAbsenceDto(int Id, int SeasonId, int PlayerId, DateOnly Date)`
- **`SetPlayerAbsencesRequest`** — `record SetPlayerAbsencesRequest(List<DateOnly> Dates)` — replaces all absences for a player in a season with the given dates

### CQRS & Repository (`Winterplein.Application/PlayerAbsences/`)

- **`IPlayerAbsenceRepository`** in `Interfaces/`
  - `List<PlayerAbsence> GetBySeasonAndPlayer(int seasonId, int playerId)`
  - `List<PlayerAbsence> GetBySeason(int seasonId)`
  - `List<PlayerAbsence> GetBySeasonAndDate(int seasonId, DateOnly date)` — used by Story 2 for filtering
  - `void ReplaceForSeasonAndPlayer(int seasonId, int playerId, List<PlayerAbsence> absences)` — removes existing and adds new
- **`SetPlayerAbsencesCommand(int SeasonId, int PlayerId, List<DateOnly> Dates)`** → `List<PlayerAbsenceDto>?`
  - Loads season via `ISeasonRepository` — returns null if not found
  - Validates player is enrolled in the season — returns null if not found
  - Validates all dates are valid matchdays via `season.GetMatchdays()` — throws if any date is invalid
  - Calls `repository.ReplaceForSeasonAndPlayer()` with new `PlayerAbsence` entities
  - Returns the updated list of absences
- **`GetPlayerAbsencesQuery(int SeasonId, int PlayerId)`** → `List<PlayerAbsenceDto>?`
  - Returns null if season not found or player not enrolled
  - Returns absences for the given player in the season
- **`PlayerAbsenceMapper`** in `Mappers/` — `ToDto(PlayerAbsence)` extension method

### Infrastructure (`Winterplein.Infrastructure/Persistence/`)

- **`InMemoryPlayerAbsenceRepository`** backed by `ConcurrentDictionary<int, PlayerAbsence>` (keyed by auto-incrementing ID, same pattern as `InMemorySeasonRepository`)
  - `GetBySeasonAndPlayer` filters by `SeasonId` and `PlayerId`
  - `GetBySeason` filters by `SeasonId`
  - `GetBySeasonAndDate` filters by `SeasonId` and `Date`
  - `ReplaceForSeasonAndPlayer` removes all entries matching `(SeasonId, PlayerId)`, then adds the new entries with assigned IDs
- Registered as Singleton in DI

### API Endpoints (on `SeasonsController`)

| Method | Route                                           | Request                    | Response                           |
| ------ | ----------------------------------------------- | -------------------------- | ---------------------------------- |
| PUT    | `/api/seasons/{id}/players/{playerId}/absences` | `SetPlayerAbsencesRequest` | `List<PlayerAbsenceDto>` 200 / 404 |
| GET    | `/api/seasons/{id}/players/{playerId}/absences` | —                          | `List<PlayerAbsenceDto>` 200 / 404 |

- PUT replaces all absences for the player in the season; returns 404 if season or player not found; returns 400 if any date is not a valid matchday
- GET returns 404 if season or player not found

### Blazor UI (`Winterplein.Client/`)

- Add `GetPlayerAbsencesAsync(int seasonId, int playerId)` and `SetPlayerAbsencesAsync(int seasonId, int playerId, SetPlayerAbsencesRequest request)` to `SeasonApiClient`
- **Player Absences Dialog** — accessible from the season detail page's player list via an "Absences" `MudIconButton` per player
  - `MudDialog` showing the player name and a list of all matchdays with `MudCheckBox` per date
  - Checked = absent, unchecked = available
  - On open: loads current absences via GET, pre-checks the absent dates
  - On save: sends PUT with all checked dates, closes dialog
  - Shows absence count badge on the icon button (e.g. "3" if 3 absences)

### Tests

- **`PlayerAbsenceBuilder`** in `tests/Winterplein.UnitTests.Common/Builders/` — fluent builder with `.WithId()`, `.WithSeasonId()`, `.WithPlayerId()`, `.WithDate()`
- **Unit tests** for `PlayerAbsence` domain: constructor succeeds, throws on default date
- **Unit tests** for `SetPlayerAbsencesCommandHandler`:
  - Returns null when season not found
  - Returns null when player not enrolled
  - Throws when date is not a valid matchday
  - Replaces absences (happy path — set 2 dates, then set 1 date, verify only 1 remains)
  - Returns empty list when called with empty dates (clears all absences)
- **Unit tests** for `GetPlayerAbsencesQueryHandler`: returns null for unknown season, returns absences for known player
- **Integration tests**: PUT set absences → 200; GET absences → 200; PUT unknown season → 404; PUT invalid date → 400; PUT then GET round-trip

## Technical Notes

- Using PUT with a full replacement semantics (`SetPlayerAbsencesRequest` with all dates) avoids complex add/remove choreography in the UI — the client sends the complete set of absent dates on every save
- `GetBySeasonAndDate` is the key query for Story 2 — it returns all absences on a given matchday so the schedule generator can filter out matches with absent players
- Handlers live in `Winterplein.Application/PlayerAbsences/`
- The `PlayerAbsence` entity is intentionally simple (no reference to `Player` or `Season` objects) — it stores IDs only, validated against the Season's enrolled players and matchdays at command time

## Tasks

- [ ] T1: Create `PlayerAbsence` entity with properties and constructor validation
- [ ] T2: Create `PlayerAbsenceDto` and `SetPlayerAbsencesRequest` records in `Winterplein.Shared/DTOs/`
- [ ] T3: Define `IPlayerAbsenceRepository` interface (blockedBy: T1)
- [ ] T4: Implement `InMemoryPlayerAbsenceRepository` using `ConcurrentDictionary` (blockedBy: T3)
- [ ] T5: Register `InMemoryPlayerAbsenceRepository` as Singleton in DI (blockedBy: T4)
- [ ] T6: Create `PlayerAbsenceMapper.ToDto()` extension method (blockedBy: T1, T2)
- [ ] T7: Create `SetPlayerAbsencesCommand` + handler (blockedBy: T3, T6)
- [ ] T8: Create `GetPlayerAbsencesQuery` + handler (blockedBy: T3, T6)
- [ ] T9: Implement PUT and GET API actions on `SeasonsController` (blockedBy: T5, T7, T8)
- [ ] T10: Add `GetPlayerAbsencesAsync` and `SetPlayerAbsencesAsync` to `SeasonApiClient` (blockedBy: T9)
- [ ] T11: Build player absences dialog with matchday checkboxes (blockedBy: T10)
- [ ] T12: Create `PlayerAbsenceBuilder` in test commons (blockedBy: T1)
- [ ] T13: Write unit tests for domain and CQRS handlers (blockedBy: T7, T8, T12)
- [ ] T14: Write integration tests for PUT and GET endpoints (blockedBy: T9)
