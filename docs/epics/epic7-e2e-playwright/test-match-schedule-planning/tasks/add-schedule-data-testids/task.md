# add-schedule-data-testids

## Scope

Add `data-testid` attributes to the season detail page and schedule-related UI elements so Playwright tests can target stable selectors. These cover the schedule section, matchday status chips, generate/clear buttons.

## Domain model changes

None.

## Test cases

No automated tests for this task. Correctness verified by the E2E tests in tasks T3 and T4 consuming these selectors.

## Affected files

- modify: `src/Winterplein.Client/Pages/SeasonDetail.razor` — add `data-testid="schedule-section"` on the schedule container, `data-testid="schedule-entry-status"` on each status chip, `data-testid="generate-schedule-btn"` on the generate button, `data-testid="clear-match-btn"` on per-matchday clear button, `data-testid="clear-all-btn"` on the clear-all button
