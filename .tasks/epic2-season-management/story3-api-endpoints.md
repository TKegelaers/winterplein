# Story 3 — Build Season API

**Epic:** Epic 2 — Season Management
**Dependencies:** Story 2

## Description

Expose season CRUD and matchday computation via an ASP.NET Core Controller, using MediatR and a static mapper.

## Acceptance Criteria

### Endpoints

| Method | Route | Request | Response |
|--------|-------|---------|----------|
| GET | `/api/seasons` | — | `List<SeasonDto>` 200 |
| GET | `/api/seasons/{id}` | — | `SeasonDto` 200 / 404 |
| POST | `/api/seasons` | `CreateSeasonRequest` | `SeasonDto` 201 |
| PUT | `/api/seasons/{id}` | `UpdateSeasonRequest` | `SeasonDto` 200 / 404 |
| DELETE | `/api/seasons/{id}` | — | 204 / 404 |
| GET | `/api/seasons/{id}/matchdays` | — | `List<DateOnly>` 200 / 404 |

- `{id}` route parameters are `int`
- Controller inherits from `ControllerBase` with `[ApiController]` and `[Route("api/seasons")]`
- Return types use `IActionResult`
- Validation returns 400 for empty name, EndDate <= StartDate, EndHour <= StartHour

### Mapper

- Static `SeasonMapper` class in `Winterplein.Api/Mappers/` with `ToDto(Season)` method

## Technical Notes

- Each action delegates to MediatR — no business logic in the controller
- `SeasonMapper.ToDto` populates `Matchdays` by calling `season.GetMatchdays()`
- Register MediatR scanning `Winterplein.Application` assembly and `ISeasonRepository` → `InMemorySeasonRepository` in `Program.cs`

## Tasks

- [x] T1: Create static `SeasonMapper` class with `ToDto(Season season)` method mapping all fields including `Players` (using existing `PlayerMapper`) (blockedBy: Story 1 T5, T6)
- [x] T2: Implement GET `/api/seasons` and GET `/api/seasons/{id:int}` actions (blockedBy: T1, Story 2 T3, Story 2 T4)
- [x] T3: Implement POST `/api/seasons` with request validation (blockedBy: T1, Story 2 T2)
- [x] T4: Implement PUT `/api/seasons/{id:int}` with request validation (blockedBy: T1, Story 2 T5)
- [x] T5: Implement DELETE `/api/seasons/{id:int}` action (blockedBy: T1, Story 2 T6)
- [x] T6: Implement GET `/api/seasons/{id:int}/matchdays` action (blockedBy: T2)
- [x] T7: Register MediatR + `ISeasonRepository` DI in `Program.cs` (blockedBy: Story 2 T8)
- [x] T8: Implement player-season actions (blockedBy: T1, Story 2 T10, T11, T12):
  - `GET /api/seasons/{id:int}/players` → `List<PlayerDto>` 200 / 404
  - `POST /api/seasons/{id:int}/players` body: `AddSeasonPlayerRequest` → `SeasonDto` 200 / 404 / 400
  - `DELETE /api/seasons/{id:int}/players/{playerId:int}` → 204 / 404 / 400
