# ui-polish

## Problem Statement

The app has functional pages but lacks a polished layout, home page, loading feedback, and consistent theming. Navigation is incomplete and there is no cross-component state sharing.

## Proposed Solution

Finalize the MudBlazor layout (AppBar, collapsible NavDrawer), add a home page with app stats and quick actions, add loading/feedback indicators, and define a custom tennis-themed MudBlazor palette. Share player count via a lightweight `AppState` scoped service.

## Business Requirements

**Given** a user opens the app
**When** they view the home page
**Then** they see the current player count and match count with quick-action buttons to navigate to Players and Matches pages

## Acceptance Criteria

- [ ] `MainLayout.razor`: MudLayout with MudAppBar and collapsible MudDrawer (auto-collapses on mobile)
- [ ] `NavMenu.razor`: links to Home, Players, Matches
- [ ] `Home.razor` at `/`: app description, player/match count stats, "Manage Players" and "Generate Matches" buttons
- [ ] MudProgressLinear or MudSkeleton while loading; MudSnackbar on player added/removed; MudAlert for errors
- [ ] Custom MudBlazor theme with tennis/sport palette (green primary, amber secondary)
- [ ] `AppState` scoped service sharing player count across components

## Technical Notes

- `AppState` is a scoped service; components subscribe via `AppState.OnChange` and implement `IDisposable` to unsubscribe
- MudBlazor providers must be in `App.razor` or `MainLayout.razor`
- Theme defined in `WinterpleinTheme.cs` using `MudTheme` — no inline styles
