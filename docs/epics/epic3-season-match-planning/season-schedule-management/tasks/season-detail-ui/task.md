# season-detail-ui

## Scope

Replace the Story-2 flat planned-matches table in `SeasonDetail.razor` with a matchday-by-matchday schedule view loaded from `GetScheduleAsync`. Keep the Generate Schedule button.

- On load (and after Generate / Clear / Clear All), call `SeasonApi.GetScheduleAsync(Id)` and bind a `SeasonScheduleResponse?` field; remove the `_schedule` `GenerateScheduleResponse` binding from the table.
- Schedule `MudTable` columns: Date, Match (formatted teams via existing `FormatTeam`, or "—" when open), Status chip (`Planned` / `Open` from `IsPlanned`), Action.
- Per-row Clear `MudIconButton` shown only when `entry.IsPlanned`; calls `SeasonApi.ClearPlannedMatchAsync(Id, entry.Date)` then reloads the schedule; success/error snackbar.
- "Clear All" button next to Generate Schedule; confirmation via `DialogService.ShowMessageBoxAsync` (style of `ConfirmRemovePlayer`), then `SeasonApi.ClearAllPlannedMatchesAsync(Id)`, reload, snackbar. Disable/hide when no planned entries.
- Generate Schedule keeps working; after generating, reload the schedule (so open slots flip to planned) instead of binding the generate response directly.
- Empty/no-matchday and loading states preserved (skeletons / "No matchdays" text).

## Domain model changes

None (UI binds to `SeasonScheduleResponse` / `MatchdayScheduleEntryDto`).

## Test cases

No automated UI tests in this project (no bUnit). Manual verification: load season → every matchday shown in date order with Planned/Open chip; Generate → open rows flip to Planned; per-row Clear → row flips to Open; Clear All (confirm) → all Open; Clear All when empty → no error. Covered indirectly by the api-and-client integration tests.

## Affected files

- modify: src/Winterplein.Client/Pages/SeasonDetail.razor
