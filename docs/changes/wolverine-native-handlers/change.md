# wolverine-native-handlers

## Problem Statement

All 13 handlers still use the MediatR shim interfaces (`IRequestHandler<TRequest, TResponse>`). The shim layer adds unnecessary indirection. Wolverine's native convention (static class, static `Handle` method, method-level dependency injection) is cleaner and removes the last framework dependency from `Winterplein.Application`.

## Proposed Solution

Convert all 13 handlers to the Wolverine native pattern. Remove `IRequest<T>` inheritance from all command/query records. Remove `WolverineFx` from `Winterplein.Application.csproj` — after this story the Application layer has zero framework dependencies.

## Business Requirements

**Given** all handlers use Wolverine native conventions
**When** the application runs
**Then** all functionality works identically — no behavioral or API surface changes

## Acceptance Criteria

- [ ] All 13 command/query records: remove `: IRequest<T>` / `: IRequest` inheritance; remove `using Wolverine.Shims.MediatR`
- [ ] All 13 handler files converted: class becomes `static`; `Handle` becomes `static`; constructor-injected deps move to method parameters; return type changes from `Task<T>` to `T`; `RemovePlayerCommand` handler returns `void`
- [ ] `WolverineFx` removed from `Winterplein.Application.csproj`
- [ ] Handler unit tests updated: `new XxxHandler(mock.Object).Handle(cmd, CancellationToken.None)` → `XxxHandler.Handle(cmd, mock.Object)`; `CancellationToken.None` removed; `await` removed where sync
- [ ] `dotnet build` and `dotnet test` — all green; no `MediatR` or `Wolverine.Shims` references in `src/` or `tests/`
- [ ] `CLAUDE.md` and `ROADMAP.md` updated to reference Wolverine native handler convention

## Technical Notes

- Wolverine discovers handlers by convention: static class with a static `Handle` method where the first parameter is the message type — no interface or registration needed
- All 13 current handlers wrap synchronous in-memory calls; dropping `Task.FromResult` to direct sync returns is cleaner; Wolverine wraps sync handlers transparently
- After removing `WolverineFx` from Application, that project depends only on `Winterplein.Domain` and `Winterplein.Shared` — zero framework dependencies
