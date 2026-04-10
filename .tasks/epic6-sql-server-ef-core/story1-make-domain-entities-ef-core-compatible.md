# Story 1 — Make Domain Entities EF Core Compatible

**Epic:** Epic 6 — SQL Server Persistence with EF Core
**Dependencies:** None (domain-only changes; no other epic required)

## Description

Add private parameterless constructors and change get-only properties to `{ get; private set; }` on all domain entities so EF Core can materialize them during database reads. The public API of every entity remains identical — no constructor signature changes, no new public members, no behavioral changes. This is a prerequisite for Stories 2, 3, and 4 in this epic.

## Acceptance Criteria

### Domain Entities (`Winterplein.Domain/Entities/`)

**`Player`**

- Add `private Player() { }` parameterless constructor with `= null!` on `Name`
- Change `Id`, `Name`, `Gender` from `{ get; }` to `{ get; private set; }`
- Existing public constructor and `ArgumentNullException` on `Name` unchanged

**`Season`**

- Add `private Season() { }` parameterless constructor with `= null!` on `Name`
- Change `Id`, `Name`, `StartDate`, `EndDate`, `Weekday`, `StartHour`, `EndHour` from `{ get; }` to `{ get; private set; }`
- Change `_players` field initialization from `= []` to `= new List<Player>()`
- Existing public constructor with all validation logic (`EndDate > StartDate`, `EndHour > StartHour`, non-empty `Name`) unchanged

**`Match`**

- Add `private Match() { }` parameterless constructor with `= null!` on `Team1` and `Team2`
- Change `Id`, `Team1`, `Team2` from `{ get; }` to `{ get; private set; }`
- Existing public constructor unchanged

**`Team`**

- Add `private Team() { }` parameterless constructor with `= null!` on `Player1` and `Player2`
- Change `Id`, `Player1`, `Player2` from `{ get; }` to `{ get; private set; }`
- Existing public constructor unchanged

### Value Objects (`Winterplein.Domain/ValueObjects/`)

**`Name`**

- Add `private Name() { }` parameterless constructor with `= null!` on `FirstName` and `LastName`
- Change `FirstName`, `LastName` from `{ get; }` to `{ get; init; }` (`init` is idiomatic for records)
- Existing public constructor with validation unchanged

### Build & Test

- `dotnet build` succeeds with no new warnings
- `dotnet test` — all existing unit and integration tests pass unchanged (no public API altered)

## Technical Notes

- Use `= null!` (null-forgiving default) in private parameterless constructors for non-nullable reference type properties — this is cleaner than `#pragma warning disable CS8618` and signals intentional EF Core usage
- Use `private set` on class entities (`Player`, `Season`, `Match`, `Team`); use `init` on the `Name` record — records are conventionally immutable and `init` preserves that semantic
- The `Season._players` backing field **must** be initialized to `new List<Player>()` (not `[]`) in the parameterless constructor — EF Core populates navigation collections via the backing field and requires a concrete `List<T>` instance
- These changes are purely additive from a caller perspective — all existing code that constructs or reads entities continues to work without modification

## Tasks

- [ ] T1: Add `private Player() { }` and `{ get; private set; }` to `Player` entity
- [ ] T2: Add `private Season() { }` and `{ get; private set; }` to `Season` entity; change `_players` init to `new List<Player>()`
- [ ] T3: Add `private Match() { }` and `{ get; private set; }` to `Match` entity
- [ ] T4: Add `private Team() { }` and `{ get; private set; }` to `Team` entity
- [ ] T5: Add `private Name() { }` and `{ get; init; }` to `Name` record
- [ ] T6: `dotnet build` + `dotnet test` — confirm no regressions (blockedBy: T1, T2, T3, T4, T5)
