# season-tests

## Problem Statement

The season domain logic, CQRS handlers, and API endpoints were implemented without a complete test suite. Unit and integration tests are needed to verify correctness and prevent regressions.

## Proposed Solution

Write unit tests for domain logic (`GetMatchdays`, validation, `AddPlayer`/`RemovePlayer`), CQRS handler tests with mocked repositories, and integration tests for full CRUD cycles and edge cases via `WebApplicationFactory`.

## Business Requirements

**Given** the season domain and API are implemented
**When** the full test suite runs
**Then** all unit and integration tests pass

## Acceptance Criteria

- [ ] Unit tests for `GetMatchdays()`: correct dates, empty when no match, boundary dates included
- [ ] Unit tests for `Season` validation: throws on empty name, EndDate ≤ StartDate, EndHour ≤ StartHour
- [ ] Unit tests for `AddPlayer`/`RemovePlayer`: happy path, null throws, duplicate throws, not-found throws, min-4-players constraint
- [ ] Unit tests for all 5 + 3 CQRS handlers (mocked `ISeasonRepository` / `IPlayerRepository`)
- [ ] Integration tests: full CRUD cycle, validation errors (400), matchday endpoint, player-season endpoints
- [ ] `WithPlayer(Player)` method added to `SeasonBuilder` in `tests/Winterplein.UnitTests.Common/Builders/`

## Technical Notes

- Unit tests use Moq for `ISeasonRepository` and `IPlayerRepository` mocks
- `DateOnly` serialization in `System.Text.Json` requires a custom converter — verify in integration tests
- Integration tests use the real `InMemorySeasonRepository` via `WebApplicationFactory<Program>`
