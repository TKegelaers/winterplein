# 05-split-test-projects

## Scope

Split the monolithic `Winterplein.UnitTests` into per-layer test projects and
rename `Winterplein.UnitTests.Common` to `Winterplein.Common.UnitTests`, keeping
the existing `Winterplein.IntegrationTests`. Every existing test must still pass.

Create (via `dotnet new xunit` + `dotnet sln add`):

- `tests/Winterplein.Domain.UnitTests` — refs `Domain` + `Common.UnitTests`.
  Move `UnitTests/Domain/*` and `UnitTests/Seasons/SeasonDomainTests.cs`.
- `tests/Winterplein.Application.UnitTests` — refs `Application`, `Application.IO`,
  `Domain`, `Common.UnitTests`. Move `UnitTests/Application/*` and
  `UnitTests/Seasons/SeasonHandlerTests.cs`.
- `tests/Winterplein.Infrastructure.UnitTests` — refs `Infrastructure`,
  `Application`, `Domain`, `Common.UnitTests`, EF InMemory package.
  Move `UnitTests/Infrastructure/*`.
- `tests/Winterplein.WebApi.UnitTests` — refs `WebApi`, `Application.IO`,
  `Common.UnitTests`. Move `UnitTests/Api/*`.
- `tests/Winterplein.Common.UnitTests` — rename of `UnitTests.Common` (builders).

Each new project gets the standard package set (xUnit, Moq where needed,
FluentAssertions) and global `Using` entries matching the current `UnitTests.csproj`.
Update namespaces of moved test files. Delete `tests/Winterplein.UnitTests` once
empty. Update `Winterplein.slnx` via `dotnet sln`.

## Domain model changes (optional)

No domain model changes. Test-file relocation and project split only.

## Test cases

The moved tests are themselves the verification — every existing test must pass
in its new project:

- `Winterplein.Domain.UnitTests`: MatchTests, NameTests, PlayerTests, TeamTests,
  SeasonDomainTests
- `Winterplein.Application.UnitTests`: AddPlayer/RemovePlayer/GenerateMatches
  handler tests, GetAllPlayers/GetMatchCount query handler tests,
  MatchGeneratorServiceTests, Match/Player/Team mapper tests, SeasonHandlerTests
- `Winterplein.Infrastructure.UnitTests`: EfPlayerRepositoryTests, EfSeasonRepositoryTests
- `Winterplein.WebApi.UnitTests`: MatchesControllerTests, PlayersControllerTests

Guard:

- `dotnet build` green, no new warnings.
- `dotnet test` green across all six test projects — same test count as before
  the split (no tests lost).

## Affected files

- create: `tests/Winterplein.Domain.UnitTests/` (+ `.csproj`, `Usings.cs`)
- create: `tests/Winterplein.Application.UnitTests/` (+ `.csproj`, `Usings.cs`)
- create: `tests/Winterplein.Infrastructure.UnitTests/` (+ `.csproj`, `Usings.cs`)
- create: `tests/Winterplein.WebApi.UnitTests/` (+ `.csproj`, `Usings.cs`)
- rename: `tests/Winterplein.UnitTests.Common/` -> `tests/Winterplein.Common.UnitTests/`
- move: all files under `tests/Winterplein.UnitTests/{Domain,Application,Infrastructure,Api,Seasons}/`
  into the matching new projects (namespace updates)
- delete: `tests/Winterplein.UnitTests/`
- modify: `tests/Winterplein.IntegrationTests/Winterplein.IntegrationTests.csproj`
  (repoint `UnitTests.Common` -> `Common.UnitTests`, WebApi ref)
- modify: `Winterplein.slnx` (via `dotnet sln` only)
