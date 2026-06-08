# update-handler-unit-tests

## Scope

Update all handler unit tests to match the new async signatures. Mock setups change from `.Returns()` to `.ReturnsAsync()`. Handler calls become `await Handler.Handle(...)` and test methods become `async Task`.

Tests to update in `tests/Winterplein.UnitTests/Application/Handlers/`:

- `AddPlayerCommandHandlerTests` — mock `AddAsync` with `.ReturnsAsync(player)`; verify `AddAsync` called with a `Player` (not `Name`/`Gender`)
- `RemovePlayerCommandHandlerTests` — mock `RemoveAsync` with `.Returns(Task.CompletedTask)`
- `GetAllPlayersQueryHandlerTests` — mock `GetAllAsync` with `.ReturnsAsync(...)`
- `GetMatchCountQueryHandlerTests` — mock `CountAsync` with `.ReturnsAsync(10)`
- `GenerateMatchesCommandHandlerTests` — mock `GetAllAsync` with `.ReturnsAsync(...)`

Tests to update in `tests/Winterplein.UnitTests/Seasons/SeasonHandlerTests`:

- All 14 test methods — replace `.Returns(...)` with `.ReturnsAsync(...)` on all season and player repo mocks; make test methods `async Task`

## Test cases

No new cases. All existing cases are preserved; only mock setup and await patterns change.

## Affected files

- modify: `tests/Winterplein.UnitTests/Application/Handlers/AddPlayerCommandHandlerTests.cs`
- modify: `tests/Winterplein.UnitTests/Application/Handlers/RemovePlayerCommandHandlerTests.cs`
- modify: `tests/Winterplein.UnitTests/Application/Handlers/GetAllPlayersQueryHandlerTests.cs`
- modify: `tests/Winterplein.UnitTests/Application/Handlers/GetMatchCountQueryHandlerTests.cs`
- modify: `tests/Winterplein.UnitTests/Application/Handlers/GenerateMatchesCommandHandlerTests.cs`
- modify: `tests/Winterplein.UnitTests/Seasons/SeasonHandlerTests.cs`
