# Story 7 — Polish UI

**Epic:** Epic 1 — Match Generation
**Dependencies:** Story 6 (match display UI)

## Description

Finalize the MudBlazor layout and navigation, add a home page with app overview, and ensure the app is responsive and polished.

## Acceptance Criteria

### Layout (`Winterplein.Client/Layout/`)

- `MainLayout.razor`: MudLayout with MudAppBar ("Winterplein") and collapsible MudDrawer
- `NavMenu.razor`: MudNavMenu with links to Home (`/`), Players (`/players`), Matches (`/matches`)
- Drawer collapses automatically on mobile (narrow screen)
- Player count badge visible in nav or app bar (live count)

### Home page (`Winterplein.Client/Pages/Home.razor`, route `/`)

- Short description of the app
- Current stats: player count, and match count if already generated
- Quick-action buttons: "Manage Players" → `/players`, "Generate Matches" → `/matches`

### Loading & feedback

- MudProgressLinear or MudSkeleton shown while loading data
- MudSnackbar confirmation on successful actions (player added, player removed)
- MudAlert for error states (API unreachable, validation failures)

### Theme

- Custom MudBlazor theme: primary color fitting a tennis/sport aesthetic
- Consistent typography and spacing throughout

## Technical Notes

- Share player count via a lightweight `AppState` service (scoped) injected into both layout and pages
- MudBlazor providers (`MudThemeProvider`, `MudPopoverProvider`, `MudDialogProvider`, `MudSnackbarProvider`) must be in `App.razor` or `MainLayout.razor`
- Use `MudTheme` to define custom palette — avoid inline styles

## Tasks

- [ ] T1: Create `AppState` scoped service and register in `Program.cs`
- [ ] T2: Update `MainLayout.razor` with MudAppBar, collapsible MudDrawer, and NavMenu (blockedBy: T1)
- [ ] T3: Build `Home.razor` page with app description, stats, and quick-action buttons (blockedBy: T1)
- [ ] T4: Add MudSnackbar notifications for player added/removed actions (blockedBy: T2)
- [ ] T5: Add MudProgressLinear/MudSkeleton loading indicators across pages (blockedBy: T2)
- [ ] T6: Configure custom MudBlazor theme with tennis/sport palette (blockedBy: T2)
