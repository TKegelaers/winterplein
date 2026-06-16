# migrate-existing-tests

## Scope

Move every existing integration test class onto `IntegrationTestBase` so they run against SQL Server with the clear-before-each-test hook, and add one seeded test that exercises the new seed builders.

- Update `PlayersControllerTests`, `MatchesControllerTests`, and `Seasons/SeasonsControllerTests` classes:
  - Inherit `IntegrationTestBase` instead of `IDisposable`.
  - Remove the per-class `new WinterpleinApiFactory()`, `CreateClient()`, and `Dispose()` boilerplate; use the factory/`HttpClient` provided by the base.
  - Keep all existing test bodies and API-driven state setup unchanged.
- Add a seeded test (in the seasons test file) that uses `SeasonSeedBuilder` + `PlayerSeedBuilder` to insert a season with enrolled players directly via EF Core, then calls `GET /api/seasons/{id}/players` and asserts the seeded players are returned — proving the seed-builder path and pre-existing state.
- Run the full suite against `Winterplein_integrationTests` and confirm all tests pass.

## Domain model changes

None.

## Test cases

- PlayersControllerTests, MatchesControllerTests, Seasons/SeasonsControllerTests
  - all existing tests pass unchanged on SQL Server
- Seasons/SeasonsControllerTests
  - GetPlayers_Returns200_WithSeededPlayers (new, via seed builders)

## Affected files

- modify: tests/Winterplein.IntegrationTests/PlayersControllerTests.cs
- modify: tests/Winterplein.IntegrationTests/MatchesControllerTests.cs
- modify: tests/Winterplein.IntegrationTests/Seasons/SeasonsControllerTests.cs
