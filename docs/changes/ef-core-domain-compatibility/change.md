# ef-core-domain-compatibility

## Problem Statement

EF Core cannot materialize domain entities that use get-only properties and lack a parameterless constructor. All domain entities (`Player`, `Season`, `Match`, `Team`) and the `Name` value object currently use `{ get; }` properties and have no private parameterless constructors, which prevents EF Core from reading them back from the database.

## Proposed Solution

Add private parameterless constructors and change get-only properties to `{ get; private set; }` (or `{ get; init; }` for records) on all domain entities and value objects. The public API of every entity remains identical — no constructor signature changes, no new public members, no behavioral changes.

## Business Requirements

**Given** the domain entities and value objects exist with get-only properties
**When** EF Core attempts to materialize them from a database query
**Then** EF Core can construct and populate the entities without reflection errors

## Acceptance Criteria

- [ ] `Player`: private parameterless constructor added; `Id`, `Name`, `Gender` changed to `{ get; private set; }`
- [ ] `Season`: private parameterless constructor added; all scalar properties changed to `{ get; private set; }`; `_players` backing field initialized to `new List<Player>()`
- [ ] `Match`: private parameterless constructor added; `Id`, `Team1`, `Team2` changed to `{ get; private set; }`
- [ ] `Team`: private parameterless constructor added; `Id`, `Player1`, `Player2` changed to `{ get; private set; }`
- [ ] `Name` (record): private parameterless constructor added; `FirstName`, `LastName` changed to `{ get; init; }`
- [ ] `dotnet build` succeeds with no new warnings
- [ ] `dotnet test` — all existing unit and integration tests pass unchanged

## Potential Pitfalls

- `Season._players` backing field must be initialized to `new List<Player>()` (not `[]`) in the parameterless constructor — EF Core requires a concrete `List<T>` instance when populating navigation collections via backing fields
- Use `private set` on class entities and `init` on the `Name` record — `init` preserves the record's immutability semantic
- Use `= null!` in private parameterless constructors for non-nullable reference type properties — signals intentional EF Core usage without pragma suppressions
