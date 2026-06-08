# project-setup

## Problem Statement

There is no solution or project structure. The Winterplein application needs a full Clean Architecture scaffold with a Blazor WASM client, ASP.NET Core API, domain/application/infrastructure layers, shared DTOs, and test projects — all wired together with correct project references.

## Proposed Solution

Scaffold all projects using `dotnet new`, add them to a single solution, configure Clean Architecture project references, install MudBlazor in the client, configure CORS in the API, and scaffold a basic layout with AppBar and NavDrawer.

## Business Requirements

**Given** a developer clones the repository
**When** they run `dotnet build`
**Then** all projects compile with no errors

## Acceptance Criteria

- [ ] `Winterplein.sln` at repo root containing all 9 projects
- [ ] Clean Architecture project reference rules enforced (Domain has no refs; Client only refs Shared)
- [ ] MudBlazor installed and configured (providers, CSS/JS) in `Winterplein.Client`
- [ ] CORS configured in API to allow Blazor client origin
- [ ] Basic layout with AppBar ("Winterplein") and NavDrawer with placeholder links (Home, Players, Matches)
- [ ] `dotnet build` succeeds with no errors

## Technical Notes

- 9 projects: Client (Blazor WASM), Api (ASP.NET Core), Domain, Application, Infrastructure, Shared (DTOs), UnitTests, IntegrationTests, UnitTests.Common
- `Winterplein.Client` references only `Winterplein.Shared` — no domain or application coupling
- MudBlazor providers (`MudThemeProvider`, `MudPopoverProvider`, `MudDialogProvider`, `MudSnackbarProvider`) in `App.razor` or `MainLayout.razor`
