# add-ef-core-packages

## Scope

Add the required NuGet packages to the relevant projects:

- `Microsoft.EntityFrameworkCore.SqlServer` → `Winterplein.Infrastructure.csproj`
- `Microsoft.EntityFrameworkCore.Tools` → `Winterplein.Infrastructure.csproj`
- `Microsoft.EntityFrameworkCore.Design` → `Winterplein.Api.csproj`

## Domain model changes

None.

## Test cases

None — this is a project file change only.

## Affected files

- modify: `src/Winterplein.Infrastructure/Winterplein.Infrastructure.csproj`
- modify: `src/Winterplein.Api/Winterplein.Api.csproj`
