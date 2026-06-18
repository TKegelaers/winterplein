# season-schedule-generation

## Problem Statement

A season has computed matchdays but no mechanism to automatically assign a match to each one. Manually selecting a match per matchday would be impractical given the large pool size.

## Proposed Solution

Add a `GenerateScheduleCommand` that, for a season, assigns one randomly chosen match from the season's match pool to each **open** matchday (a matchday date that has no planned match yet). Each assignment is persisted as a `PlannedMatch` that stores a **frozen snapshot** of the match (the four players' names and genders as two teams), so it stays readable even if a player is later renamed or removed.

Matches are **unique across the whole season** — no match (same four players in the same team pairing) is assigned to more than one matchday. Generation is **idempotent**: re-running only fills matchdays that are still open and never disturbs already-planned ones. The endpoint returns the full planned schedule plus counts.

`PlannedMatch` is persisted with EF Core / SQL Server like the other entities (the in-memory repository pattern referenced in older docs no longer applies after the Epic 6 migration).

## Business Requirements

**Given** a season with at least 4 enrolled players and one or more open matchdays
**When** `POST /api/seasons/{id}/schedule/generate` is called
**Then** each open matchday receives one unique match from the pool, already-planned matchdays are untouched, and a `GenerateScheduleResponse` is returned

**Given** the pool has fewer remaining (unused) matches than open matchdays
**When** schedule generation runs
**Then** as many matchdays as possible are filled and the response's `OpenCount` reflects the unfilled remainder

**Given** a season with fewer than 4 enrolled players (empty pool)
**When** schedule generation runs
**Then** nothing is planned and the response reports all matchdays as open (HTTP 200)

**Given** a season id that does not exist
**When** the endpoint is called
**Then** HTTP 404 is returned

## Acceptance Criteria

- [ ] `PlannedMatch` domain entity (EF-compatible: private parameterless ctor, private setters): `Id`, `SeasonId`, `Date` (matchday), and a frozen match snapshot (two teams, each two players' name + gender); constructor validation rejects a default/empty date
- [ ] `PlannedMatchDto` and `GenerateScheduleResponse` (planned matches + `PlannedCount` + `OpenCount`) in `Winterplein.Application.IO/DTOs/`; `GenerateScheduleCommand(int SeasonId)` in `Winterplein.Application.IO/Commands/`
- [ ] `IPlannedMatchRepository` (async, in `Application/Ports/`): at least `GetAllBySeasonAsync(int seasonId)` and `AddAsync`/add-range; EF implementation `EfPlannedMatchRepository` in `Infrastructure/Repositories/`, registered Scoped
- [ ] EF Core wiring: `DbSet<PlannedMatch>` on `WinterpleinDbContext`, `PlannedMatchConfiguration` in `Infrastructure/Configurations/`, and a new migration
- [ ] `GenerateScheduleCommandHandler` (Wolverine static `Handle`): loads season, computes open matchdays from `GetMatchdays()` minus already-planned dates, derives the pool, excludes matches already planned this season, randomly assigns one unique match per open matchday, persists, returns `GenerateScheduleResponse`
- [ ] `POST /api/seasons/{id}/schedule/generate` on `SeasonsController` — 200 with response or 404; `GenerateScheduleAsync(int seasonId)` on `SeasonApiClient`
- [ ] "Generate Schedule" button on the season detail page with snackbar feedback reflecting planned/open counts
- [ ] `PlannedMatchBuilder` in `Common.UnitTests/Builders/`; unit tests (fills open matchdays, skips already-planned, partial fill when pool too small, season-wide uniqueness, empty pool) and integration tests (200 with planned matches, idempotent re-run, 404 unknown season)

## Testing Plan

- Unit: handler fills every open matchday with a unique match; a second run with some matchdays already planned only fills the rest and changes nothing else; pool smaller than open matchdays → partial fill with correct `OpenCount`; < 4 players → empty plan; randomness verified by structural invariants (uniqueness, count), not specific outputs
- Integration (real SQL Server + Respawn): POST returns 200 with persisted planned matches; re-running POST is idempotent; 404 for unknown season

## Potential Pitfalls

- Pool match IDs are transient and sequential (recomputed each request, dependent on the current player set) — do **not** persist them as identity. Track season-wide uniqueness by the match's player composition (the four player ids + pairing) compared against existing `PlannedMatch` snapshots, so a changed roster between runs cannot cause false collisions or duplicates.
- The snapshot is intentionally denormalized (no FK to `Player`) so a later rename/removal does not alter or break historical planned matches.
- Randomization uses a thread-safe shared RNG; tests assert invariants, not exact selections.
- "Open matchday" is derived: `Season.GetMatchdays()` minus the dates already present in `PlannedMatch` for that season.
- Clearing/removing planned matches is out of scope here — it belongs to Story 3 (Browse & Manage Schedule).
