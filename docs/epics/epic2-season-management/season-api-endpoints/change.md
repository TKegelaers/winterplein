# season-api-endpoints

## Problem Statement

Season CQRS handlers exist but are not exposed over HTTP. There are no REST endpoints for season CRUD, matchday computation, or player-season membership management.

## Proposed Solution

Implement a `SeasonsController` with full CRUD endpoints plus matchday and player-season endpoints. A static `SeasonMapper` converts domain entities to DTOs. All actions delegate to Wolverine via `IMessageBus`.

## Business Requirements

**Given** a season exists
**When** `GET /api/seasons/{id}/matchdays` is called
**Then** the computed list of matchday dates is returned

## Acceptance Criteria

- [ ] `GET /api/seasons`, `GET /api/seasons/{id}`, `POST /api/seasons`, `PUT /api/seasons/{id}`, `DELETE /api/seasons/{id}`, `GET /api/seasons/{id}/matchdays`
- [ ] `GET /api/seasons/{id}/players`, `POST /api/seasons/{id}/players`, `DELETE /api/seasons/{id}/players/{playerId}`
- [ ] `SeasonsController` inherits `ControllerBase` with `[ApiController]` and `[Route("api/seasons")]`
- [ ] Static `SeasonMapper` in `Winterplein.Api/Mappers/` with `ToDto(Season)` populating `Matchdays` from `season.GetMatchdays()`
- [ ] Validation returns 400 for empty name, EndDate ≤ StartDate, EndHour ≤ StartHour
- [ ] 404 for unknown season/player; 201 on create with `SeasonDto` body

## Technical Notes

- All actions use `IMessageBus.InvokeAsync<T>(message)` — no business logic in the controller
- `SeasonMapper.ToDto` populates `Matchdays` by calling `season.GetMatchdays()` — matchdays are not stored
