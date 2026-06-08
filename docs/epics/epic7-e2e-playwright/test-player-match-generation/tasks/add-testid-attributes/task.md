# add-testid-attributes

## Scope

Add `data-testid` attributes to the Players and Matches Blazor pages so Playwright tests can target stable selectors independent of styling or component hierarchy.

Players page (`src/Winterplein.Client/Pages/Players.razor`):

- First Name `MudTextField` — `data-testid="player-firstname-input"` on the wrapping element or via `UserAttributes`
- Last Name `MudTextField` — `data-testid="player-lastname-input"`
- Add button `MudButton` — `data-testid="add-player-btn"`
- Player count `MudText` ("@\_players.Count players") — wrap in a `<span data-testid="player-count">`
- Delete `MudIconButton` per row — `data-testid="remove-player-btn"`

Matches page (`src/Winterplein.Client/Pages/Matches.razor`):

- Generate Matches `MudButton` — `data-testid="generate-matches-btn"`
- Each match row `MudText` inside `Virtualize` — wrap in `<div data-testid="match-list-item">`

Note: MudBlazor renders its own HTML; `data-testid` must be placed on an outer `<div>` wrapper where MudBlazor attributes do not natively pass through, or on the rendered input using `UserAttributes` where supported.

## Domain model changes

None.

## Test cases

None — this is a markup-only change. Correctness verified visually or by the E2E tests in the next task.

## Affected files

- modify: `src/Winterplein.Client/Pages/Players.razor`
- modify: `src/Winterplein.Client/Pages/Matches.razor`
