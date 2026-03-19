# Story 2 — Domain Models

**Epic:** Epic 1 — Match Generation
**Dependencies:** Story 1 (project setup)

## Description

Implement the core domain entities and the shared DTOs used to transfer data between the API and the Blazor client.

## Acceptance Criteria

### Domain entities (`Winterplein.Domain/Entities/`)

- **`Player`** — `Id` (Guid), `Name` (string)
- **`Team`** — `Player1`, `Player2` (Player references); implements `IEquatable<Team>` with order-independent equality: `{A, B} == {B, A}`
- **`Match`** — `MatchNumber` (int), `Team1` (Team), `Team2` (Team)

### Shared DTOs (`Winterplein.Shared/DTOs/`)

- **`PlayerDto`** — `Id` (Guid), `Name` (string)
- **`MatchDto`** — `MatchNumber` (int), `Team1Player1` (string), `Team1Player2` (string), `Team2Player1` (string), `Team2Player2` (string)
- **`AddPlayerRequest`** — `Name` (string)
- **`GenerateMatchesResponse`** — `Matches` (List\<MatchDto\>), `TotalCount` (int)

### Unit tests (`Winterplein.UnitTests/`)

- `Team {A, B}` equals `Team {B, A}`
- `Team {A, B}` does not equal `Team {A, C}`
- `GetHashCode` is consistent with equality

## Technical Notes

- Domain entities are plain C# classes with no framework dependencies
- DTOs are records or simple classes — no domain logic, only data
- `Team` hash code: `HashCode.Combine` on both player IDs unordered (e.g. XOR the two hashes or sort before combining)

## Tasks

- [ ] T1: Create `Player` entity (`Id`, `Name`)
- [ ] T2: Create `Team` entity with order-independent `IEquatable<Team>` (blocks: T1)
- [ ] T3: Create `Match` entity (`MatchNumber`, `Team1`, `Team2`) (blocks: T2)
- [ ] T4: Create `PlayerDto` and `MatchDto` in `Winterplein.Shared/DTOs/`
- [ ] T5: Create `AddPlayerRequest` and `GenerateMatchesResponse` DTOs (blocks: T4)
- [ ] T6: Write Team equality and hash code unit tests (blocks: T2)
