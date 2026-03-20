# Story 4 — API Endpoints

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

- [ ] T1: Implement `InMemoryPlayerRepository` in Infrastructure (blocks: `IPlayerRepository` from Story 3)
- [ ] T2: Register `IPlayerRepository` and `IMatchGeneratorService` in DI (blocks: T1)
- [ ] T3: Implement player endpoints (`GET`, `POST`, `DELETE /api/players`) (blocks: T2)
- [ ] T4: Add validation — reject empty names (400), missing player (404) (blocks: T3)
- [ ] T5: Implement match endpoints (`POST /api/matches/generate`, `GET /api/matches/count`) (blocks: T2)
- [ ] T6: Smoke-test all endpoints with `curl` or Swagger (blocks: T3, T4, T5)
