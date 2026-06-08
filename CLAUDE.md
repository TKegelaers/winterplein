# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Winterplein is a tennis doubles match generator. Given N players, it generates all possible doubles (2v2) matches using combinatorics: C(N,4) groups × 3 unique team pairings (e.g. 10 players → 630 matches).

- ROADMAP.md = high-level plan (epics + story status table)
- docs/epics/ = one folder per epic (e.g. `epic2-season-management/`), each containing one folder per story/change with change.md (from /w-explore) and plan.md + tasks/ (from /w-plan)

**Tech stack:** .NET 10 · Blazor WebAssembly · ASP.NET Core Controllers · MudBlazor · Clean Architecture · CQRS · Wolverine

## Workflow

### Planning a new story

1. Run `/create-epic` to register a new epic in ROADMAP.md (plain-text story titles, no file links yet).
2. Run `/w-explore` for each story — produces `docs/epics/<epic-folder>/<change-name>/change.md`. Update the ROADMAP.md story title cell to link to that file.
3. Run `/w-plan <epic-folder>/<change-name>` — produces `docs/epics/<epic-folder>/<change-name>/plan.md` and task files. Append the plan link in the same ROADMAP.md cell.

### Implementing

4. Always read ROADMAP.md before starting work.
5. Pick the next pending task from the relevant `docs/epics/<epic-folder>/<change-name>/plan.md`.
6. Mark tasks done in the plan file as work completes.
7. Update ROADMAP.md story Status column when a story is complete.

## Commands

```powershell
# Build entire solution
dotnet build

# Run API (http://localhost:5095)
dotnet run --project src/Winterplein.Api

# Run Blazor WASM client (http://localhost:5149)
dotnet run --project src/Winterplein.Client

# Run all tests
dotnet test

# Run a single test project
dotnet test tests/Winterplein.UnitTests
dotnet test tests/Winterplein.IntegrationTests

# Run a specific test by name filter
dotnet test --filter "FullyQualifiedName~MyTestClass.MyTestMethod"
```

## Architecture

Clean Architecture with strict dependency rules:

```text
Winterplein.Domain          — entities, no external dependencies
Winterplein.Shared          — DTOs shared between API and Client, no external dependencies
Winterplein.Application     — CQRS commands/queries/handlers (Wolverine native), refs Domain + Shared only
Winterplein.Infrastructure  — EF Core, external services, refs Application + Domain
Winterplein.Api             — ASP.NET Core Controllers, refs Application + Infrastructure + Shared
Winterplein.Client          — Blazor WASM (MudBlazor), refs Shared only
tests/Winterplein.UnitTests        — xUnit + FluentAssertions, refs Application + Domain + UnitTests.Common
tests/Winterplein.UnitTests.Common — Test builders, refs Domain
tests/Winterplein.IntegrationTests — xUnit + FluentAssertions, refs Api + UnitTests.Common
```

Key constraint: `Winterplein.Client` only references `Winterplein.Shared` — it communicates with the API over HTTP, never directly calling application or domain code.

## Current State

See ROADMAP.md for the authoritative status table. Summary:

**Epic 1 — Match Generation**

- Stories 1–7: all Done

**Epic 2 — Season Management**

- Stories 1–5: all Pending — **next up: Story 1 (Season Domain & DTOs)**

**Epic 3 — Season Match Planning**

- Stories 1–3: all Pending

**Epic 4 — Player Absence Management**

- Stories 1–2: all Pending

**Epic 5 — Migrate from MediatR to Wolverine**

- Stories 1–2: all Done

**Epic 6 — SQL Server Persistence with EF Core**

- Stories 1–4: all Pending

**Epic 7 — E2E Tests with Playwright**

- Stories 1–4: all Pending

## Development Notes

- The application layer uses CQRS via Wolverine: commands (write) and queries (read) live in `Winterplein.Application`; handlers are static classes with a static `Handle(message, ...deps)` method — Wolverine discovers them by convention and injects dependencies as method parameters
- Wolverine is registered in `Winterplein.Api/Program.cs` via `builder.Host.UseWolverine(opts => opts.Discovery.IncludeAssembly(...))`. Controllers use `IMessageBus.InvokeAsync<T>(message)` for queries/commands that return a result, and `IMessageBus.InvokeAsync(message)` for void commands
- Handlers must return reference types (not `int`, `bool`, etc.) — Wolverine does not support value-type returns from `InvokeAsync<T>`
- The match generation algorithm lives in `Winterplein.Application` as a Wolverine native handler (`GenerateMatchesCommandHandler`)
- API uses Controllers (`[ApiController]` + `ControllerBase`) for both epics
- CORS must allow the Blazor client origin (`http://localhost:5149`) — configure in `Winterplein.Api/Program.cs`
- MudBlazor is the UI component library for the Blazor client
- xUnit is used for all tests; `Xunit` is globally imported in test projects
- FluentAssertions is used alongside xUnit; `FluentAssertions` is globally imported in test projects
- Moq is used for mocking; `using Moq;` must be added explicitly (not globally imported)
- Test builders live in `tests/Winterplein.UnitTests.Common/Builders/` (`PlayerBuilder`, `TeamBuilder`, `MatchBuilder`, `NameBuilder`)
- Domain→DTO mappers are extension methods in `src/Winterplein.Application/Mappers/`
- `AppState` (scoped service in `Winterplein.Client/Services/`) holds `PlayerCount` and `MatchCount` for cross-component state sharing; components subscribe via `AppState.OnChange` and implement `IDisposable` to unsubscribe
- Custom MudBlazor theme is defined in `Winterplein.Client/WinterpleinTheme.cs` (tennis/sport palette: green primary, amber secondary)
