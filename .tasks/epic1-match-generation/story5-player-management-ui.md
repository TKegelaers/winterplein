# Story 5 — Build Player UI

**Epic:** Epic 1 — Match Generation
**Dependencies:** Story 4 (API endpoints)

## Description

Build the Players page in Blazor WASM where users can add players by first name, last name and gender, view the current list, and remove individual players. All state is backed by the API.

## Acceptance Criteria

### HTTP client (`Winterplein.Client/Services/PlayerApiClient.cs`)

- Typed `HttpClient` wrapping `/api/players` calls:
  - `GetPlayersAsync()` → `List<PlayerDto>`
  - `AddPlayerAsync(AddPlayerRequest request)` → `PlayerDto`
  - `RemovePlayerAsync(int id)` → `bool`
- Registered in `Program.cs` with base address pointing to the API

### Players page (`Winterplein.Client/Pages/Players.razor`, route `/players`)

- Add form with:
  - MudTextField for **First Name**
  - MudTextField for **Last Name**
  - MudSelect for **Gender** (options: `Male`, `Female` from `GenderDto`)
  - MudButton "Add" (also triggers on Enter key in text fields)
- Validation: empty/whitespace first name or last name shows inline error; Add button stays disabled until form is valid
- MudTable listing all players with columns: **First Name**, **Last Name**, **Gender**, **Action** (delete icon button)
- Player count shown above the table (e.g. "10 players")
- Adding or removing a player refreshes the list immediately
- Loading skeleton shown while fetching

## Technical Notes

- Register `PlayerApiClient` in `Program.cs`:
  ```csharp
  builder.Services.AddHttpClient<PlayerApiClient>(client =>
      client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]!));
  ```
- Set `ApiBaseUrl` in `wwwroot/appsettings.json` (e.g. `http://localhost:5095`)
- Use `@inject PlayerApiClient PlayerApi` in the component
- `AddPlayerRequest` is from `Winterplein.Shared.DTOs` and requires `FirstName`, `LastName`, and `GenderDto Gender`
- `PlayerDto` has `int Id`, `string FirstName`, `string LastName`, `string Gender`
- `OnKeyDown` on text fields: call add on `Enter` key

## Tasks

- [x] T1: Implement `PlayerApiClient` with `GetPlayersAsync`, `AddPlayerAsync(AddPlayerRequest)`, and `RemovePlayerAsync(int id)`
- [x] T2: Register `PlayerApiClient` in `Program.cs` and set `ApiBaseUrl` in `appsettings.json` (blockedBy: T1)
- [x] T3: Build `Players.razor` page with add form (First Name + Last Name MudTextFields, Gender MudSelect, MudButton) (blockedBy: T1, T2)
- [x] T4: Add player table (MudTable) with columns First Name, Last Name, Gender, and per-row delete button (blockedBy: T3)
- [x] T5: Add form validation (blank name fields), Enter-key support on text fields, and loading skeleton (blockedBy: T3, T4)
