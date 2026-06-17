# 01-rename-shared-to-application-io

## Scope

Rename `Winterplein.Shared` to `Winterplein.Application.IO` and turn it into the
Domain-free contracts layer holding DTOs plus all CQRS command/query message
types. After this task the solution must build and all existing tests pass.

- Rename the project folder, `.csproj`, and assembly to `Winterplein.Application.IO`
  via `dotnet sln remove` / `dotnet sln add` (never hand-edit `.slnx`).
- Move the existing DTO files; change namespace `Winterplein.Shared.DTOs`
  -> `Winterplein.Application.IO.DTOs`.
- Move all 13 CQRS message types (commands + queries) out of `Winterplein.Application`
  into `Winterplein.Application.IO` under `Commands/` and `Queries/` folders.
  Drop the stray/unused `using Winterplein.Domain.*` and `using Winterplein.Shared.DTOs`
  directives during the move (see audit in design.md — none are load-bearing).
- Add `IAmApplicationIO.cs` marker interface at the project root.
- Repoint references: `Winterplein.Application`, `Winterplein.Client` and the host
  reference `Application.IO` instead of `Shared`. `Application.IO` must have **no**
  reference to `Domain`, `Application`, or `Infrastructure`.
- Sweep all `using Winterplein.Shared.DTOs` -> `using Winterplein.Application.IO.DTOs`
  and message-type namespaces across Application, Api, Client, and tests.

Note: handlers stay in `Winterplein.Application` and continue to return Domain
entities for the Season flow; only the message records move. Wolverine discovery
in `Program.cs` currently keys off `typeof(GetAllPlayersQuery).Assembly` — once
that type moves to Application.IO, discovery would scan the wrong assembly.
Temporarily point discovery at a type that stays in Application (e.g. a handler);
the permanent `IAmApplication` marker is introduced in task 02.

## Domain model changes (optional)

No domain model changes. Namespace move only:

```
Winterplein.Shared.DTOs.*            -> Winterplein.Application.IO.DTOs.*
Winterplein.Application.Commands.*   -> Winterplein.Application.IO.Commands.*
Winterplein.Application.Queries.*    -> Winterplein.Application.IO.Queries.*
Winterplein.Application.Seasons.*Command/*Query -> Winterplein.Application.IO.Commands / .Queries
```

## Test cases

No new tests. Behaviour-preservation guard is the existing suite:

- `dotnet build` is green with no new warnings.
- `dotnet test` is green (all existing unit + integration tests pass), confirming
  Wolverine still discovers handlers and message routing is unchanged.

## Affected files

- rename: `src/Winterplein.Shared/` -> `src/Winterplein.Application.IO/` (folder + `.csproj`)
- create: `src/Winterplein.Application.IO/IAmApplicationIO.cs`
- modify: all `src/Winterplein.Application.IO/DTOs/*.cs` (namespace)
- move: 13 message files from `src/Winterplein.Application/Commands|Queries|Seasons/`
  into `src/Winterplein.Application.IO/Commands|Queries/` (namespace + drop stray usings)
- modify: `src/Winterplein.Application/Winterplein.Application.csproj` (ref Application.IO)
- modify: `src/Winterplein.Client/Winterplein.Client.csproj` (Shared -> Application.IO)
- modify: `src/Winterplein.Api/Winterplein.Api.csproj` (Shared -> Application.IO)
- modify: `src/Winterplein.Api/Program.cs` (Wolverine discovery type, usings)
- modify: controllers, mappers, handlers, client services — `using` sweep
- modify: tests referencing `Winterplein.Shared.DTOs` — `using` sweep
- modify: `Winterplein.slnx` (via `dotnet sln` only)
