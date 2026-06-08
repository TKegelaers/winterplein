# domain-models

## Problem Statement

The application has no domain model. Before any business logic can be implemented, the core entities (`Player`, `Team`, `Match`), value objects (`Name`), enums (`Gender`), and shared DTOs must exist.

## Proposed Solution

Implement the domain entities and value objects in `Winterplein.Domain`, and the shared DTOs used to transfer data between the API and the Blazor client in `Winterplein.Shared`.

## Business Requirements

**Given** domain entities exist
**When** the application constructs a `Team` with two players in any order
**Then** `{A, B}` equals `{B, A}` (order-independent equality)

**Given** a `Name` value object is constructed
**When** `FirstName` or `LastName` is empty or whitespace
**Then** an `ArgumentException` is thrown

## Acceptance Criteria

- [ ] `Gender` enum (`Male`, `Female`) in `Winterplein.Domain/Enums/`
- [ ] `Name` record in `Winterplein.Domain/ValueObjects/` with validation (neither property may be empty/whitespace)
- [ ] `Player` entity: `Id` (int), `Name` (Name), `Gender` (Gender)
- [ ] `Team` entity: `Id` (int), `Player1`, `Player2`; implements order-independent `IEquatable<Team>`
- [ ] `Match` entity: `Id` (int), `Team1` (Team), `Team2` (Team)
- [ ] `PlayerDto`, `MatchDto`, `AddPlayerRequest`, `GenerateMatchesResponse` in `Winterplein.Shared/DTOs/`
- [ ] Unit tests for `Team` equality, hash code, and `Name` validation

## Potential Pitfalls

- `Team` hash code must use unordered combine (e.g. XOR the two player ID hashes) so `{A,B}` and `{B,A}` produce the same hash
- DTOs are records (data-only, no domain logic) — domain entities are plain classes
