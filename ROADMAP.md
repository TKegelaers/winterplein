# Winterplein — Roadmap

Winterplein is a tennis doubles match generator. Given a list of players, it automatically generates all possible doubles (2v2) matches using combinatorics: C(N,4) groups × 3 unique team pairings = total matches (e.g. 10 players → 630 matches).

**Tech stack:** .NET 10 · Blazor WebAssembly · ASP.NET Core Web API · MudBlazor · Clean Architecture · Wolverine

---

## Epic 1 — Match Generation

> Generate all possible doubles matches from a player list.

| #   | Story                                                                                         | Description                                           | Status |
| --- | --------------------------------------------------------------------------------------------- | ----------------------------------------------------- | ------ |
| 1   | [Set Up Solution](.tasks/epic1-match-generation/story1-project-setup.md)                      | Scaffold solution with all projects, MudBlazor, CORS  | Done   |
| 2   | [Define Domain Models](.tasks/epic1-match-generation/story2-domain-models.md)                 | Player, Team, Match entities + Shared DTOs            | Done   |
| 3   | [Implement Match Generator](.tasks/epic1-match-generation/story3-match-generation-service.md) | C(N,4)×3 algorithm, unit tests                        | Done   |
| 4   | [Build Player & Match API](.tasks/epic1-match-generation/story4-api-endpoints.md)             | Controllers, CQRS/MediatR, Swagger, integration tests | Done   |
| 5   | [Build Player UI](.tasks/epic1-match-generation/story5-player-management-ui.md)               | Add/remove players in Blazor WASM                     | Done   |
| 6   | [Build Match Display](.tasks/epic1-match-generation/story6-match-display-ui.md)               | Generate and display all matches                      | Done   |
| 7   | [Polish UI](.tasks/epic1-match-generation/story7-ui-polish.md)                                | Layout, home page, responsive design                  | Done   |

---

---

## Epic 2 — Season Management

> Manage seasons with a name, date range, weekly matchday, and start/end hours. Matchdays are computed from the date range and weekday — not stored.

| #   | Story                                                                                              | Description                                                                    | Status  |
| --- | -------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------ | ------- |
| 1   | [Define Season Domain & DTOs](.tasks/epic2-season-management/story1-domain-and-dtos.md)            | `Season` entity with `GetMatchdays()`, `SeasonDto`, Create/Update request DTOs | Pending |
| 2   | [Implement Season CQRS & Repository](.tasks/epic2-season-management/story2-cqrs-and-repository.md) | MediatR commands/queries, `ISeasonRepository`, `InMemorySeasonRepository`      | Pending |
| 3   | [Build Season API](.tasks/epic2-season-management/story3-api-endpoints.md)                         | CRUD + matchdays endpoints, `SeasonMapper`, DI registration                    | Pending |
| 4   | [Build Season UI](.tasks/epic2-season-management/story4-blazor-ui.md)                              | `SeasonApiClient`, list/create/edit/detail pages, `SeasonForm` component       | Pending |
| 5   | [Write Season Tests](.tasks/epic2-season-management/story5-tests.md)                               | Unit tests for domain + handlers, integration tests for API endpoints          | Pending |

---

## Epic 3 — Season Match Planning

> Generate and schedule matches for a season's matchdays using enrolled players.

| #   | Story                                                                                               | Description                                                              | Status  |
| --- | --------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------ | ------- |
| 1   | [View Season Match Pool](.tasks/epic3-season-match-planning/story1-view-season-match-pool.md)       | View all possible matches generated from a season's enrolled players     | Pending |
| 2   | [Generate Season Schedule](.tasks/epic3-season-match-planning/story2-plan-match-for-matchday.md)    | Automatically assign random unique matches to all open matchdays at once | Pending |
| 3   | [Browse & Manage Schedule](.tasks/epic3-season-match-planning/story3-browse-and-manage-schedule.md) | View full matchday schedule, clear individual or all planned matches     | Pending |

---

## Epic 4 — Player Absence Management

> Record player absences per matchday so the automatic schedule generator only picks matches where all four players are available.

| #   | Story                                                                                                                   | Description                                                                                | Status  |
| --- | ----------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------ | ------- |
| 1   | [Record Player Absences](.tasks/epic4-player-absence-management/story1-record-player-absences.md)                       | Mark/unmark players as absent on specific matchdays, with domain, API, and UI              | Pending |
| 2   | [Absence-Aware Schedule Generation](.tasks/epic4-player-absence-management/story2-absence-aware-schedule-generation.md) | Filter the match pool per matchday to exclude matches with absent players before assigning | Pending |

---

## Epic 5 — Migrate from MediatR to Wolverine

> Replace MediatR with Wolverine as the mediator/message bus, adopting convention-based handlers while preserving the CQRS architecture.

| #   | Story                                                                                                                               | Description                                                                             | Status |
| --- | ----------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- | ------ |
| 1   | [Replace MediatR with Wolverine Using Shims](.tasks/epic5-wolverine-migration/story1-replace-mediatr-with-wolverine-shims.md)       | Swap NuGet package, DI, controllers, and using directives via Wolverine's MediatR shims | Done   |
| 2   | [Convert Handlers to Wolverine Native Conventions](.tasks/epic5-wolverine-migration/story2-convert-handlers-to-wolverine-native.md) | Remove shims, convert to static Handle methods with method injection                    | Done   |

---

## Epic 6 — SQL Server Persistence with EF Core

> Replace in-memory repositories with SQL Server persistence using Entity Framework Core, preserving the Clean Architecture repository pattern.

| #   | Story                                                                                                                                     | Description                                                                             | Status  |
| --- | ----------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- | ------- |
| 1   | [Make Domain Entities EF Core Compatible](.tasks/epic6-sql-server-ef-core/story1-make-domain-entities-ef-core-compatible.md)              | Add private parameterless constructors and private set accessors to all domain entities | Pending |
| 2   | [Convert Repositories to Async](.tasks/epic6-sql-server-ef-core/story2-convert-repositories-to-async.md)                                  | Make IPlayerRepository and ISeasonRepository fully async, update handlers               | Pending |
| 3   | [Add DbContext, Configurations, and EF Repos](.tasks/epic6-sql-server-ef-core/story3-add-dbcontext-configurations-and-ef-repositories.md) | EF Core setup, entity configurations, SQL Server repositories, DI wiring, migration     | Pending |
| 4   | [Update Test Infrastructure for EF Core](.tasks/epic6-sql-server-ef-core/story4-update-test-infrastructure-for-ef-core.md)                | Replace in-memory repo DI swaps with SQLite in-memory DbContext in integration tests    | Pending |

---

## Epic 7 — E2E Tests with Playwright

> Validate end-to-end user flows across the full stack using Playwright, running against the live Blazor WASM + API dev stack.

| #   | Story                                                                                                    | Description                                                                  | Status  |
| --- | -------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------- | ------- |
| 1   | [Scaffold Playwright Test Project](.tasks/epic7-e2e-playwright/story1-scaffold-playwright-project.md)    | Create test project, configure app startup, base URL, and CI task            | Pending |
| 2   | [Test Player & Match Generation](.tasks/epic7-e2e-playwright/story2-test-player-and-match-generation.md) | E2E tests for the add-players → generate-matches → view-results flow         | Pending |
| 3   | [Test Season Management](.tasks/epic7-e2e-playwright/story3-test-season-management.md)                   | E2E tests for season CRUD, matchday listing, and player enrollment           | Pending |
| 4   | [Test Match Schedule Planning](.tasks/epic7-e2e-playwright/story4-test-match-schedule-planning.md)       | E2E tests for schedule generation, absence management, and schedule browsing | Pending |

---

## Future Epics

| Epic | Description |
| ---- | ----------- |
