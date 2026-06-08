# make-player-ef-compatible

## Scope

Make the `Player` entity materializable by EF Core.

- Add a `private Player()` parameterless constructor with `Name = null!`
- Change `Id`, `Name`, `Gender` from `{ get; }` to `{ get; private set; }`

## Domain model changes

No structural change — same public constructor and property signatures.

## Test cases

No new tests required. All existing `Player`-related unit tests must continue to pass unchanged.

## Affected files

- modify: `src/Winterplein.Domain/Entities/Player.cs`
