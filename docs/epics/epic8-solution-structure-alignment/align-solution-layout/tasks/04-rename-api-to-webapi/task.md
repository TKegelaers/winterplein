# 04-rename-api-to-webapi

## Scope

Rename the host project `Winterplein.Api` to `Winterplein.WebApi` and add a
`Configuration/` folder for startup/IoC wiring, keeping `Controllers/`.

- Rename project folder, `.csproj`, assembly, and root namespace `Winterplein.Api`
  -> `Winterplein.WebApi` via `dotnet sln remove` / `dotnet sln add`.
- Add `Configuration/` folder; extract IoC/startup wiring from `Program.cs`
  (Wolverine, DbContext, repository/service registrations, CORS) into a
  configuration class (e.g. `Configuration/IocConfig.cs`) per the reference,
  leaving `Program.cs` thin. Keep behaviour identical.
- Keep `Controllers/` and `ExceptionHandling/` (or relocate per reference).
- Update `launchSettings.json` (ports unchanged: 5095/7108) and any name-bound
  comments (CORS origin comment referencing the client).
- Sweep all `using Winterplein.Api.*` -> `using Winterplein.WebApi.*` and the
  `namespace Winterplein.Api.*` declarations.
- Repoint test project references from `Winterplein.Api` to `Winterplein.WebApi`
  (UnitTests and IntegrationTests) — they consume `public partial class Program`.

## Domain model changes (optional)

No domain model changes. Project/namespace rename and `Program.cs` decomposition only.

## Test cases

No new tests. Guard:

- `dotnet build` green, no new warnings.
- `dotnet test` green — `WebApplicationFactory<Program>` integration tests and
  controller unit tests confirm the host still boots, Swagger lists all endpoints,
  CORS and exception handling are unchanged.
- Manual: run API + client, verify player/match/season flows unchanged.

## Affected files

- rename: `src/Winterplein.Api/` -> `src/Winterplein.WebApi/` (folder + `.csproj` + assembly)
- create: `src/Winterplein.WebApi/Configuration/IocConfig.cs` (extracted wiring)
- modify: `src/Winterplein.WebApi/Program.cs` (thinned, namespace)
- modify: `src/Winterplein.WebApi/Controllers/*.cs`, `ExceptionHandling/*.cs` (namespace)
- modify: `src/Winterplein.WebApi/Properties/launchSettings.json` (comments only; ports same)
- modify: `tests/Winterplein.UnitTests/*` and `tests/Winterplein.IntegrationTests/*`
  — `using`/namespace + `.csproj` ProjectReference repoint
- modify: `Winterplein.slnx` (via `dotnet sln` only)
