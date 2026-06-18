# add-match-pool-ui

## Scope

Show the match pool on the season detail page.

- In `SeasonDetail.razor`, add a collapsible section (e.g. `MudExpansionPanels` / `MudExpansionPanel`) titled "Match Pool".
- Lazily fetch the pool via `SeasonApi.GetMatchPoolAsync(Id)` when the panel is first expanded (avoid fetching the potentially large pool on page load); cache the result in a field.
- Show the total count (`TotalCount`).
- Render the matches in a **paged** `MudTable` (`Items` = `Matches`, built-in pager) with columns for match number/Id, Team 1, Team 2. Format a team as its two players' names.
- When the response is empty (`Matches.Count == 0` / `TotalCount == 0`), show a "Not enough players — a season needs at least 4 enrolled players to generate matches." message instead of the table.
- Reuse the existing loading/snackbar error patterns already in the component.

## Domain model changes

None. Consumes `GenerateMatchesResponse` / `MatchDto` / `TeamDto` / `PlayerDto`.

## Test cases

None (UI-only; no bUnit harness exists in this solution). Verify manually that the panel loads, pages, shows the count, and shows the empty-state message for a season with fewer than 4 players. E2E coverage is owned by Epic 7.

## Affected files

- modify: src/Winterplein.Client/Pages/SeasonDetail.razor
