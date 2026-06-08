# season-blazor-ui

## Problem Statement

Season management works via the API but has no UI. Users need pages to list, create, edit, and view seasons, plus a reusable season form component and player enrollment management.

## Proposed Solution

Build season list, create, edit, and detail pages in Blazor WASM using MudBlazor. A typed `SeasonApiClient` wraps all API calls. A reusable `SeasonForm.razor` component handles both create and edit flows.

## Business Requirements

**Given** a user opens the Seasons list page
**When** they click "Create"
**Then** they see a form with all season fields, a matchday count preview, and a submit button

**Given** a season detail page is open
**When** a user adds a player to the season
**Then** the enrolled player list refreshes immediately

## Acceptance Criteria

- [ ] `SeasonApiClient` typed HttpClient with all CRUD + matchday + player-season methods
- [ ] Season List page (`/seasons`): MudTable with Name, Weekday, Dates, Time, Matchday Count, Edit/Delete/View actions
- [ ] Season Create (`/seasons/create`) and Edit (`/seasons/{id}/edit`) pages using `SeasonForm`
- [ ] Season Detail (`/seasons/{id}`): summary + matchday list + player management section (add/remove enrolled players)
- [ ] `SeasonForm.razor`: MudTextField (Name), MudDatePicker (StartDate, EndDate), MudSelect (Weekday), MudTimePicker (StartHour, EndHour); preview panel with computed matchday count; client-side validation
- [ ] Delete confirmation `MudDialog`; "Seasons" nav link added

## Technical Notes

- `SeasonApiClient` registered as typed HttpClient with base address from config
- Delete uses `MudDialog` confirmation before calling the delete endpoint
- Pages use `@inject SeasonApiClient` — no direct `HttpClient` in pages
