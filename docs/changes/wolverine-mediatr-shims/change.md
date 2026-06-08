# wolverine-mediatr-shims

## Problem Statement

The application uses MediatR for CQRS dispatch. MediatR's maintenance cadence and licensing model make it a long-term risk. Wolverine is the preferred replacement as it supports the same handler pattern through a shim layer, enabling a safe, incremental migration.

## Proposed Solution

Swap the MediatR NuGet package for WolverineFx across `Winterplein.Application` and `Winterplein.Api`. Use Wolverine's MediatR compatibility shim so all existing handler signatures (`IRequestHandler<TRequest, TResponse>`) remain unchanged. Update DI registration and controller dispatch. This creates a verified, running checkpoint on Wolverine before the full native conversion in Story 2.

## Business Requirements

**Given** MediatR is replaced with Wolverine shims
**When** the application runs
**Then** all existing functionality works identically — no behavioral changes

## Acceptance Criteria

- [ ] `MediatR` removed from `Winterplein.Application.csproj`; `WolverineFx` added to Application and Api
- [ ] `Program.cs`: `AddMediatR` replaced with `builder.Host.UseWolverine(opts => opts.Discovery.IncludeAssembly(...))`
- [ ] All 13 command/query records: `using MediatR` → `using Wolverine.Shims.MediatR`
- [ ] All 13 handler files: `using MediatR` → `using Wolverine.Shims.MediatR`
- [ ] Controllers: `ISender sender` → `IMessageBus bus`; `.Send()` → `.InvokeAsync()` / `.InvokeAsync<T>()`
- [ ] Controller unit tests: `Mock<ISender>` → `Mock<IMessageBus>`
- [ ] `dotnet build` and `dotnet test` — all green

## Technical Notes

- Wolverine's MediatR shims (`IRequest<T>`, `IRequestHandler<TRequest, TResponse>`) are included in `WolverineFx` — no separate package needed
- `IMessageBus.InvokeAsync<T>(message)` = `ISender.Send<T>(message)`; void commands use `InvokeAsync(message)` with no type parameter
- Handlers live in `Winterplein.Application` — `opts.Discovery.IncludeAssembly(typeof(GetAllPlayersQuery).Assembly)` is required for Wolverine to discover them
