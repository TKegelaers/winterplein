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
dotnet run --project src/Winterplein.WebApi

# Run Blazor WASM client (http://localhost:5149)
dotnet run --project src/Winterplein.Client

# Run all tests
dotnet test

# Run a single test project
dotnet test tests/Winterplein.Application.UnitTests
dotnet test tests/Winterplein.IntegrationTests

# Run a specific test by name filter
dotnet test --filter "FullyQualifiedName~MyTestClass.MyTestMethod"
```

## Architecture

Clean Architecture with strict dependency rules:

```text
Winterplein.Domain                 — entities, no external dependencies
Winterplein.Application.IO         — DTOs + CQRS commands/queries (contracts), no Domain dependency
Winterplein.Application            — CQRS handlers (Wolverine native) in CommandHandlers/QueryHandlers, Ports/, Mappers/; refs Domain + Application.IO
Winterplein.Infrastructure         — EF Core (DbContext, Configurations/, Repositories/), refs Application + Domain
Winterplein.WebApi                 — ASP.NET Core Controllers + Configuration/ (IocConfig), refs Application + Infrastructure + Application.IO
Winterplein.Client                 — Blazor WASM (MudBlazor), refs Application.IO only
tests/Winterplein.Domain.UnitTests         — refs Domain + Common.UnitTests
tests/Winterplein.Application.UnitTests     — refs Application + Application.IO + Domain + Common.UnitTests
tests/Winterplein.Infrastructure.UnitTests  — refs Infrastructure + Application + Domain + Common.UnitTests
tests/Winterplein.WebApi.UnitTests          — refs WebApi + Application.IO + Common.UnitTests
tests/Winterplein.Common.UnitTests          — Test builders, refs Domain
tests/Winterplein.IntegrationTests          — refs WebApi + Infrastructure + Common.UnitTests
```

Key constraint: `Winterplein.Client` only references `Winterplein.Application.IO` — it communicates with the API over HTTP, never directly calling application or domain code.

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

**Epic 8 — Solution Structure Alignment**

- Story 1: Done

## Development Notes

- The application layer uses CQRS via Wolverine: command/query message types (write/read) live in `Winterplein.Application.IO` (so the Client can reference them); their handlers live in `Winterplein.Application` under `CommandHandlers/`/`QueryHandlers/` as static classes with a static `Handle(message, ...deps)` method — Wolverine discovers them by convention and injects dependencies as method parameters
- Wolverine is registered in `Winterplein.WebApi` (`Configuration/IocConfig.cs`) via `UseWolverine(opts => opts.Discovery.IncludeAssembly(typeof(IAmApplication).Assembly))`. Controllers use `IMessageBus.InvokeAsync<T>(message)` for queries/commands that return a result, and `IMessageBus.InvokeAsync(message)` for void commands
- Handlers must return reference types (not `int`, `bool`, etc.) — Wolverine does not support value-type returns from `InvokeAsync<T>`
- The match generation algorithm lives in `Winterplein.Application` as a Wolverine native handler (`GenerateMatchesCommandHandler`)
- API uses Controllers (`[ApiController]` + `ControllerBase`) for both epics
- CORS must allow the Blazor client origin (`http://localhost:5149`) — configure in `Winterplein.WebApi` (`Configuration/IocConfig.cs`)
- MudBlazor is the UI component library for the Blazor client
- xUnit is used for all tests; `Xunit` is globally imported in test projects
- FluentAssertions is used alongside xUnit; `FluentAssertions` is globally imported in test projects
- Moq is used for mocking; `using Moq;` must be added explicitly (not globally imported)
- Test builders live in `tests/Winterplein.Common.UnitTests/Builders/` (`PlayerBuilder`, `TeamBuilder`, `MatchBuilder`, `NameBuilder`)
- Domain→DTO mappers are extension methods in `src/Winterplein.Application/Mappers/`
- `AppState` (scoped service in `Winterplein.Client/Services/`) holds `PlayerCount` and `MatchCount` for cross-component state sharing; components subscribe via `AppState.OnChange` and implement `IDisposable` to unsubscribe
- Custom MudBlazor theme is defined in `Winterplein.Client/WinterpleinTheme.cs` (tennis/sport palette: green primary, amber secondary)
