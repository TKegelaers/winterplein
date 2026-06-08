# add-season-data-testids

## Scope

Add `data-testid` attributes to all season-related Blazor pages and the shared SeasonForm component so Playwright selectors are stable across UI restyling.

Attributes to add:

**Seasons.razor (list page)**

- `data-testid="new-season-btn"` on the New Season button
- `data-testid="season-table"` on the MudTable
- `data-testid="season-row"` on each MudTableRow (RowTemplate)
- `data-testid="season-name"` on the Name MudTd
- `data-testid="season-matchday-count"` on the Matchdays MudTd
- `data-testid="season-view-btn"` on the View icon button
- `data-testid="season-edit-btn"` on the Edit icon button
- `data-testid="season-delete-btn"` on the Delete icon button

**SeasonForm.razor**

- `data-testid="season-name-input"` on the Name MudTextField
- `data-testid="season-start-date"` on the Start Date MudDatePicker
- `data-testid="season-end-date"` on the End Date MudDatePicker
- `data-testid="season-weekday-select"` on the Weekday MudSelect
- `data-testid="season-start-hour"` on the Start Hour MudTimePicker
- `data-testid="season-end-hour"` on the End Hour MudTimePicker
- `data-testid="season-preview-matchday-count"` on the Matchdays preview text
- `data-testid="season-submit-btn"` on the submit MudButton

**SeasonDetail.razor**

- `data-testid="season-detail-name"` on the season name MudText heading
- `data-testid="season-matchday-table"` on the matchdays MudTable
- `data-testid="season-matchday-row"` on each matchday RowTemplate
- `data-testid="season-player-table"` on the enrolled players MudTable
- `data-testid="season-player-row"` on each player RowTemplate
- `data-testid="season-add-player-select"` on the Add player MudSelect
- `data-testid="season-add-player-btn"` on the Add MudButton
- `data-testid="season-remove-player-btn"` on each remove MudIconButton

## Domain model changes

None.

## Test cases

None — verified visually by inspecting rendered HTML in the browser DevTools.

## Affected files

- modify: `src/Winterplein.Client/Pages/Seasons.razor`
- modify: `src/Winterplein.Client/Pages/SeasonForm.razor`
- modify: `src/Winterplein.Client/Pages/SeasonDetail.razor`
