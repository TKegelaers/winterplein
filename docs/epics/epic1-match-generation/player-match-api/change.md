# player-match-api

## Problem Statement

The match generation logic exists but is not accessible over HTTP. There are no API endpoints for managing players or generating matches, and no in-memory persistence.

## Proposed Solution

Expose player management (CRUD) and match generation via ASP.NET Core API endpoints. Back the player list with an `InMemoryPlayerRepository`. Register all services in DI.

## Business Requirements

**Given** players have been added via the API
**When** `POST /api/matches/generate` is called
**Then** all possible doubles matches are returned

## Acceptance Criteria

- [ ] `GET /api/players` — returns `List<PlayerDto>` 200
- [ ] `POST /api/players` — adds player, returns `PlayerDto` 201; returns 400 for empty/whitespace names
- [ ] `DELETE /api/players/{id}` — returns 204; returns 404 if not found
- [ ] `POST /api/matches/generate` — returns `GenerateMatchesResponse` 200
- [ ] `GET /api/matches/count` — returns match count 200
- [ ] `InMemoryPlayerRepository` in `Winterplein.Infrastructure/Persistence/` — thread-safe, registered as Singleton
- [ ] Integration tests for all 5 endpoints via `WebApplicationFactory<Program>`

## Potential Pitfalls

- DTOs come from `Winterplein.Shared` — no domain types leak into API responses
- `InMemoryPlayerRepository` must be registered as Singleton (shared state across requests in a session)
