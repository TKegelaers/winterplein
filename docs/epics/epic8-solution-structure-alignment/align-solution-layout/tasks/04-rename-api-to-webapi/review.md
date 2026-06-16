# Review — 04-rename-api-to-webapi

## Verdict: Pass with minor warnings

The host project rename `Winterplein.Api` -> `Winterplein.WebApi` is complete and
correct. Build is green; full suite passes (88 unit + 36 integration = 124).
The findings below are cleanup items, not correctness or completeness failures.

## Completeness — Pass

- Folder `src/Winterplein.WebApi/`, `Winterplein.WebApi.csproj`, root namespace and
  assembly name all renamed. `Microsoft.NET.Sdk.Web` SDK; default root namespace and
  assembly name derive from the `.csproj` filename (`Winterplein.WebApi`).
- `.slnx` lists `src/Winterplein.WebApi/Winterplein.WebApi.csproj`; git status shows the
  rename was tracked via move (`RM`), consistent with `dotnet sln` CLI usage. `.slnx` not
  hand-edited for stale names.
- `Configuration/IocConfig.cs` added; `Program.cs` thinned to builder/pipeline calls plus
  `public partial class Program { }`. IoC wiring (controllers, Swagger, exception handler,
  Wolverine discovery via `IAmApplication`, EF DbContext, repositories, services, CORS)
  extracted with identical behaviour.
- `Controllers/` and `ExceptionHandling/` retained; namespaces are `Winterplein.WebApi.*`.
- Both test projects repoint to `..\..\src\Winterplein.WebApi\Winterplein.WebApi.csproj`.
  `WinterpleinApiFactory : WebApplicationFactory<Program>` resolves the WebApi assembly's
  global `Program` partial class correctly.
- `launchSettings.json` unchanged ports (5095 / 7108).

## Correctness — Pass

- `dotnet test`: 88 unit + 36 integration, 0 failed.
- CORS origin for the Blazor client `http://localhost:5149` is present in
  `appsettings.Development.json` `AllowedOrigins` (alongside `5173`); production origins live
  in `appsettings.json`. CORS policy `AllowClient` wired in `IocConfig`. Configured and intact.
- No stale `Winterplein.Api` references in any `.cs`, `.csproj`, `.json`, `.http`, `.slnx`,
  or `launchSettings` under `src/` or `tests/`.

## Warnings (should fix)

1. **Stray merge-conflict artifact committed in the renamed project.**
   `src/Winterplein.WebApi/appsettings.json.orig` is git-tracked (renamed in along with the
   folder) and still contains unresolved conflict markers (`<<<<<<< HEAD`, `=======`,
   `>>>>>>>`). This task moved the folder, so it carried the junk file into the new project.
   It should be deleted (`git rm`) and ideally `*.orig` added to `.gitignore`.

2. **Leftover sweep scripts at repo root.**
   `sweep.ps1` and `sweep2.ps1` (untracked) appear to be working artifacts from this rename's
   namespace sweep. They should be removed before the change is merged.

## Out of scope (noted, not part of this task's affected-files list)

- `.github/workflows/main_winterplein-api.yml` still builds/publishes
  `src/Winterplein.Api/Winterplein.Api.csproj` — will break the Azure deploy pipeline. Not
  listed in this task's affected files; flag for a follow-up task or epic-wide sweep.
- `CLAUDE.md` still references `Winterplein.Api` (docs); not in this task's scope.
