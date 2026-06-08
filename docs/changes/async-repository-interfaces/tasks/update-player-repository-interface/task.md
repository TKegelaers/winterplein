# update-player-repository-interface

## Scope

Redefine `IPlayerRepository` to use fully async method signatures with `CancellationToken` parameters.

- Replace `IReadOnlyList<Player> GetAll()` → `Task<IReadOnlyList<Player>> GetAllAsync(CancellationToken ct = default)`
- Replace `Player? GetById(int id)` → `Task<Player?> GetByIdAsync(int id, CancellationToken ct = default)`
- Replace `int Count { get; }` → `Task<int> CountAsync(CancellationToken ct = default)`
- Replace `Player Add(Name name, Gender gender)` → `Task<Player> AddAsync(Player player, CancellationToken ct = default)`
- Replace `void Remove(int id)` → `Task RemoveAsync(int id, CancellationToken ct = default)`

Note: `AddAsync` now accepts a `Player` (with `Id = 0`) instead of `Name`/`Gender`. The caller (handler) is responsible for constructing the `Player` before calling the repo.

## Domain model changes

None. Interface change only.

## Test cases

No new test cases. Compilation success after T2 (in-memory) and T3 (handlers) are the verification.

## Affected files

- modify: `src/Winterplein.Application/Interfaces/IPlayerRepository.cs`
