# season-match-pool

## Problem Statement

A season has enrolled players but there is no way to view all possible doubles matches that can be played within that season. The match pool is the foundation for schedule planning.

## Proposed Solution

Add a `GetSeasonMatchPoolQuery` that computes the full match pool on-the-fly from the season's enrolled players using the existing `IMatchGeneratorService`. Expose it via a new API endpoint and display it on the season detail page.

## Business Requirements

**Given** a season with at least 4 enrolled players
**When** `GET /api/seasons/{id}/match-pool` is called
**Then** all possible doubles matches are returned as a `GenerateMatchesResponse`

## Acceptance Criteria

- [ ] `GetSeasonMatchPoolQuery(int SeasonId)` handler in `Winterplein.Application/MatchdayPlans/`: loads season, generates matches, returns null for unknown season, returns empty for < 4 players
- [ ] `GET /api/seasons/{id}/match-pool` endpoint on `SeasonsController` — 200 or 404
- [ ] `GetMatchPoolAsync(int seasonId)` added to `SeasonApiClient`
- [ ] Match pool section on season detail page: collapsible panel with total count and MudTable of all matches
- [ ] Unit tests: returns matches, null for unknown season, empty for < 4 players
- [ ] Integration tests: 200 with matches, 404 for unknown season

## Technical Notes

- Match pool is never persisted — computed on-the-fly from enrolled players each request
- Reuses existing `GenerateMatchesResponse` DTO
- Match IDs in the pool are deterministic (sequential) for a given player set — Story 2 uses these IDs for uniqueness tracking
