# create-season-management-tests

## Scope

Create `tests/Winterplein.E2eTests/SeasonManagementTests.cs` with the `[Collection("Playwright")]` attribute, extending `PageTest`. All seven tests use a GUID suffix on season names to avoid inter-test collisions.

Tests to implement:

1. **CreateSeason_AppearsInList** — navigate to `/seasons/create`, fill the form (name, start date, end date, weekday, start/end hour), submit, assert the season name appears in the season table on `/seasons`.

2. **CreateSeason_ComputedMatchdayCount_IsCorrect** — fill the form with a 4-Tuesday span (e.g. 2025-10-07 to 2025-10-28, weekday Tuesday), assert the preview shows matchday count 4, submit, assert the Matchdays cell in the list shows 4.

3. **EditSeason_UpdatesName** — create a season, navigate to its edit page, change the name, save, assert the updated name appears in the list.

4. **SeasonDetail_ShowsMatchdays** — create a season with a known date range, navigate to its detail page, assert the matchday table row count equals the expected matchday count.

5. **EnrollPlayer_AppearsInSeasonPlayerList** — call `SeasonTestHelpers.EnsurePlayerExistsAsync` to add a player, create a season, navigate to the detail page, select the player from the add-player dropdown, click Add, assert the player row appears in the enrolled players table.

6. **RemovePlayer_DisappearsFromSeasonPlayerList** — enroll a player (as above), click the remove button on that player row, confirm the dialog, assert the player row is no longer in the table.

7. **DeleteSeason_RemovedFromList** — create a season, return to `/seasons`, click the delete button for that season, confirm the dialog, assert the season name no longer appears in the table.

Implementation notes:

- Use `Page.WaitForSelectorAsync` after navigation and after mutations before asserting.
- MudBlazor date pickers: use `Page.FillAsync` with ISO date string (e.g. `"2025-10-07"`).
- MudBlazor time pickers: use `Page.FillAsync` with `"HH:mm"` format.
- MudBlazor select (weekday / add-player): click the select to open, then click the desired option by text.
- Delete/remove confirmation dialogs render in a MudBlazor portal — wait for the dialog button selector before clicking.

## Domain model changes

None.

## Test cases

The seven tests listed above are themselves the test cases.

## Affected files

- create: `tests/Winterplein.E2eTests/SeasonManagementTests.cs`
