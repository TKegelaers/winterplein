# Story 4 — Build Season UI

**Epic:** Epic 2 — Season Management
**Dependencies:** Story 3

## Description

Build the season management UI in Blazor WASM using MudBlazor, including list, create, edit, and detail pages plus a reusable form component.

## Acceptance Criteria

### Client service (`src/Winterplein.Client/Services/SeasonApiClient.cs`)

- Typed `HttpClient` wrapper with methods:
  - `GetSeasonsAsync()` → `List<SeasonDto>`
  - `GetSeasonAsync(int id)` → `SeasonDto?`
  - `CreateSeasonAsync(CreateSeasonRequest)` → `SeasonDto`
  - `UpdateSeasonAsync(int id, UpdateSeasonRequest)` → `SeasonDto?`
  - `DeleteSeasonAsync(int id)` → `bool`
  - `GetMatchdaysAsync(int id)` → `List<DateOnly>`

### Pages

- **Season List** (`/seasons`) — `MudTable` with columns: Name, Weekday, Start Date, End Date, Time, Matchday Count, Actions (Edit/Delete/View)
- **Season Create** (`/seasons/create`) — uses `SeasonForm` component
- **Season Edit** (`/seasons/{id:int}/edit`) — loads existing season, uses `SeasonForm` component
- **Season Detail** (`/seasons/{id:int}`) — season summary + `MudTable` listing computed matchdays

### Shared component (`SeasonForm.razor`)

- Fields: `MudTextField` (Name), `MudDatePicker` (StartDate, EndDate), `MudSelect<DayOfWeek>` (Weekday), `MudTimePicker` (StartHour, EndHour)
- Client-side validation (Blazor `EditForm` with `DataAnnotationsValidator`)
- Preview panel showing computed matchday count and first/last matchday date

### Navigation

- "Seasons" link added to `NavMenu.razor`

## Technical Notes

- `SeasonApiClient` is registered as a typed `HttpClient` with base address pointing to the API
- Pages use `@inject SeasonApiClient` — no direct HttpClient usage in pages
- Delete uses a `MudDialog` confirmation before calling delete endpoint

## Tasks

- [x] T1: Implement `SeasonApiClient` with all API methods (using `int id` parameters) including `GetSeasonPlayersAsync(int id)`, `AddPlayerToSeasonAsync(int seasonId, AddSeasonPlayerRequest)`, `RemovePlayerFromSeasonAsync(int seasonId, int playerId)`
- [x] T2: Register `SeasonApiClient` as typed HttpClient in `Program.cs` (blockedBy: T1)
- [x] T3: Add "Seasons" nav link to `NavMenu.razor`
- [x] T4: Build season list page (`/seasons`) with `MudTable` and delete confirmation dialog (blockedBy: T1, T2)
- [x] T5: Build `SeasonForm.razor` reusable component with validation and matchday preview (blockedBy: T1)
- [x] T6: Build create page (`/seasons/create`) using `SeasonForm` (blockedBy: T4, T5)
- [x] T7: Build edit page (`/seasons/{id:int}/edit`) using `SeasonForm` (blockedBy: T4, T5)
- [x] T8: Build detail page (`/seasons/{id:int}`) with matchday list and player management section (blockedBy: T1, T2, T9)
- [x] T9: Add delete confirmation `MudDialog` to list page (blockedBy: T4)
- [x] T10: Add player management section to detail page — `MudTable` of enrolled players with remove button (with `MudDialog` confirmation); `MudSelect<PlayerDto>` populated from `/api/players` filtered to non-enrolled players with "Add" button (blockedBy: T1, T8)
