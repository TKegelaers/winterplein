# match-generation-service

## Problem Statement

The core business logic for generating all possible doubles matches from a player list does not exist. The algorithm must produce C(N,4) groups × 3 unique team pairings per group — for 10 players this yields 630 matches.

## Proposed Solution

Implement `IMatchGeneratorService` with `GenerateAllMatches(IReadOnlyList<Player>)` and `CalculateMatchCount(int)` in `Winterplein.Application`. Also define `IPlayerRepository` interface at this layer.

## Business Requirements

**Given** N enrolled players
**When** matches are generated
**Then** the result contains exactly C(N,4) × 3 unique matches

## Acceptance Criteria

- [ ] `IMatchGeneratorService` interface in `Winterplein.Application/Interfaces/`
- [ ] `MatchGeneratorService` implementation: returns empty list for < 4 players; produces exactly 3 pairings per group of 4; sequential match numbers from 1
- [ ] `IPlayerRepository` interface: `GetAll()`, `Add(Player)`, `Remove(Guid id)`
- [ ] Unit tests verify match counts for 3/4/6/8/10 players; no duplicates; all match numbers unique and start at 1
- [ ] `CalculateMatchCount(N)` returns `C(N,4) × 3` without generating matches

## Technical Notes

- Algorithm: 4-nested loop over player indices, yield 3 pairings per group: (i&j vs k&l), (i&k vs j&l), (i&l vs j&k)
- `CalculateMatchCount` uses direct combinatorial formula, not the generation loop
