# Story 3 — Match Generation Service

**Epic:** Epic 1 — Match Generation
**Dependencies:** Story 2 (domain models)

## Description

Implement the core business logic that generates all possible doubles matches from a player list. The algorithm produces C(N,4) groups of 4 players, then splits each group into 3 unique team pairings.

## Acceptance Criteria

### Interface (`Winterplein.Application/Interfaces/IMatchGeneratorService.cs`)

```csharp
public interface IMatchGeneratorService
{
    List<Match> GenerateAllMatches(IReadOnlyList<Player> players);
    int CalculateMatchCount(int playerCount);
}
```

### Implementation (`Winterplein.Application/Services/MatchGeneratorService.cs`)

- Returns empty list when fewer than 4 players are provided
- For each group of 4 players `{A, B, C, D}`, produces exactly 3 matches:
  - `AB vs CD`
  - `AC vs BD`
  - `AD vs BC`
- Match numbers are sequential starting at 1
- Service registered in DI (`AddScoped` or `AddSingleton`)

### Player repository interface (`Winterplein.Application/Interfaces/IPlayerRepository.cs`)

```csharp
public interface IPlayerRepository
{
    IReadOnlyList<Player> GetAll();
    void Add(Player player);
    void Remove(Guid id);
}
```

### Unit tests (`Winterplein.UnitTests/`)

| Players | Expected matches |
|---------|-----------------|
| 3       | 0               |
| 4       | 3               |
| 6       | 45              |
| 8       | 210             |
| 10      | 630             |

- No duplicate matches in output
- All match numbers are unique and start at 1
- `CalculateMatchCount(N)` returns `C(N,4) * 3` without generating matches

## Technical Notes

```
C(N,4) = N! / (4! * (N-4)!)

Algorithm:
for i in 0..N-4:
  for j in i+1..N-3:
    for k in j+1..N-2:
      for l in k+1..N-1:
        group = {players[i], players[j], players[k], players[l]}
        yield (i&j vs k&l), (i&k vs j&l), (i&l vs j&k)
```

## Tasks

- [x] T1: Define `IMatchGeneratorService` interface
- [x] T2: Define `IPlayerRepository` interface
- [x] T3: Implement `MatchGeneratorService` with C(N,4)×3 algorithm (blocks: T1)
- [x] T4: Implement `CalculateMatchCount` without generating matches (blocks: T1)
- [x] T5: Write unit tests for match counts (3/4/6/8/10 players) (blocks: T3, T4)
- [x] T6: Write no-duplicate-matches test (blocks: T3)
