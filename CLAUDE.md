# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Winterplein is a tennis doubles match generator. Given N players, it generates all possible doubles (2v2) matches using combinatorics: C(N,4) groups × 3 unique team pairings (e.g. 10 players → 630 matches).

- ROADMAP.md = high-level plan
- .tasks/ = one folder per epic and one file per user story
- User stories have tasks with dependencies using TaskCreate and addBlockedBy.

**Tech stack:** .NET 10 · Blazor WebAssembly · ASP.NET Core Controllers · MudBlazor · Clean Architecture · CQRS · MediatR

## Workflow

1. Always read ROADMAP.md before starting work
2. Pick the next incomplete task from the relevant .tasks/epic file
3. Before starting the work on a new User Story always create a new branch beginning with `feat/`; use `fix/` for bugfixes
4. Mark tasks as done when complete
5. Update ROADMAP.md progress after each session

## Commands

```bash
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

``` text
Winterplein.Domain          — entities, no external dependencies
Winterplein.Shared          — DTOs shared between API and Client, no external dependencies
Winterplein.Application     — CQRS commands/queries/handlers (MediatR), refs Domain + Shared
Winterplein.Infrastructure  — EF Core, external services, refs Application + Domain
Winterplein.Api             — ASP.NET Core Controllers, refs Application + Infrastructure + Shared
Winterplein.Client          — Blazor WASM (MudBlazor), refs Shared only
tests/Winterplein.UnitTests        — xUnit + FluentAssertions, refs Application + Domain + UnitTests.Common
tests/Winterplein.UnitTests.Common — Test builders, refs Domain
tests/Winterplein.IntegrationTests — xUnit + FluentAssertions, refs Api + UnitTests.Common
```

Key constraint: `Winterplein.Client` only references `Winterplein.Shared` — it communicates with the API over HTTP, never directly calling application or domain code.

## Current State

Stories 1–4 are complete:
- **Story 1** — project setup
- **Story 2** — domain models: entities (`Player`, `Team`, `Match`), enums (`Gender`), value objects (`Name`); `Winterplein.Shared` DTOs (`PlayerDto`, `TeamDto`, `MatchDto`); Domain→DTO extension-method mappers in `Winterplein.Application/Mappers/`
- **Story 3** — match generation service: `GenerateMatchesCommandHandler` using C(N,4)×3 algorithm in `Winterplein.Application`
- **Story 4** — API endpoints: `PlayersController` and `MatchesController` using CQRS/MediatR; global exception handler; CORS configured; Swagger UI at `/swagger`; `GenderDto` enum in `Winterplein.Shared`; `JsonStringEnumConverter` configured globally; `WinterpleinApiFactory` + 19 integration tests

Next work is Story 5 (Player Management UI). See `.tasks/` for detailed story files and task checklists.

## Shell Commands

When generating shell commands:

- Never use `cd`, `cd &&`, or any compound directory‑changing commands.
- Always use `git -C <path>` for Git operations.
- If I provide a command using `cd`, rewrite it using `git -C` automatically.


## Development Notes

- The application layer uses CQRS via MediatR: commands (write) and queries (read) live in `Winterplein.Application`, handlers are registered via `services.AddMediatR(...)`
- The match generation algorithm lives in `Winterplein.Application` as a MediatR command handler (`GenerateMatchesCommandHandler`)
- API uses Controllers
- CORS must allow the Blazor client origin (`http://localhost:5149`) — configure in `Winterplein.Api/Program.cs`
- MudBlazor is the UI component library for the Blazor client
- xUnit is used for all tests; `Xunit` is globally imported in test projects
- FluentAssertions is used alongside xUnit; `FluentAssertions` is globally imported in test projects
- Moq is used for mocking; `using Moq;` must be added explicitly (not globally imported)
- Test builders live in `tests/Winterplein.UnitTests.Common/Builders/` (`PlayerBuilder`, `TeamBuilder`, `MatchBuilder`, `NameBuilder`)
- Domain→DTO mappers are extension methods in `src/Winterplein.Application/Mappers/`
