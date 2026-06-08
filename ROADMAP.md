# Winterplein — Roadmap

Winterplein is a tennis doubles match generator. Given a list of players, it automatically generates all possible doubles (2v2) matches using combinatorics: C(N,4) groups × 3 unique team pairings = total matches (e.g. 10 players → 630 matches).

**Tech stack:** .NET 10 · Blazor WebAssembly · ASP.NET Core Web API · MudBlazor · Clean Architecture · Wolverine

---

## Epic 1 — Match Generation

> Generate all possible doubles matches from a player list.

| #   | Story                                                                                             | Description                                          | Status |
| --- | ------------------------------------------------------------------------------------------------- | ---------------------------------------------------- | ------ |
| 1   | [Set Up Solution](docs/epics/epic1-match-generation/project-setup/change.md)                      | Scaffold solution with all projects, MudBlazor, CORS | Done   |
| 2   | [Define Domain Models](docs/epics/epic1-match-generation/domain-models/change.md)                 | Player, Team, Match entities + Shared DTOs           | Done   |
| 3   | [Implement Match Generator](docs/epics/epic1-match-generation/match-generation-service/change.md) | C(N,4)×3 algorithm, unit tests                       | Done   |
| 4   | [Build Player & Match API](docs/epics/epic1-match-generation/player-match-api/change.md)          | Controllers, CQRS, Swagger, integration tests        | Done   |
| 5   | [Build Player UI](docs/epics/epic1-match-generation/player-management-ui/change.md)               | Add/remove players in Blazor WASM                    | Done   |
| 6   | [Build Match Display](docs/epics/epic1-match-generation/match-display-ui/change.md)               | Generate and display all matches                     | Done   |
| 7   | [Polish UI](docs/epics/epic1-match-generation/ui-polish/change.md)                                | Layout, home page, responsive design                 | Done   |

---

---

## Epic 2 — Season Management

> Manage seasons with a name, date range, weekly matchday, and start/end hours. Matchdays are computed from the date range and weekday — not stored.

| #   | Story                                                                                                     | Description                                                                    | Status  |
| --- | --------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------ | ------- |
| 1   | [Define Season Domain & DTOs](docs/epics/epic2-season-management/season-domain-and-dtos/change.md)        | `Season` entity with `GetMatchdays()`, `SeasonDto`, Create/Update request DTOs | Pending |
| 2   | [Implement Season CQRS & Repository](docs/epics/epic2-season-management/season-cqrs-repository/change.md) | Wolverine commands/queries, `ISeasonRepository`, `InMemorySeasonRepository`    | Pending |
| 3   | [Build Season API](docs/epics/epic2-season-management/season-api-endpoints/change.md)                     | CRUD + matchdays endpoints, `SeasonMapper`, DI registration                    | Pending |
| 4   | [Build Season UI](docs/epics/epic2-season-management/season-blazor-ui/change.md)                          | `SeasonApiClient`, list/create/edit/detail pages, `SeasonForm` component       | Pending |
| 5   | [Write Season Tests](docs/epics/epic2-season-management/season-tests/change.md)                           | Unit tests for domain + handlers, integration tests for API endpoints          | Pending |

---

## Epic 3 — Season Match Planning

> Generate and schedule matches for a season's matchdays using enrolled players.

| #   | Story                                                                                                   | Description                                                              | Status  |
| --- | ------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------ | ------- |
| 1   | [View Season Match Pool](docs/epics/epic3-season-match-planning/season-match-pool/change.md)            | View all possible matches generated from a season's enrolled players     | Pending |
| 2   | [Generate Season Schedule](docs/epics/epic3-season-match-planning/season-schedule-generation/change.md) | Automatically assign random unique matches to all open matchdays at once | Pending |
| 3   | [Browse & Manage Schedule](docs/epics/epic3-season-match-planning/season-schedule-management/change.md) | View full matchday schedule, clear individual or all planned matches     | Pending |

---

## Epic 4 — Player Absence Management

> Record player absences per matchday so the automatic schedule generator only picks matches where all four players are available.

| #   | Story                                                                                                            | Description                                                                                | Status  |
| --- | ---------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------ | ------- |
| 1   | [Record Player Absences](docs/epics/epic4-player-absence-management/player-absence-recording/change.md)          | Mark/unmark players as absent on specific matchdays, with domain, API, and UI              | Pending |
| 2   | [Absence-Aware Schedule Generation](docs/epics/epic4-player-absence-management/absence-aware-schedule/change.md) | Filter the match pool per matchday to exclude matches with absent players before assigning | Pending |

---

## Epic 5 — Migrate from MediatR to Wolverine

> Replace MediatR with Wolverine as the mediator/message bus, adopting convention-based handlers while preserving the CQRS architecture.

| #   | Story                                                                                                                        | Description                                                                             | Status |
| --- | ---------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- | ------ |
| 1   | [Replace MediatR with Wolverine Using Shims](docs/epics/epic5-wolverine-migration/wolverine-mediatr-shims/change.md)         | Swap NuGet package, DI, controllers, and using directives via Wolverine's MediatR shims | Done   |
| 2   | [Convert Handlers to Wolverine Native Conventions](docs/epics/epic5-wolverine-migration/wolverine-native-handlers/change.md) | Remove shims, convert to static Handle methods with method injection                    | Done   |

---

## Epic 6 — SQL Server Persistence with EF Core

> Replace in-memory repositories with SQL Server persistence using Entity Framework Core, preserving the Clean Architecture repository pattern.

| #   | Story                                                                                                                                                                                                            | Description                                                                             | Status  |
| --- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | --------------------------------------------------------------------------------------- | ------- |
| 1   | [Make Domain Entities EF Core Compatible](docs/epics/epic6-sql-server-ef-core/ef-core-domain-compatibility/change.md) · [plan](docs/epics/epic6-sql-server-ef-core/ef-core-domain-compatibility/plan.md)         | Add private parameterless constructors and private set accessors to all domain entities | Pending |
| 2   | [Convert Repositories to Async](docs/epics/epic6-sql-server-ef-core/async-repository-interfaces/change.md) · [plan](docs/epics/epic6-sql-server-ef-core/async-repository-interfaces/plan.md)                     | Make IPlayerRepository and ISeasonRepository fully async, update handlers               | Pending |
| 3   | [Add DbContext, Configurations, and EF Repos](docs/epics/epic6-sql-server-ef-core/ef-core-dbcontext-repositories/change.md) · [plan](docs/epics/epic6-sql-server-ef-core/ef-core-dbcontext-repositories/plan.md) | EF Core setup, entity configurations, SQL Server repositories, DI wiring, migration     | Pending |
| 4   | [Update Test Infrastructure for EF Core](docs/epics/epic6-sql-server-ef-core/ef-core-test-infrastructure/change.md) · [plan](docs/epics/epic6-sql-server-ef-core/ef-core-test-infrastructure/plan.md)            | Replace in-memory repo DI swaps with SQLite in-memory DbContext in integration tests    | Pending |

---

## Epic 7 — E2E Tests with Playwright

> Validate end-to-end user flows across the full stack using Playwright, running against the live Blazor WASM + API dev stack.

| #   | Story                                                                                                                                                                                   | Description                                                                  | Status  |
| --- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------- | ------- |
| 1   | [Scaffold Playwright Test Project](docs/epics/epic7-e2e-playwright/scaffold-playwright-project/change.md) · [plan](docs/epics/epic7-e2e-playwright/scaffold-playwright-project/plan.md) | Create test project, configure app startup, base URL, and CI task            | Pending |
| 2   | [Test Player & Match Generation](docs/epics/epic7-e2e-playwright/test-player-match-generation/change.md) · [plan](docs/epics/epic7-e2e-playwright/test-player-match-generation/plan.md) | E2E tests for the add-players → generate-matches → view-results flow         | Pending |
| 3   | [Test Season Management](docs/epics/epic7-e2e-playwright/test-season-management/change.md) · [plan](docs/epics/epic7-e2e-playwright/test-season-management/plan.md)                     | E2E tests for season CRUD, matchday listing, and player enrollment           | Pending |
| 4   | [Test Match Schedule Planning](docs/epics/epic7-e2e-playwright/test-match-schedule-planning/change.md) · [plan](docs/epics/epic7-e2e-playwright/test-match-schedule-planning/plan.md)   | E2E tests for schedule generation, absence management, and schedule browsing | Pending |

---

## Future Epics

| Epic | Description |
| ---- | ----------- |
