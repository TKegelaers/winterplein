# schedule-blazor-ui

## Scope

Add a "Generate Schedule" action to the season detail page.

- On `SeasonDetail.razor`, add a "Generate Schedule" `MudButton` (e.g. near the Match Pool panel).
- On click, call `SeasonApi.GenerateScheduleAsync(Id)`; show snackbar feedback reflecting the result, e.g. "Planned {PlannedCount} matchdays, {OpenCount} still open." on success; error snackbar on failure.
- Optional: display the returned planned matches (date + both teams) in a `MudTable`, reusing the existing `FormatTeam` helper. Keep consistent with the existing Match Pool panel styling.
- Out of scope: clearing/removing planned matches (Story 3).

## Domain model changes

None.

## Test cases

No automated UI tests in this project (Blazor UI is exercised manually / future Playwright epic). Verify manually: button generates and reports counts; re-clicking reports 0 newly planned.

## Affected files

- modify: src/Winterplein.Client/Pages/SeasonDetail.razor
  </content>
