# player-management-ui

## Problem Statement

Players can be managed via the API but there is no user interface. Users need a browser-based page to add, view, and remove players.

## Proposed Solution

Build a Players page in Blazor WASM using MudBlazor components. A typed `PlayerApiClient` wraps the API calls. The page includes an add form with validation and a table listing all players with per-row delete buttons.

## Business Requirements

**Given** a user opens the Players page
**When** they fill in first name, last name, and gender and click "Add"
**Then** the player is added and the list refreshes immediately

**Given** a player exists in the list
**When** the user clicks the delete icon
**Then** the player is removed and the list refreshes

## Acceptance Criteria

- [ ] `PlayerApiClient` typed HttpClient in `Winterplein.Client/Services/` with `GetPlayersAsync`, `AddPlayerAsync`, `RemovePlayerAsync`
- [ ] `Players.razor` page at `/players`: MudTextField (First Name, Last Name), MudSelect (Gender), MudButton "Add"
- [ ] Form validation: empty/whitespace fields show inline error; Add button disabled until valid
- [ ] MudTable listing players with First Name, Last Name, Gender, Action (delete icon) columns
- [ ] Player count shown above table; list refreshes after add/remove
- [ ] Loading skeleton shown while fetching

## Technical Notes

- `PlayerApiClient` registered as typed HttpClient with `ApiBaseUrl` from `wwwroot/appsettings.json`
- Enter key on text fields triggers the add action
