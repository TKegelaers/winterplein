# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Winterplein is a tennis doubles match generator. Given N players, it generates all possible doubles (2v2) matches using combinatorics: C(N,4) groups × 3 unique team pairings (e.g. 10 players → 630 matches).

- ROADMAP.md = high-level plan
- .tasks/ = one folder per epic and one file per user story
- User stories have tasks with dependencies using TaskCreate and addBlockedBy."

**Tech stack:** .NET 10 · Blazor WebAssembly · ASP.NET Core Minimal API · MudBlazor · Clean Architecture

## Workflow

1. Always read ROADMAP.md before starting work
2. Pick the next incomplete task from the relevant .tasks/epic file
3. Mark tasks as done when complete
4. Update ROADMAP.md progress after each session

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

```
Winterplein.Domain          — entities, no external dependencies
Winterplein.Shared          — DTOs shared between API and Client, no external dependencies
Winterplein.Application     — use cases/services, refs Domain
Winterplein.Infrastructure  — EF Core, external services, refs Application + Domain
Winterplein.Api             — ASP.NET Core Minimal API, refs Application + Infrastructure + Shared
Winterplein.Client          — Blazor WASM (MudBlazor), refs Shared only
tests/Winterplein.UnitTests        — xUnit, refs Application + Domain
tests/Winterplein.IntegrationTests — xUnit, refs Api
```

Key constraint: `Winterplein.Client` only references `Winterplein.Shared` — it communicates with the API over HTTP, never directly calling application or domain code.

## Current State

The solution is scaffolded (Story 1 T1-T3 complete) but most projects contain only placeholder `Class1.cs` files. Feature work begins at Story 1 T4 (project references) through Story 7 (UI polish). See `.tasks/epic1-match-generation/` for detailed story files and task checklists.

## Development Notes

- The match generation algorithm lives (or will live) in `Winterplein.Application` as a service
- API uses Minimal APIs pattern (no controllers)
- CORS must allow the Blazor client origin (`http://localhost:5149`) — configure in `Winterplein.Api/Program.cs`
- MudBlazor is the UI component library for the Blazor client
- xUnit is used for all tests; `Xunit` is globally imported in test projects
