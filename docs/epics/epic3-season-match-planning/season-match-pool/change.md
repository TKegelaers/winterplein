# season-match-pool

## Problem Statement

A season has enrolled players but there is no way to view all possible doubles matches that can be played within that season. The match pool is the foundation for schedule planning.

## Proposed Solution

Add a `GetSeasonMatchPoolQuery` that computes the full match pool on-the-fly from the season's enrolled players using the existing `IMatchGeneratorService.GenerateAllMatches`. Expose it via a read-only API endpoint and display it on the season detail page as a collapsible, paged table. The pool is never persisted — it is recomputed each request.

## Business Requirements

**Given** a season with at least 4 enrolled players
**When** `GET /api/seasons/{id}/match-pool` is called
**Then** all possible doubles matches are returned as a `GenerateMatchesResponse`

**Given** a season with fewer than 4 enrolled players
**When** `GET /api/seasons/{id}/match-pool` is called
**Then** a `GenerateMatchesResponse` with an empty `Matches` list and `TotalCount` 0 is returned (HTTP 200)

**Given** a season id that does not exist
**When** `GET /api/seasons/{id}/match-pool` is called
**Then** HTTP 404 is returned

## Acceptance Criteria

- [ ] `GetSeasonMatchPoolQuery(int SeasonId)` query type in `Winterplein.Application.IO/Queries/`
- [ ] Handler `GetSeasonMatchPoolQueryHandler` in `Winterplein.Application/QueryHandlers/GetSeasonMatchPool/`: loads the season via `ISeasonRepository`, returns `null` for an unknown season, generates matches via `IMatchGeneratorService.GenerateAllMatches(season.Players)`, maps to `GenerateMatchesResponse`, returns an empty response for fewer than 4 players
- [ ] `GET /api/seasons/{id}/match-pool` endpoint on `SeasonsController` — 200 (with or without matches) or 404 for unknown season, invoked via `IMessageBus.InvokeAsync`
- [ ] `GetMatchPoolAsync(int seasonId)` added to `SeasonApiClient` (path `/api/seasons/{id}/match-pool`)
- [ ] Match pool section on `SeasonDetail.razor`: collapsible panel showing the total count and a **paged** `MudTable` of all matches; shows a "not enough players" message when the pool is empty
- [ ] Unit tests: returns matches for 4+ players, `null` for unknown season, empty response for fewer than 4 players
- [ ] Integration tests: 200 with matches, 200 empty for fewer than 4 players, 404 for unknown season

## Technical Notes

- Match pool is never persisted — computed on-the-fly from enrolled players each request
- Reuses existing `GenerateMatchesResponse` / `MatchDto` / `TeamDto` / `PlayerDto` DTOs
- Existing convention: queries live in `Winterplein.Application.IO/Queries/`, handlers as static `Handle` methods under `Winterplein.Application/QueryHandlers/<QueryName>/` (the previously referenced `MatchdayPlans/` folder does not exist after the Epic 8 restructure)
- `Match` → DTO mapping uses an extension-method mapper in `Winterplein.Application/Mappers/` (add one if no `Match`/`GenerateMatchesResponse` mapper exists yet)
- The pool grows combinatorially (10 players → 630 matches, 20 → 14,535); the UI table must page to stay responsive
- Match IDs in the pool are deterministic (sequential) for a given player set — Story 2 uses these IDs for uniqueness tracking
