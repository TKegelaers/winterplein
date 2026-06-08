# create-e2e-test-project

## Scope

Create a new xUnit test project `Winterplein.E2eTests` targeting net10.0 and register it in the solution:

- Run `dotnet new xunit` in `tests/Winterplein.E2eTests/`
- Add the project to `Winterplein.slnx` under the `/tests/` folder
- No references to other Winterplein projects
- Set `IsPackable=false` in the csproj

## Domain model changes

None.

## Test cases

None — this is scaffolding only. Verify the project builds with `dotnet build tests/Winterplein.E2eTests`.

## Affected files

- create: `tests/Winterplein.E2eTests/Winterplein.E2eTests.csproj`
- create: `tests/Winterplein.E2eTests/UnitTest1.cs` (generated placeholder — deleted or replaced in later tasks)
- modify: `Winterplein.slnx`
