# Winterplein — Roadmap

Winterplein is a tennis doubles match generator. Given a list of players, it automatically generates all possible doubles (2v2) matches using combinatorics: C(N,4) groups × 3 unique team pairings = total matches (e.g. 10 players → 630 matches).

**Tech stack:** .NET 10 · Blazor WebAssembly · ASP.NET Core Web API · MudBlazor · Clean Architecture

---

## Epic 1 — Match Generation

> Generate all possible doubles matches from a player list.

| # | Story | Description | Status |
| --- | ------- | ------------- | -------- |
| 1 | [Project Setup](.tasks/epic1-match-generation/story1-project-setup.md) | Scaffold solution with all projects, MudBlazor, CORS | Done |
| 2 | [Domain Models](.tasks/epic1-match-generation/story2-domain-models.md) | Player, Team, Match entities + Shared DTOs | Done |
| 3 | [Match Generation Service](.tasks/epic1-match-generation/story3-match-generation-service.md) | C(N,4)×3 algorithm, unit tests | Done |
| 4 | [API Endpoints](.tasks/epic1-match-generation/story4-api-endpoints.md) | Controllers, CQRS/MediatR, Swagger, integration tests | Done |
| 5 | [Player Management UI](.tasks/epic1-match-generation/story5-player-management-ui.md) | Add/remove players in Blazor WASM | Done |
| 6 | [Match Display UI](.tasks/epic1-match-generation/story6-match-display-ui.md) | Generate and display all matches | Done |
| 7 | [UI Polish](.tasks/epic1-match-generation/story7-ui-polish.md) | Layout, home page, responsive design | Pending |

---

---

## Epic 2 — Season Management

> Manage seasons with a name, date range, weekly matchday, and start/end hours. Matchdays are computed from the date range and weekday — not stored.

| # | Story | Description | Status |
| --- | ------- | ------------- | -------- |
| 1 | [Domain Entity & Shared DTOs](.tasks/epic2-season-management/story1-domain-and-dtos.md) | `Season` entity with `GetMatchdays()`, `SeasonDto`, Create/Update request DTOs | Pending |
| 2 | [CQRS Handlers & In-Memory Repository](.tasks/epic2-season-management/story2-cqrs-and-repository.md) | MediatR commands/queries, `ISeasonRepository`, `InMemorySeasonRepository` | Pending |
| 3 | [Minimal API Endpoints](.tasks/epic2-season-management/story3-api-endpoints.md) | CRUD + matchdays endpoints, `SeasonMapper`, DI registration | Pending |
| 4 | [Blazor WASM UI](.tasks/epic2-season-management/story4-blazor-ui.md) | `SeasonApiClient`, list/create/edit/detail pages, `SeasonForm` component | Pending |
| 5 | [Tests](.tasks/epic2-season-management/story5-tests.md) | Unit tests for domain + handlers, integration tests for API endpoints | Pending |

---

## Future Epics

| Epic | Description |
|------|-------------|
