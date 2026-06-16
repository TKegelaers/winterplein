# Align Solution to Service Layout

## Problem Statement

Winterplein's solution layout has drifted from the team's standard service project layout (`docs/service-project-layout.md`, derived from `KOAla.Opvangvoorzieningen`). The current solution uses a `Shared` contracts project, an `Api` host, three coarse test projects, and ad-hoc internal folder structures (e.g. `Seasons/` mixing commands, queries, and handlers). New developers and shared tooling expect the standard layout, so the divergence raises onboarding cost and makes cross-service conventions harder to apply.

This is a pure structural refactor: no behavior, endpoints, or business logic change.

## Proposed Solution

Refactor the solution to match the reference layout, adapted pragmatically to Winterplein's actual stack. Winterplein is a single service, so the project prefix stays `Winterplein.` (no `{Service}` segment).

Apply the parts of the reference that fit:

- **Rename `Winterplein.Shared` → `Winterplein.Application.IO`** as the Domain-free contracts layer. Move DTOs plus the CQRS message types (commands and queries) into it. The Blazor `Client` references `Application.IO` instead of `Shared`.
- **Restructure `Winterplein.Application`** into the reference folders: `CommandHandlers/`, `QueryHandlers/`, `Ports/` (repository and service interfaces), `Mappers/`, plus an `IAmApplication` marker interface. Handlers stay here; the message types they handle live in `Application.IO`.
- **Restructure `Winterplein.Infrastructure`** to align DbContext, entity configurations, and repositories with the reference folder structure.
- **Rename `Winterplein.Api` → `Winterplein.WebApi`**, add a `Configuration/` folder for IoC/startup wiring, and keep `Controllers/`.
- **Split the three test projects** into per-layer projects: `Domain.UnitTests`, `Application.UnitTests`, `Infrastructure.UnitTests`, `WebApi.UnitTests`, a shared `Common.UnitTests` (today's `UnitTests.Common`), and the existing `IntegrationTests`.

Deliberately **out of scope** because they do not apply to Winterplein: the DbUp `Database` project (Winterplein keeps EF Core migrations), `Providers.*` integrations, `Synchronisatie`, Hangfire, Rebus, MediatR-style naming, and domain sub-type folders (Winterplein has none).

The `.slnx` is CLI-managed throughout — all project add/remove/rename go through `dotnet sln`, never hand-edited.

## Business Requirements

This is a technical refactor; requirements are structural rules rather than user behavior.

**Given** the refactored solution
**When** a developer inspects the projects
**Then** project names, references, and internal folders match the reference layout for every applicable layer

**Given** the `Application.IO` project
**When** its references are inspected
**Then** it has no dependency on `Domain`, `Application`, or `Infrastructure` — it is a pure contracts layer

**Given** the `Client` project
**When** its references are inspected
**Then** it references only `Application.IO` (replacing the former `Shared` reference) and never `Application`, `Domain`, or `Infrastructure`

**Given** the full refactor
**When** the application runs
**Then** all existing endpoints and UI behave exactly as before — no functional change

## Acceptance Criteria

- [ ] `Winterplein.Shared` is renamed to `Winterplein.Application.IO`; DTOs, commands, and queries live in it; it has no `Domain` reference
- [ ] `Winterplein.Client` references `Application.IO` only; the solution builds and the client runs unchanged
- [ ] `Winterplein.Application` uses `CommandHandlers/`, `QueryHandlers/`, `Ports/`, `Mappers/` folders and defines an `IAmApplication` marker; the `Seasons/` flat folder is gone
- [ ] `Winterplein.Infrastructure` DbContext, configurations, and repositories follow the reference folder structure; EF Core migrations still work
- [ ] `Winterplein.Api` is renamed to `Winterplein.WebApi` with a `Configuration/` folder; all references and launch settings updated
- [ ] Test projects are split per layer (`Domain`, `Application`, `Infrastructure`, `WebApi` UnitTests) plus `Common.UnitTests` and `IntegrationTests`; every existing test still passes
- [ ] `dotnet build` and `dotnet test` are green with no new warnings
- [ ] All project changes are made via the `dotnet sln` / `dotnet` CLI, not by hand-editing `.slnx`

## Testing Plan

- Automated:
  - Run the full existing test suite after each rename/restructure step — it must stay green (the suite is the behavior-preservation guard for a refactor with no new logic)
  - Confirm `dotnet build` produces no new warnings in changed projects
- Manual:
  - Run the API and Blazor client; verify player management, match generation, and season flows behave identically to before
  - Verify Swagger still lists all endpoints unchanged

## Refactors

- Move CQRS message types (commands/queries) out of `Application` into `Application.IO`, leaving only handlers in `Application`
- Collapse the `Seasons/` flat folder into the `CommandHandlers/` + `QueryHandlers/` convention
- Relocate repository and service interfaces (`IPlayerRepository`, `ISeasonRepository`, `IMatchGeneratorService`) into `Application/Ports/`

## Potential Pitfalls

- **Application.IO Domain-free rule:** any command/query that currently references a `Domain` type (entity, value object, enum) must use a DTO instead, or stay in `Application`. Audit each message before moving it.
- **Project renames break references everywhere:** `.csproj` references, namespaces, `using` directives, Wolverine assembly discovery (`IncludeAssembly`), CORS origin comments, and `launchSettings.json` all reference old names. Rename via CLI and sweep all references.
- **Namespace churn vs. behavior:** renaming projects changes namespaces; keep changes mechanical and rely on the test suite to catch regressions.
- **EF migrations history:** restructuring Infrastructure must not orphan the existing migration/snapshot — keep the migrations assembly and DbContext discoverable.
- **Sequencing:** order steps so the solution compiles after each (contracts rename → application internals → infrastructure → host rename → test split), avoiding a long broken-build window.
