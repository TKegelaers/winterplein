# match-display-ui

## Problem Statement

Match generation works via the API but there is no UI to trigger it or browse the results. With up to 630 matches, naive rendering would be slow.

## Proposed Solution

Build a Matches page in Blazor WASM. A typed `MatchApiClient` wraps the API calls. Results render in a `<Virtualize>` component for smooth scrolling of large result sets.

## Business Requirements

**Given** at least 4 players are added
**When** the user clicks "Generate Matches"
**Then** all possible doubles matches are displayed in a virtualized scrollable list

## Acceptance Criteria

- [ ] `MatchApiClient` typed HttpClient with `GenerateMatchesAsync` and `GetMatchCountAsync`
- [ ] `Matches.razor` page at `/matches`: shows player count and expected match count before generating
- [ ] "Generate Matches" button: disabled with message when < 4 players; shows spinner while generating
- [ ] After generation: MudAlert with count; results in `<Virtualize>` component
- [ ] Each row: `Match #N: Player1 & Player2 vs Player3 & Player4`
- [ ] Regenerating clears previous results first

## Technical Notes

- Use `<Virtualize Items="matches" Context="match">` — required for 630+ rows without lag
- `GenerateMatchesResponse.Matches` is a flat `List<MatchDto>`
