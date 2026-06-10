# ef-core-dbcontext-repositories Implementation Plan

## Overview

Replace in-memory repositories with SQL Server persistence using EF Core. Add `WinterpleinDbContext` with entity configurations, implement `EfPlayerRepository` and `EfSeasonRepository`, wire up DI, and run the initial migration.

## Task list

- T1 [add-ef-core-packages](./tasks/add-ef-core-packages/task.md)
  - status: completed
  - dependencies: none

- T2 [create-dbcontext-and-configurations](./tasks/create-dbcontext-and-configurations/task.md)
  - status: completed
  - dependencies: T1

- T3 [implement-ef-repositories](./tasks/implement-ef-repositories/task.md)
  - status: completed
  - dependencies: T2

- T4 [wire-up-di-and-connection-string](./tasks/wire-up-di-and-connection-string/task.md)
  - status: completed
  - dependencies: T3

- T5 [run-initial-migration](./tasks/run-initial-migration/task.md)
  - status: in progress
  - dependencies: T4
