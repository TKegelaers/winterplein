# Story 1 — Replace MediatR with Wolverine Using Shims

**Epic:** Epic 5 — Migrate from MediatR to Wolverine
**Dependencies:** None (all Epic 1 stories are Done; modifies only completed code)

## Description

Swap the MediatR NuGet package for WolverineFx and update all DI registration, controller dispatch, and handler `using` directives to use Wolverine's MediatR shim layer (`Wolverine.Shims.MediatR`). All 13 existing handler signatures remain unchanged — only namespace imports and the host registration change. This creates a safe, running checkpoint on Wolverine before the native handler conversion in Story 2.

## Acceptance Criteria

### NuGet Packages

- `Winterplein.Application.csproj`: remove `MediatR 14.1.0`, add `WolverineFx`
- `Winterplein.Api.csproj`: add `WolverineFx` (for `UseWolverine()` host extension)

### DI Registration (`Winterplein.Api/Program.cs`)

- Remove `builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllPlayersQuery).Assembly));`
- Add `builder.Host.UseWolverine(opts => opts.Discovery.IncludeAssembly(typeof(GetAllPlayersQuery).Assembly));`
- Remove `using MediatR;` import, add `using Wolverine;`

### Commands & Queries (`Winterplein.Application/`)

All 13 command/query record files (8 commands + 5 queries):

- Replace `using MediatR;` with `using Wolverine.Shims.MediatR;`
- Record signatures (`: IRequest<T>`) remain unchanged

### Handlers (`Winterplein.Application/`)

All 13 handler files:

- Replace `using MediatR;` with `using Wolverine.Shims.MediatR;`
- Handler class signatures (`: IRequestHandler<TRequest, TResponse>`) remain unchanged

### Controllers (`Winterplein.Api/Controllers/`)

`PlayersController`, `MatchesController`, `SeasonsController`:

- Constructor parameter: `ISender sender` → `IMessageBus bus`
- All `sender.Send(new XxxCommand(...))` calls → `bus.InvokeAsync<TResponse>(new XxxCommand(...))`
- Void-returning commands (e.g. `RemovePlayerCommand`): `sender.Send(...)` → `bus.InvokeAsync(...)`
- Replace `using MediatR;` with `using Wolverine;`

### Unit Tests (`tests/Winterplein.UnitTests/Api/`)

`PlayersControllerTests`, `MatchesControllerTests`:

- Replace `Mock<ISender> _sender` with `Mock<IMessageBus> _bus`
- Update all `.Setup(s => s.Send(...))` calls to `.Setup(b => b.InvokeAsync<T>(...))`
- Update all `.Verify(s => s.Send(...))` calls accordingly
- Replace `using MediatR;` with `using Wolverine;`

### Build & Test

- `dotnet build` succeeds with zero errors
- `dotnet test` — all existing unit and integration tests pass

## Technical Notes

- Wolverine's MediatR shims (`IRequest<T>`, `IRequestHandler<TRequest, TResponse>`) are included in the core `WolverineFx` NuGet package — no separate shim package is needed
- `IMessageBus` is Wolverine's equivalent of MediatR's `ISender` — it is automatically registered in DI by `UseWolverine()`
- `IMessageBus.InvokeAsync<T>(message)` = `ISender.Send<T>(message)`; for void-returning commands, use `IMessageBus.InvokeAsync(message)` (no type parameter)
- Handlers live in `Winterplein.Application`, not the entry assembly — `opts.Discovery.IncludeAssembly(typeof(GetAllPlayersQuery).Assembly)` is required for Wolverine to discover them
- `RemovePlayerCommand` implements `IRequest` (no return type) — the shim supports this; the handler returns `Task`, not `Task<Unit>`
- Integration tests call HTTP endpoints and do not reference `ISender` or `IMessageBus` — they require no changes

## Tasks

- [ ] T1: Remove `MediatR` from `Winterplein.Application.csproj`, add `WolverineFx`; add `WolverineFx` to `Winterplein.Api.csproj`
- [ ] T2: Update `Program.cs` — remove `AddMediatR`, add `builder.Host.UseWolverine(opts => opts.Discovery.IncludeAssembly(...))` (blockedBy: T1)
- [ ] T3: Update `using` directives in all 13 command/query files — `using MediatR;` → `using Wolverine.Shims.MediatR;` (blockedBy: T1)
- [ ] T4: Update `using` directives in all 13 handler files — `using MediatR;` → `using Wolverine.Shims.MediatR;` (blockedBy: T1)
- [ ] T5: Update `PlayersController`, `MatchesController`, `SeasonsController` — `ISender` → `IMessageBus`, `.Send()` → `.InvokeAsync()` / `.InvokeAsync<T>()` (blockedBy: T2)
- [ ] T6: Update `PlayersControllerTests` and `MatchesControllerTests` — `Mock<ISender>` → `Mock<IMessageBus>`, update Setup/Verify calls (blockedBy: T5)
- [ ] T7: `dotnet build` + `dotnet test` — verify all green (blockedBy: T3, T4, T5, T6)
