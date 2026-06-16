# Align Solution to Service Layout — Design

Technical design for the structural refactor described in
`docs/epics/epic8-solution-structure-alignment/align-solution-layout/change.md`.
Problem and acceptance criteria live in `change.md`; this document covers the
technical approach, architecture decisions, data flow, and file changes.

## Technical Approach

Pure structural refactor executed as an ordered sequence of project renames,
folder restructures, and reference sweeps. The driving constraint is that the
solution must build (`dotnet build`) and the suite must stay green
(`dotnet test`) after **every** task, so the work is sequenced to never leave a
long broken-build window.

All project add/remove/rename go through the `dotnet sln` / `dotnet` CLI; the
`.slnx` is never hand-edited.

Order of operations:

1. Contracts rename: `Winterplein.Shared` -> `Winterplein.Application.IO`, move
   the CQRS message types (commands/queries) into it.
2. Application internals: `CommandHandlers/`, `QueryHandlers/`, `Ports/`,
   `Mappers/`, `IAmApplication` marker; delete the flat `Seasons/` and
   `Interfaces/` folders.
3. Infrastructure folder restructure (keep EF migrations discoverable).
4. Host rename: `Winterplein.Api` -> `Winterplein.WebApi`, add `Configuration/`.
5. Test split into per-layer projects.

## Current vs. Target Layout

```
CURRENT                              TARGET
src/Winterplein.Domain               src/Winterplein.Domain            (unchanged)
src/Winterplein.Shared               src/Winterplein.Application.IO
src/Winterplein.Application          src/Winterplein.Application       (restructured)
src/Winterplein.Infrastructure       src/Winterplein.Infrastructure    (restructured)
src/Winterplein.Api                  src/Winterplein.WebApi
src/Winterplein.Client               src/Winterplein.Client            (repointed)

tests/Winterplein.UnitTests          tests/Winterplein.Domain.UnitTests
                                     tests/Winterplein.Application.UnitTests
                                     tests/Winterplein.Infrastructure.UnitTests
                                     tests/Winterplein.WebApi.UnitTests
tests/Winterplein.UnitTests.Common   tests/Winterplein.Common.UnitTests
tests/Winterplein.IntegrationTests   tests/Winterplein.IntegrationTests (refs repointed)
```

## Architecture Decisions

### Decision: Move CQRS message types into Application.IO; handlers stay in Application

**Alternatives**: Keep messages in Application (status quo); duplicate message
contracts in both layers.
**Rationale**: The reference layout places commands/queries in `Application.IO`
as caller-facing contracts. The current message types are already effectively
Domain-free in their signatures (only stray `using Winterplein.Domain.Entities`
directives that are unused, see audit below), so the move is safe. Handlers
reference Domain and repositories, so they must remain in `Application`.

### Decision: Application.IO namespace becomes `Winterplein.Application.IO`

**Alternatives**: Keep `Winterplein.Shared` namespaces inside the renamed
project; introduce a `.Contracts` namespace.
**Rationale**: Project rename should carry the namespace for consistency with
the reference. DTOs move from `Winterplein.Shared.DTOs` to
`Winterplein.Application.IO.DTOs`; messages move under
`Winterplein.Application.IO.Commands` / `.Queries`. This is mechanical namespace
churn caught by the compiler and the test suite.

### Decision: Repoint Wolverine discovery to a type that stays in Application

**Alternatives**: Point discovery at the Application.IO assembly.
**Rationale**: `Program.cs` currently discovers handlers via
`typeof(GetAllPlayersQuery).Assembly`. `GetAllPlayersQuery` moves to
Application.IO, which would make Wolverine scan the wrong (handler-free)
assembly. Discovery must reference a type that stays in `Application` — the new
`IAmApplication` marker interface. This is a load-bearing change: missing it
silently breaks handler registration at runtime, not compile time.

### Decision: Keep EF Core migrations; do not adopt the DbUp Database project

**Alternatives**: Reference layout's standalone DbUp `Database` project.
**Rationale**: Explicitly out of scope per change.md. The Infrastructure
restructure must keep the `Migrations/` folder and snapshot in the same assembly
as `WinterpleinDbContext`, and `OnModelCreating` uses
`ApplyConfigurationsFromAssembly(typeof(WinterpleinDbContext).Assembly)` — moving
configurations within the same project is safe; moving the DbContext across
assemblies is not. Keep all of it in `Winterplein.Infrastructure`.

### Decision: Split the monolithic UnitTests project per layer

**Alternatives**: Keep one UnitTests project.
**Rationale**: Reference layout mandates per-layer test projects. The current
`UnitTests` already groups tests into `Domain/`, `Application/`, `Infrastructure/`,
`Api/`, `Seasons/` folders, so the split is a near-mechanical move of files into
new projects with the matching production reference. `WebApi.UnitTests` is the
only project that references the host; the others reference only their layer.

## Application.IO No-Domain Audit

Every message type was inspected for Domain coupling in its public signature:

| Message type              | Domain in signature?                  | Action                  |
| ------------------------- | ------------------------------------- | ----------------------- |
| AddPlayerCommand          | No (uses `GenderDto`)                 | move to .IO             |
| RemovePlayerCommand       | No                                    | move to .IO             |
| GenerateMatchesCommand    | No                                    | move to .IO             |
| GetAllPlayersQuery        | No                                    | move to .IO             |
| GetMatchCountQuery        | No                                    | move to .IO             |
| CreateSeasonCommand       | No (primitives only)                  | move to .IO             |
| UpdateSeasonCommand       | No (primitives only)                  | move to .IO             |
| DeleteSeasonCommand       | No                                    | move to .IO             |
| AddSeasonPlayerCommand    | No                                    | move to .IO             |
| RemoveSeasonPlayerCommand | No                                    | move to .IO             |
| GetSeasonByIdQuery        | No (has stray unused `using Domain`)  | move to .IO, drop using |
| GetSeasonsQuery           | No (has stray unused `using Domain`)  | move to .IO, drop using |
| GetSeasonPlayersQuery     | No (stray unused `using Shared.DTOs`) | move to .IO             |

Result: **all 13 message types are safe to move.** The stray `using` directives
on `GetSeasonByIdQuery`, `GetSeasonsQuery`, and several Player/Season messages
are unused and must be dropped during the move.

The Domain coupling that remains is entirely in **handler return types**, not
messages: Season handlers return `Season`/`List<Season>`/`List<Player>` and the
controller maps to DTOs via `ToDto()`. Handlers stay in `Application`, so this
coupling is allowed and unchanged. No message needs to stay behind.

## Data Flow

No data flow changes — this is a structural refactor. The runtime request path
is identical before and after:

```
Client (Application.IO DTOs)  ->  WebApi Controller
   ->  IMessageBus.InvokeAsync(message from Application.IO)
   ->  Wolverine -> Handler (Application) -> Repository (Infrastructure / EF)
   ->  Domain entity -> ToDto() mapper (Application) -> DTO (Application.IO)
   ->  Controller -> Client
```

The only behavioural-adjacent risk is Wolverine handler discovery (see ADR
above); everything else is namespace and file-location churn.

## File Changes Overview

**Renamed projects (via `dotnet sln remove` + `dotnet sln add`, plus folder/`.csproj` rename):**

- `src/Winterplein.Shared` -> `src/Winterplein.Application.IO`
- `src/Winterplein.Api` -> `src/Winterplein.WebApi`

**Restructured in place:**

- `src/Winterplein.Application` — new folders `CommandHandlers/`, `QueryHandlers/`,
  `Ports/`, `Mappers/` (kept); delete `Commands/`, `Queries/`, `Seasons/`,
  `Interfaces/`, `Services/`; add `IAmApplication.cs`.
- `src/Winterplein.Infrastructure` — folders for DbContext, configurations,
  repositories; keep `Migrations/`.

**Created test projects:**

- `tests/Winterplein.Domain.UnitTests`
- `tests/Winterplein.Application.UnitTests`
- `tests/Winterplein.Infrastructure.UnitTests`
- `tests/Winterplein.WebApi.UnitTests`
- `tests/Winterplein.Common.UnitTests` (renamed from `UnitTests.Common`)

**Deleted:**

- `tests/Winterplein.UnitTests` (split out)

**Modified (reference / namespace sweeps):**

- All `.csproj` ProjectReferences pointing at renamed projects.
- All `using Winterplein.Shared.DTOs` -> `using Winterplein.Application.IO.DTOs`
  (and message namespaces) across Application, WebApi, Client, tests.
- `Program.cs` (Wolverine discovery, namespace), `launchSettings.json` comments,
  CORS comments, `appsettings.json` if name-bound.
- `.slnx` via CLI only.

## Key Patterns Reused

- **Static Wolverine handlers** with `static Handle(message, ...deps)` — unchanged,
  only relocated into `CommandHandlers/` / `QueryHandlers/`.
- **Extension-method mappers** in `Mappers/` — unchanged.
- **Builder pattern** test helpers in `Common.UnitTests` — unchanged, project
  renamed.
- **Constructor-based xUnit + Moq + FluentAssertions** test classes — unchanged,
  redistributed across per-layer projects.
- **`WebApplicationFactory<Program>`** integration harness — unchanged, only the
  `Program` reference follows the WebApi rename.
