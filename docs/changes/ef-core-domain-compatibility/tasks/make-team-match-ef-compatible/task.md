# make-team-match-ef-compatible

## Scope

Make the `Team` and `Match` entities materializable by EF Core.

For `Team`:

- Add a `private Team()` parameterless constructor with `Player1 = null!; Player2 = null!`
- Change `Id`, `Player1`, `Player2` from `{ get; }` to `{ get; private set; }`

For `Match`:

- Add a `private Match()` parameterless constructor with `Team1 = null!; Team2 = null!`
- Change `Id`, `Team1`, `Team2` from `{ get; }` to `{ get; private set; }`

## Domain model changes

No structural change — same public constructors, `IEquatable<Team>` implementation unchanged.

## Test cases

No new tests required. All existing `Team`- and `Match`-related unit tests must continue to pass unchanged.

## Affected files

- modify: `src/Winterplein.Domain/Entities/Team.cs`
- modify: `src/Winterplein.Domain/Entities/Match.cs`
