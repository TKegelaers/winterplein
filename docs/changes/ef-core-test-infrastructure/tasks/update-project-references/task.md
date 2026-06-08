# update-project-references

## Scope

Add the NuGet package and project reference required for SQLite-based integration tests.

- Add `Microsoft.EntityFrameworkCore.Sqlite` NuGet package to `Winterplein.IntegrationTests.csproj`
- Add `<ProjectReference>` for `Winterplein.Infrastructure` to `Winterplein.IntegrationTests.csproj`

## Domain model changes (optional)

None.

## Test cases

No new tests — verify with `dotnet build tests/Winterplein.IntegrationTests` that the project compiles after the changes.

## Affected files

- modify: tests/Winterplein.IntegrationTests/Winterplein.IntegrationTests.csproj
