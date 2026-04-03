# Story 4 — Build Player & Match API

**Epic:** Epic 1 — Match Generation
**Dependencies:** Story 3 (match generation service)

## Description

Expose the player management and match generation functionality via a minimal ASP.NET Core Web API. The API uses in-memory storage for this epic.

## Acceptance Criteria

### Endpoints (`Winterplein.Api/`)

| Method | Route | Description | Request | Response |
|--------|-------|-------------|---------|----------|
| GET | `/api/players` | List all players | — | `List<PlayerDto>` |
| POST | `/api/players` | Add a player | `AddPlayerRequest` | `PlayerDto` (201) |
| DELETE | `/api/players/{id}` | Remove a player | — | 204 / 404 |
| POST | `/api/matches/generate` | Generate all matches | — | `GenerateMatchesResponse` |
| GET | `/api/matches/count` | Preview match count | — | `{ count: int }` |

### Infrastructure (`Winterplein.Infrastructure/Persistence/InMemoryPlayerRepository.cs`)

- Implements `IPlayerRepository` (from Application)
- Thread-safe in-memory `List<Player>` with a lock
- Registered as `Singleton` (shared state across requests within a session)

### DI registration

- `IPlayerRepository` → `InMemoryPlayerRepository` (Singleton)
- `IMatchGeneratorService` → `MatchGeneratorService` (Singleton)

### Validation

- `POST /api/players`: reject empty/whitespace names with 400
- `DELETE /api/players/{id}`: return 404 if player not found

### CORS

- Allow requests from `http://localhost:5173` (Vite/Blazor WASM dev server) and the production client URL

## Technical Notes

- Use Minimal API route groups (`app.MapGroup("/api/players")`)
- Return `TypedResults` for proper OpenAPI type inference
- DTOs come from `Winterplein.Shared` — no domain types leak into API responses
- Map Domain → DTO inside endpoint handlers (or a thin mapper class)

## Tasks

- [x] T1: Implement `InMemoryPlayerRepository` in Infrastructure (blockedBy: `IPlayerRepository` from Story 3)
- [x] T2: Register `IPlayerRepository` and `IMatchGeneratorService` in DI (blockedBy: T1)
- [x] T3: Implement player endpoints (`GET`, `POST`, `DELETE /api/players`) (blockedBy: T2)
- [x] T4: Add validation — reject empty names (400), missing player (404) (blockedBy: T3)
- [x] T5: Implement match endpoints (`POST /api/matches/generate`, `GET /api/matches/count`) (blockedBy: T2)
- [x] T6: Add Swagger (Swashbuckle) — OpenAPI spec generation and Swagger UI at `/swagger` (blockedBy: T3, T5)
- [x] T7: Add `WinterpleinApiFactory` (`WebApplicationFactory<Program>`) in `Winterplein.IntegrationTests` — resets `InMemoryPlayerRepository` between tests (blockedBy: T3, T5)
- [x] T8: Integration tests for `GET /api/players` — returns 200 with empty list when no players exist (blockedBy: T7)
- [x] T9: Integration tests for `POST /api/players` — returns 201 with `PlayerDto` and `Location` header; returns 400 for blank name (blockedBy: T7)
- [x] T10: Integration tests for `DELETE /api/players/{id}` — returns 204 for known player; returns 404 for unknown id (blockedBy: T7)
- [x] T11: Integration tests for `POST /api/matches/generate` — returns 200 with correct match count after adding players (blockedBy: T7)
- [x] T12: Integration tests for `GET /api/matches/count` — returns 200 with correct count (blockedBy: T7)
