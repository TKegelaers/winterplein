# Story 3 — Minimal API Endpoints

**Epic:** Epic 2 — Season Management
**Dependencies:** Story 2

## Description

Expose season CRUD and matchday computation via ASP.NET Core Minimal API endpoints, using MediatR and a static mapper.

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
- Grouped with `app.MapGroup("/api/seasons")`
- Return types use `TypedResults`
- Validation returns 400 for empty name, EndDate <= StartDate, EndHour <= StartHour

### Mapper

- Static `SeasonMapper` class in `Winterplein.Api/Mappers/` with `ToDto(Season)` method

## Technical Notes

- Each endpoint delegates to MediatR — no business logic in endpoint handlers
- `SeasonMapper.ToDto` populates `Matchdays` by calling `season.GetMatchdays()`
- Register MediatR scanning `Winterplein.Application` assembly and `ISeasonRepository` → `InMemorySeasonRepository` in `Program.cs`

## Tasks

- [ ] T1: Create static `SeasonMapper` class with `ToDto(Season season)` method
- [ ] T2: Implement GET `/api/seasons` and GET `/api/seasons/{id:int}` endpoints (blocks: T1, Story 2 T3, Story 2 T4)
- [ ] T3: Implement POST `/api/seasons` with request validation (blocks: T1, Story 2 T2)
- [ ] T4: Implement PUT `/api/seasons/{id:int}` with request validation (blocks: T1, Story 2 T5)
- [ ] T5: Implement DELETE `/api/seasons/{id:int}` endpoint (blocks: T1, Story 2 T6)
- [ ] T6: Implement GET `/api/seasons/{id:int}/matchdays` endpoint (blocks: T2)
- [ ] T7: Register MediatR + `ISeasonRepository` DI in `Program.cs` (blocks: Story 2 T8)
