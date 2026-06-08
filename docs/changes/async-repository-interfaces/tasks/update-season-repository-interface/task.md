# update-season-repository-interface

## Scope

Redefine `ISeasonRepository` to use fully async method signatures with `CancellationToken` parameters.

- Replace `IReadOnlyList<Season> GetAll()` → `Task<IReadOnlyList<Season>> GetAllAsync(CancellationToken ct = default)`
- Replace `Season? GetById(int id)` → `Task<Season?> GetByIdAsync(int id, CancellationToken ct = default)`
- Replace `Season Add(Season season)` → `Task<Season> AddAsync(Season season, CancellationToken ct = default)`
- Replace `bool Update(Season season)` → `Task<Season> UpdateAsync(Season season, CancellationToken ct = default)`
- Replace `bool Delete(int id)` → `Task DeleteAsync(int id, CancellationToken ct = default)`

Note: `UpdateAsync` and `DeleteAsync` return `Season` and `void` respectively rather than `bool` — callers throw on not-found rather than inspecting a bool.

## Domain model changes

None. Interface change only.

## Test cases

No new test cases. Compilation success after T2 (in-memory) and T4 (handlers) are the verification.

## Affected files

- modify: `src/Winterplein.Application/Interfaces/ISeasonRepository.cs`
