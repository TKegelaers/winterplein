# add-absence-data-testids

## Scope

Add `data-testid` attributes to the player absence dialog and its trigger so Playwright can open the dialog, interact with matchday checkboxes, and submit absences.

## Domain model changes

None.

## Test cases

No automated tests for this task. Correctness verified by the E2E tests in task T4 consuming these selectors.

## Affected files

- modify: `src/Winterplein.Client/Pages/SeasonDetail.razor` — add `data-testid="absence-btn-{playerId}"` on the per-player absence trigger button, `data-testid="absence-dialog"` on the MudDialog root, `data-testid="absence-checkbox-{date}"` on each matchday checkbox inside the dialog, `data-testid="absence-save-btn"` on the Save button
