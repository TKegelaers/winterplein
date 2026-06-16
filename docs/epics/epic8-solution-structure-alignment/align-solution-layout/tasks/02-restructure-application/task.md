# 02-restructure-application

## Scope

Restructure `Winterplein.Application` internals to match the reference folders.
Message types already left in task 01; this task organises what remains.

- Create `CommandHandlers/` and move all command handlers into it (one folder per
  command following the reference, e.g. `CommandHandlers/AddPlayer/AddPlayerCommandHandler.cs`).
- Create `QueryHandlers/` and move all query handlers into it.
- Collapse the flat `Seasons/` folder: its handlers split into `CommandHandlers/`
  and `QueryHandlers/` by message kind. The `Seasons/` folder must be gone.
- Rename `Interfaces/` -> `Ports/` and move `IPlayerRepository`, `ISeasonRepository`,
  `IMatchGeneratorService` into it (namespace `Winterplein.Application.Ports`).
- Keep `Mappers/` as-is (already matches the reference).
- Add `IAmApplication.cs` marker interface at the project root; repoint Wolverine
  discovery in the host to `typeof(IAmApplication).Assembly`.
- Sweep namespaces: `Winterplein.Application.Interfaces` -> `Winterplein.Application.Ports`,
  and the handler/folder namespace changes, across Application, Infrastructure, host, and tests.

## Domain model changes (optional)

No domain model changes. Folder/namespace reorganisation only:

```
Application/Interfaces/        -> Application/Ports/
Application/Commands/*/Handler -> Application/CommandHandlers/*/
Application/Queries/*/Handler  -> Application/QueryHandlers/*/
Application/Seasons/*Handler   -> Application/CommandHandlers/ + QueryHandlers/
Application/Services/          -> retained (MatchGeneratorService) or relocated per reference
```

## Test cases

No new tests. Guard:

- `dotnet build` green, no new warnings.
- `dotnet test` green — confirms Wolverine discovery via `IAmApplication` works
  and repository ports still resolve in DI.

## Affected files

- create: `src/Winterplein.Application/IAmApplication.cs`
- rename: `src/Winterplein.Application/Interfaces/` -> `Ports/` (3 interface files, namespace)
- move: command handlers -> `CommandHandlers/<Command>/`
- move: query handlers -> `QueryHandlers/`
- move: `Seasons/*Handler.cs` -> split into `CommandHandlers/` and `QueryHandlers/`
- delete: empty `Commands/`, `Queries/`, `Seasons/`, `Interfaces/` folders
- modify: `src/Winterplein.Infrastructure/*` repositories — `Ports` namespace using
- modify: `src/Winterplein.Api/Program.cs` — discovery type + `using` for Ports/Services
- modify: tests referencing `Winterplein.Application.Interfaces` / old handler namespaces
