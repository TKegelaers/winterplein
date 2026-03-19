# Winterplein — Roadmap

Winterplein is a tennis doubles match generator. Given a list of players, it automatically generates all possible doubles (2v2) matches using combinatorics: C(N,4) groups × 3 unique team pairings = total matches (e.g. 10 players → 630 matches).

**Tech stack:** .NET 10 · Blazor WebAssembly · ASP.NET Core Web API · MudBlazor · Clean Architecture

---

## Epic 1 — Match Generation

> Generate all possible doubles matches from a player list.

| # | Story | Description |
|---|-------|-------------|
| 1 | [Project Setup](tasks/epic1-match-generation/story1-project-setup.md) | Scaffold solution with all projects, MudBlazor, CORS |
| 2 | [Domain Models](tasks/epic1-match-generation/story2-domain-models.md) | Player, Team, Match entities + Shared DTOs |
| 3 | [Match Generation Service](tasks/epic1-match-generation/story3-match-generation-service.md) | C(N,4)×3 algorithm, unit tests |
| 4 | [API Endpoints](tasks/epic1-match-generation/story4-api-endpoints.md) | Minimal API for players and match generation |
| 5 | [Player Management UI](tasks/epic1-match-generation/story5-player-management-ui.md) | Add/remove players in Blazor WASM |
| 6 | [Match Display UI](tasks/epic1-match-generation/story6-match-display-ui.md) | Generate and display all matches |
| 7 | [UI Polish](tasks/epic1-match-generation/story7-ui-polish.md) | Layout, home page, responsive design |

---

## Future Epics

| Epic | Description |
|------|-------------|
| Epic 2 — Match Scheduling | Assign matches to time slots and courts |
| Epic 3 — Score Tracking | Record scores, determine winners, calculate standings |
| Epic 4 — Player Statistics | Win/loss records, partner performance, head-to-head stats |
| Epic 5 — Persistence | SQLite/PostgreSQL with EF Core; save/load tournaments |
| Epic 6 — Tournament Modes | Round-robin, Swiss-system, bracket elimination |
| Epic 7 — Export & Share | PDF/Excel export of schedule and results |
