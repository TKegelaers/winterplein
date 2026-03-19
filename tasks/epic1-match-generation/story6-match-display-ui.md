# Story 6 — Match Display UI

**Epic:** Epic 1 — Match Generation
**Dependencies:** Story 5 (player management UI)

## Description

Build the Matches page where users can generate all possible doubles matches from the current player list and browse the results.

## Acceptance Criteria

### HTTP client (`Winterplein.Client/Services/MatchApiClient.cs`)

- Typed `HttpClient` wrapping `/api/matches/*` calls:
  - `GenerateMatchesAsync()` → `GenerateMatchesResponse`
  - `GetMatchCountAsync()` → `int`

### Matches page (`Winterplein.Client/Pages/Matches.razor`, route `/matches`)

- Shows current player count fetched from `PlayerApiClient`
- Shows expected match count (from `GET /api/matches/count`) before generating
- "Generate Matches" button:
  - Disabled with explanatory message when fewer than 4 players
  - Shows spinner while generating
- After generation:
  - MudAlert: "Generated 630 matches" (or actual count)
  - Results displayed in a virtualized scrollable list using Blazor's `<Virtualize>` component
  - Each row: `Match #N: Player1 & Player2 vs Player3 & Player4`
- Regenerating clears previous results first

## Technical Notes

- Use `<Virtualize Items="matches" Context="match">` for smooth rendering of 630+ rows
- `GenerateMatchesResponse.Matches` is a flat `List<MatchDto>` — no nested objects to unwrap
- Format display name: `$"{m.Team1Player1} & {m.Team1Player2} vs {m.Team2Player1} & {m.Team2Player2}"`
