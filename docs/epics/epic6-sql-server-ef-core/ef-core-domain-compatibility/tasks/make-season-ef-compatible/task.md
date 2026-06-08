# make-season-ef-compatible

## Scope

Make the `Season` entity materializable by EF Core.

- Add a `private Season()` parameterless constructor that initializes `_players = new List<Player>()` and sets `Name = null!`
- Change all scalar properties (`Id`, `Name`, `StartDate`, `EndDate`, `Weekday`, `StartHour`, `EndHour`) from `{ get; }` to `{ get; private set; }`
- Change `_players` backing field from `readonly` to non-readonly so EF Core can assign it via the backing field

## Domain model changes

No structural change — same public constructor, `AddPlayer`, `RemovePlayer`, and `GetMatchdays` remain unchanged.

## Test cases

No new tests required. All existing `Season`-related unit tests must continue to pass unchanged.

## Affected files

- modify: `src/Winterplein.Domain/Entities/Season.cs`
