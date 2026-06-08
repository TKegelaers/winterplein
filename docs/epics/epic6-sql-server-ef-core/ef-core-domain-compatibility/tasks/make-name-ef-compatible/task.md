# make-name-ef-compatible

## Scope

Make the `Name` record materializable by EF Core.

- Add a `private Name()` parameterless constructor with `FirstName = null!; LastName = null!`
- Change `FirstName` and `LastName` from `{ get; }` to `{ get; init; }`

## Domain model changes

No structural change — same public API, same record semantics.

## Test cases

No new tests required. All existing `Name`-related unit tests must continue to pass unchanged.

## Affected files

- modify: `src/Winterplein.Domain/ValueObjects/Name.cs`
