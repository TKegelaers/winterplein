# season-cqrs-repository

## Problem Statement

The `Season` domain entity and DTOs exist but there is no CQRS layer or repository to create, read, update, or delete seasons, nor to manage which players are enrolled in a season.

## Proposed Solution

Implement `ISeasonRepository`, all season CQRS commands and queries with Wolverine native handlers, an `InMemorySeasonRepository`, and the player-season membership commands.

## Business Requirements

**Given** a create season command is dispatched
**When** handled
**Then** the season is persisted and its new int ID is returned

**Given** an add-player-to-season command is dispatched
**When** the season and player both exist
**Then** the player is enrolled in the season

## Acceptance Criteria

- [ ] `ISeasonRepository` in `Winterplein.Application/Interfaces/`: GetAll, GetById, Add, Update, Delete
- [ ] `CreateSeasonCommand`, `UpdateSeasonCommand`, `DeleteSeasonCommand` + handlers
- [ ] `GetSeasonsQuery`, `GetSeasonByIdQuery` + handlers
- [ ] `AddSeasonPlayerCommand`, `RemoveSeasonPlayerCommand`, `GetSeasonPlayersQuery` + handlers
- [ ] `InMemorySeasonRepository` using `ConcurrentDictionary<int, Season>`, registered as Singleton
- [ ] `GetById(int id)` added to `IPlayerRepository` and `InMemoryPlayerRepository`

## Technical Notes

- Handlers are static classes with static `Handle` method — Wolverine native convention
- Commands and queries live in `Winterplein.Application/Seasons/`
- `AddSeasonPlayerCommand` loads both season and player via their repositories, calls `season.AddPlayer(player)`, then calls `repo.Update(season)`
