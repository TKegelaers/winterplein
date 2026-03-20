# Story 4 — Blazor WASM UI

**Epic:** Epic 2 — Season Management
**Dependencies:** Story 3

## Description

Build the season management UI in Blazor WASM using MudBlazor, including list, create, edit, and detail pages plus a reusable form component.

## Acceptance Criteria

### Client service (`src/Winterplein.Client/Services/SeasonApiClient.cs`)

- Typed `HttpClient` wrapper with methods:
  - `GetSeasonsAsync()` → `List<SeasonDto>`
  - `GetSeasonAsync(Guid id)` → `SeasonDto?`
  - `CreateSeasonAsync(CreateSeasonRequest)` → `SeasonDto`
  - `UpdateSeasonAsync(Guid id, UpdateSeasonRequest)` → `SeasonDto?`
  - `DeleteSeasonAsync(Guid id)` → `bool`
  - `GetMatchdaysAsync(Guid id)` → `List<DateOnly>`

### Pages

- **Season List** (`/seasons`) — `MudTable` with columns: Name, Weekday, Start Date, End Date, Time, Matchday Count, Actions (Edit/Delete/View)
- **Season Create** (`/seasons/create`) — uses `SeasonForm` component
- **Season Edit** (`/seasons/{id}/edit`) — loads existing season, uses `SeasonForm` component
- **Season Detail** (`/seasons/{id}`) — season summary + `MudTable` listing computed matchdays

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

- [ ] T1: Implement `SeasonApiClient` with all API methods
- [ ] T2: Register `SeasonApiClient` as typed HttpClient in `Program.cs` (blocks: T1)
- [ ] T3: Add "Seasons" nav link to `NavMenu.razor`
- [ ] T4: Build season list page (`/seasons`) with `MudTable` and delete confirmation dialog (blocks: T1, T2)
- [ ] T5: Build `SeasonForm.razor` reusable component with validation and matchday preview (blocks: T1)
- [ ] T6: Build create page (`/seasons/create`) using `SeasonForm` (blocks: T4, T5)
- [ ] T7: Build edit page (`/seasons/{id}/edit`) using `SeasonForm` (blocks: T4, T5)
- [ ] T8: Build detail page (`/seasons/{id}`) with matchday list (blocks: T1, T2)
- [ ] T9: Add delete confirmation `MudDialog` to list page (blocks: T4)
