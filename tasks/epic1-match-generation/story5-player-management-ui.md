# Story 5 — Player Management UI

**Epic:** Epic 1 — Match Generation
**Dependencies:** Story 4 (API endpoints)

## Description

Build the Players page in Blazor WASM where users can add players by name, view the current list, and remove individual players. All state is backed by the API.

## Acceptance Criteria

### HTTP client (`Winterplein.Client/Services/PlayerApiClient.cs`)

- Typed `HttpClient` wrapping `/api/players` calls:
  - `GetPlayersAsync()` → `List<PlayerDto>`
  - `AddPlayerAsync(string name)` → `PlayerDto`
  - `RemovePlayerAsync(Guid id)` → `bool`
- Registered in `Program.cs` with base address pointing to the API

### Players page (`Winterplein.Client/Pages/Players.razor`, route `/players`)

- MudTextField for player name + MudButton "Add" (also triggers on Enter key)
- Validation: empty/whitespace name shows inline error, button stays disabled
- MudTable listing all players with columns: Name, Action (delete icon button)
- Player count shown above the table (e.g. "10 players")
- Adding or removing a player refreshes the list immediately
- Loading skeleton shown while fetching

## Technical Notes

- Register `PlayerApiClient` in `Program.cs`:
  ```csharp
  builder.Services.AddHttpClient<PlayerApiClient>(client =>
      client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!));
  ```
- Set `ApiBaseUrl` in `wwwroot/appsettings.json` (e.g. `http://localhost:5000`)
- Use `@inject PlayerApiClient PlayerApi` in the component
- `OnKeyDown` on the text field: call add on `Enter` key

## Tasks

- [ ] T1: Implement `PlayerApiClient` typed HttpClient service
- [ ] T2: Register `PlayerApiClient` in `Program.cs` and set `ApiBaseUrl` in `appsettings.json` (blocks: T1)
- [ ] T3: Build `Players.razor` page with add form (MudTextField + MudButton) (blocks: T1, T2)
- [ ] T4: Add player table (MudTable) with per-row delete button (blocks: T3)
- [ ] T5: Add form validation, Enter-key support, and loading skeleton (blocks: T3, T4)
