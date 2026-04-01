# Story 5 — Tests

**Epic:** Epic 2 — Season Management
**Dependencies:** Story 1 (unit tests), Story 3 (integration tests)

## Description

Write unit tests for domain logic and CQRS handlers, and integration tests for the API endpoints.

## Acceptance Criteria

### Unit tests (`tests/Winterplein.UnitTests/Seasons/`)

- **`GetMatchdays()` correctness:**
  - Returns correct dates for a typical range (e.g. Jan–Mar, every Monday)
  - Returns empty list when no weekday matches fall in range
  - StartDate and EndDate on the target weekday are included
  - Single-day range with matching weekday returns one date
- **Validation:**
  - Throws when Name is empty or whitespace
  - Throws when EndDate <= StartDate
  - Throws when EndHour <= StartHour
- **CQRS handler tests (mocked `ISeasonRepository`):**
  - `CreateSeasonCommandHandler` — creates season and returns an `int` Id
  - `GetSeasonsQueryHandler` — returns all seasons from repository
  - `GetSeasonByIdQueryHandler` — returns correct season; returns null for unknown Id
  - `UpdateSeasonCommandHandler` — updates season; returns false for unknown Id
  - `DeleteSeasonCommandHandler` — deletes season; returns false for unknown Id

### Integration tests (`tests/Winterplein.IntegrationTests/Seasons/`)

- Full CRUD cycle via `WebApplicationFactory<Program>`:
  1. POST creates season → 201 with SeasonDto body
  2. GET list returns the created season
  3. GET by id returns the season
  4. PUT updates name → 200 with updated SeasonDto
  5. DELETE removes season → 204
  6. GET by id after delete → 404
- Validation errors:
  - POST with empty name → 400
  - POST with EndDate <= StartDate → 400
  - POST with EndHour <= StartHour → 400
- Matchdays endpoint:
  - GET `/api/seasons/{id}/matchdays` returns correct computed dates
  - GET with unknown id → 404

## Technical Notes

- Unit tests use `Moq` (or NSubstitute if already in solution) to mock `ISeasonRepository`
- Integration tests use `WebApplicationFactory<Program>` with the real in-memory repository
- `DateOnly` serialization in `System.Text.Json` requires a custom converter; verify it works in integration tests

## Tasks

- [ ] T1: Write `GetMatchdays()` unit tests covering correctness and edge cases (blocks: Story 1 T1)
- [ ] T2: Write `Season` validation unit tests (blocks: Story 1 T2)
- [ ] T3: Write CQRS handler unit tests with mocked repository (blocks: Story 2)
- [ ] T4: Write integration tests for full CRUD cycle (blocks: Story 3)
- [ ] T5: Write integration tests for validation errors and matchdays endpoint (blocks: Story 3)
- [ ] T6: Write `Season.AddPlayer`/`RemovePlayer` unit tests: happy path, duplicate throws, null throws, not-found throws, remove throws when exactly 4 players enrolled (blocks: Story 1 T5)
- [ ] T7: Write handler unit tests for `AddSeasonPlayerCommandHandler`, `RemoveSeasonPlayerCommandHandler`, `GetSeasonPlayersQueryHandler` — happy path + not-found cases (blocks: Story 2 T10, T11, T12)
- [ ] T8: Write integration tests for player-season endpoints: add player 200, unknown season 404, unknown player 404, list players 200, remove player 204, remove unknown player 404, remove when only 4 enrolled → 400 (blocks: Story 3 T8)
- [ ] T9: Add `WithPlayer(Player)` method to `SeasonBuilder` in `tests/Winterplein.UnitTests.Common/Builders/` (blocks: Story 1 T5)
