# Story 2 — Domain Models

**Epic:** Epic 1 — Match Generation
**Dependencies:** Story 1 (project setup)

## Description

Implement the core domain entities and the shared DTOs used to transfer data between the API and the Blazor client.

## Acceptance Criteria

### Domain enums and value objects

- **`Gender`** enum (`Male`, `Female`) in `Winterplein.Domain/Enums/Gender.cs`
- **`Name`** value object (`FirstName`, `LastName`) in `Winterplein.Domain/ValueObjects/Name.cs` — record or class with validation (neither property may be empty or whitespace)

### Domain entities (`Winterplein.Domain/Entities/`)

- **`Player`** — `Id` (int), `Name` (Name value object), `Gender` (Gender enum)
- **`Team`** — `Id` (int), `Player1`, `Player2` (Player references); implements `IEquatable<Team>` with order-independent equality: `{A, B} == {B, A}`
- **`Match`** — `Id` (int), `Team1` (Team), `Team2` (Team)

### Shared DTOs (`Winterplein.Shared/DTOs/`)

- **`PlayerDto`** — `Id` (int), `FirstName` (string), `LastName` (string), `Gender` (string)
- **`MatchDto`** — `Id` (int), `Team1Player1` (string), `Team1Player2` (string), `Team2Player1` (string), `Team2Player2` (string)
- **`AddPlayerRequest`** — `FirstName` (string), `LastName` (string), `Gender` (string)
- **`GenerateMatchesResponse`** — `Matches` (List\<MatchDto\>), `TotalCount` (int)

### Unit tests (`Winterplein.UnitTests/`)

- `Team {A, B}` equals `Team {B, A}`
- `Team {A, B}` does not equal `Team {A, C}`
- `GetHashCode` is consistent with equality
- `Name` throws when `FirstName` or `LastName` is empty or whitespace
- `Name` stores valid `FirstName` and `LastName` correctly

## Technical Notes

- Domain entities are plain C# classes with no framework dependencies
- DTOs are records or simple classes — no domain logic, only data
- `Team` hash code: `HashCode.Combine` on both player IDs unordered (e.g. XOR the two hashes or sort before combining)
- `Name` value object should be a record with constructor validation

## Tasks

- [ ] T1: Create `Gender` enum (`Male`, `Female`) and `Name` value object (`FirstName`, `LastName`) with validation
- [ ] T2: Create `Player` entity (`Id` int, `Name` Name, `Gender` Gender) (blocks: T1)
- [ ] T3: Create `Team` entity (`Id` int) with order-independent `IEquatable<Team>` (blocks: T2)
- [ ] T4: Create `Match` entity (`Id` int, `Team1`, `Team2`) (blocks: T3)
- [ ] T5: Create `PlayerDto` and `MatchDto` in `Winterplein.Shared/DTOs/`
- [ ] T6: Create `AddPlayerRequest` and `GenerateMatchesResponse` DTOs (blocks: T5)
- [ ] T7: Write Team equality and hash code unit tests (blocks: T3)
- [ ] T8: Write Name value object validation tests (blocks: T1)
