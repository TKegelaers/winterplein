# update-inmemory-repositories

## Scope

Update both in-memory repository implementations to satisfy the new async interfaces by wrapping synchronous logic in `Task.FromResult` (or `Task.CompletedTask` for void returns). No actual async I/O is introduced.

`InMemoryPlayerRepository`:

- `GetAllAsync`: wrap `GetAll` logic with `Task.FromResult`
- `GetByIdAsync`: wrap `GetById` logic with `Task.FromResult`
- `CountAsync`: wrap `Count` logic with `Task.FromResult`
- `AddAsync(Player player, ...)`: accept a `Player`, assign next auto-increment ID to a new `Player` instance, return via `Task.FromResult`
- `RemoveAsync`: wrap `Remove` logic, return `Task.CompletedTask`

`InMemorySeasonRepository`:

- All methods: wrap existing logic with `Task.FromResult` / `Task.CompletedTask`
- `UpdateAsync`: return the updated `Season` (not bool); throw `KeyNotFoundException` if not found
- `DeleteAsync`: return `Task.CompletedTask`; throw `KeyNotFoundException` if not found

## Domain model changes

None.

## Test cases

No dedicated unit tests for the in-memory repos. Build success and passing integration tests are sufficient.

## Affected files

- modify: `src/Winterplein.Infrastructure/Persistence/InMemoryPlayerRepository.cs`
- modify: `src/Winterplein.Infrastructure/Persistence/InMemorySeasonRepository.cs`
