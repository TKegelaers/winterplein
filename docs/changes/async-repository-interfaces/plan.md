# async-repository-interfaces Implementation Plan

## Overview

Convert `IPlayerRepository` and `ISeasonRepository` from synchronous to fully async interfaces (`Task<T>` returns, `CancellationToken` parameters). Update all 13 handlers and their unit tests to match. In-memory implementations wrap existing sync logic in `Task.FromResult`. No behavior changes — this is a prerequisite for EF Core persistence.

## Task list

- T1 [update-player-repository-interface](./tasks/update-player-repository-interface/task.md)
  - status: pending
  - dependencies: none

- T2 [update-season-repository-interface](./tasks/update-season-repository-interface/task.md)
  - status: pending
  - dependencies: none

- T3 [update-inmemory-repositories](./tasks/update-inmemory-repositories/task.md)
  - status: pending
  - dependencies: T1, T2

- T4 [update-player-handlers](./tasks/update-player-handlers/task.md)
  - status: pending
  - dependencies: T1

- T5 [update-season-handlers](./tasks/update-season-handlers/task.md)
  - status: pending
  - dependencies: T2

- T6 [update-handler-unit-tests](./tasks/update-handler-unit-tests/task.md)
  - status: pending
  - dependencies: T4, T5
