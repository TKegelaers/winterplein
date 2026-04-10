# Story 2 — Convert Handlers to Wolverine Native Conventions

**Epic:** Epic 5 — Migrate from MediatR to Wolverine
**Dependencies:** Story 1 (shim layer must be in place before removing it)

## Description

Remove all MediatR shim interfaces and convert every handler to Wolverine's native convention: a static class with a static `Handle` method where the first parameter is the message type and additional parameters are injected by the Wolverine runtime. Commands and queries become plain records with no interface inheritance. This is a purely internal refactor — no API surface, no behavioral changes, and no test coverage gaps.

## Acceptance Criteria

### Commands & Queries (`Winterplein.Application/`)

All 13 command/query record files (8 commands + 5 queries):

- Remove `: IRequest<T>` / `: IRequest` inheritance from every record
- Remove `using Wolverine.Shims.MediatR;` (no longer needed)
- Records remain in the same files and namespaces — no moves

### Handlers (`Winterplein.Application/`)

All 13 handler files converted to the following pattern:

```csharp
// Before
public class GetAllPlayersQueryHandler(IPlayerRepository repo)
    : IRequestHandler<GetAllPlayersQuery, List<PlayerDto>>
{
    public Task<List<PlayerDto>> Handle(GetAllPlayersQuery request, CancellationToken ct) =>
        Task.FromResult(repo.GetAll().Select(p => p.ToDto()).ToList());
}

// After
public static class GetAllPlayersQueryHandler
{
    public static List<PlayerDto> Handle(GetAllPlayersQuery query, IPlayerRepository repo) =>
        repo.GetAll().Select(p => p.ToDto()).ToList();
}
```

- Class becomes `static`
- `Handle` method becomes `static`
- Constructor-injected dependencies move to additional `Handle` method parameters
- `CancellationToken cancellationToken` parameter removed (none of the 13 handlers use it)
- Return type changes from `Task<T>` to `T` directly (Wolverine supports sync handlers)
- `RemovePlayerCommand` handler returns `void` (was `Task`)
- Remove `using Wolverine.Shims.MediatR;`

### NuGet Packages

- `Winterplein.Application.csproj`: remove `WolverineFx` reference — Application project has zero framework dependencies after this story
- `Winterplein.Api.csproj`: retains `WolverineFx` (still needed for `builder.Host.UseWolverine()`)

### Handler Unit Tests (`tests/Winterplein.UnitTests/`)

All handler test files updated to invoke static methods directly:

- Replace `new XxxHandler(mock.Object).Handle(cmd, CancellationToken.None)` with `XxxHandler.Handle(cmd, mock.Object)`
- Remove `CancellationToken.None` arguments
- Remove `await` where handlers now return `T` synchronously

### Documentation

- `CLAUDE.md`: update "Tech stack" line — replace `MediatR` with `Wolverine`; update "Development Notes" — replace MediatR registration/pattern description with Wolverine native handler convention
- `ROADMAP.md`: update tech stack reference in the header
- Pending story files in `.tasks/epic2-*`, `.tasks/epic3-*`, `.tasks/epic4-*`: update any technical notes that reference "MediatR commands/queries" or `IRequestHandler` to reference Wolverine native handlers

### Build & Test

- `dotnet build` succeeds — no `MediatR` or `Wolverine.Shims` references anywhere
- `dotnet test` — all unit and integration tests pass
- Running `Select-String -Recurse -Pattern "using MediatR|Wolverine\.Shims" src/ tests/` returns zero hits

## Technical Notes

- Wolverine discovers handlers by convention: any public (or static) class with a public method named `Handle`, `HandleAsync`, `Consume`, or `ConsumeAsync` where the first parameter is a message type — no interface or registration required
- Static handler classes with method-level DI avoid per-dispatch object allocation; Wolverine resolves method parameters from the DI container at code-generation time
- All 13 current handlers wrap synchronous in-memory calls in `Task.FromResult` — dropping to direct sync returns is cleaner; Wolverine wraps sync handlers transparently for the Wolverine runtime
- After removing `WolverineFx` from `Winterplein.Application.csproj`, the Application project depends only on `Winterplein.Domain` and `Winterplein.Shared` — zero framework dependencies, consistent with Clean Architecture intent
- Future handlers written for Epics 2–4 should follow this same convention (static class, static `Handle`, method injection) rather than the old MediatR pattern

## Tasks

- [ ] T1: Convert 5 Player/Match handlers to native convention — static class, static `Handle`, method-injected deps, sync return (`AddPlayerCommandHandler`, `RemovePlayerCommandHandler`, `GenerateMatchesCommandHandler`, `GetAllPlayersQueryHandler`, `GetMatchCountQueryHandler`) (blockedBy: Story 1 T7)
- [ ] T2: Remove `: IRequest<T>` from 5 Player/Match command/query records (blockedBy: T1)
- [ ] T3: Convert 8 Season handlers to native convention (`CreateSeasonCommandHandler`, `UpdateSeasonCommandHandler`, `DeleteSeasonCommandHandler`, `GetSeasonsQueryHandler`, `GetSeasonByIdQueryHandler`, `AddSeasonPlayerCommandHandler`, `RemoveSeasonPlayerCommandHandler`, `GetSeasonPlayersQueryHandler`) (blockedBy: Story 1 T7)
- [ ] T4: Remove `: IRequest<T>` from 8 Season command/query records (blockedBy: T3)
- [ ] T5: Remove `WolverineFx` package reference from `Winterplein.Application.csproj` (blockedBy: T2, T4)
- [ ] T6: Update Player/Match handler unit tests — static method calls, remove `CancellationToken` (blockedBy: T1)
- [ ] T7: Update Season handler unit tests — static method calls, remove `CancellationToken` (blockedBy: T3)
- [ ] T8: `dotnet build` + `dotnet test` — verify all green; confirm no MediatR/shim references remain (blockedBy: T5, T6, T7)
- [ ] T9: Update `CLAUDE.md`, `ROADMAP.md`, and pending Epic 2–4 story files to reference Wolverine native handlers (blockedBy: T8)
