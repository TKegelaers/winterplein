# Story 1 — Project Setup

**Epic:** Epic 1 — Match Generation
**Dependencies:** None

## Description

Scaffold the full solution with all projects following Clean Architecture. Configure project references, install MudBlazor in the Blazor WASM client, and set up CORS in the API so the client can communicate with it.

## Acceptance Criteria

- `Winterplein.sln` exists at repo root containing all 9 projects
- Project structure:
  ```
  src/
    Winterplein.Client/         ← Blazor WASM, targets net10.0
    Winterplein.Api/            ← ASP.NET Core minimal API, targets net10.0
    Winterplein.Domain/         ← Class library, no external dependencies
    Winterplein.Application/    ← Class library, references Domain
    Winterplein.Infrastructure/ ← Class library, references Application + Domain
    Winterplein.Shared/         ← Class library, no external dependencies (pure DTOs)
  tests/
    Winterplein.UnitTests/      ← xUnit, references Application + Domain
    Winterplein.IntegrationTests/ ← xUnit, references Api
  ```
- Project references follow Clean Architecture (Domain has no refs, Client only refs Shared)
- MudBlazor NuGet package installed in `Winterplein.Client`
- MudBlazor configured: providers in layout, CSS/JS linked in `index.html`
- API has CORS configured to allow requests from the Blazor client origin
- App bar with title "Winterplein" and nav drawer with placeholder links: Home, Players, Matches
- `dotnet build` succeeds with no errors

## Technical Notes

```bash
dotnet new sln -n Winterplein
dotnet new blazorwasm -n Winterplein.Client -o src/Winterplein.Client -f net10.0 --empty
dotnet new webapi -n Winterplein.Api -o src/Winterplein.Api -f net10.0 --use-minimal-apis
dotnet new classlib -n Winterplein.Domain -o src/Winterplein.Domain -f net10.0
dotnet new classlib -n Winterplein.Application -o src/Winterplein.Application -f net10.0
dotnet new classlib -n Winterplein.Infrastructure -o src/Winterplein.Infrastructure -f net10.0
dotnet new classlib -n Winterplein.Shared -o src/Winterplein.Shared -f net10.0
dotnet new xunit -n Winterplein.UnitTests -o tests/Winterplein.UnitTests -f net10.0
dotnet new xunit -n Winterplein.IntegrationTests -o tests/Winterplein.IntegrationTests -f net10.0

# Add to solution
dotnet sln add src/Winterplein.Client src/Winterplein.Api src/Winterplein.Domain \
  src/Winterplein.Application src/Winterplein.Infrastructure src/Winterplein.Shared \
  tests/Winterplein.UnitTests tests/Winterplein.IntegrationTests

# Project references
dotnet add src/Winterplein.Application reference src/Winterplein.Domain
dotnet add src/Winterplein.Infrastructure reference src/Winterplein.Application src/Winterplein.Domain
dotnet add src/Winterplein.Api reference src/Winterplein.Application src/Winterplein.Infrastructure src/Winterplein.Shared
dotnet add src/Winterplein.Client reference src/Winterplein.Shared
dotnet add tests/Winterplein.UnitTests reference src/Winterplein.Application src/Winterplein.Domain
dotnet add tests/Winterplein.IntegrationTests reference src/Winterplein.Api

# MudBlazor
dotnet add src/Winterplein.Client package MudBlazor
```

## Tasks

- [ ] T1: Create solution file (`dotnet new sln -n Winterplein`)
- [ ] T2: Scaffold all 8 projects (blocks: T1)
- [ ] T3: Add all projects to solution (blocks: T2)
- [ ] T4: Set up project references per Clean Architecture (blocks: T3)
- [ ] T5: Install MudBlazor NuGet package in Client (blocks: T3)
- [ ] T6: Configure CORS in API to allow Client origin (blocks: T3)
- [ ] T7: Add MudBlazor providers and CSS/JS to Client (blocks: T5)
- [ ] T8: Scaffold basic layout with AppBar and NavDrawer placeholder links (blocks: T7)
- [ ] T9: Verify `dotnet build` passes with no errors (blocks: T4, T6, T8)
