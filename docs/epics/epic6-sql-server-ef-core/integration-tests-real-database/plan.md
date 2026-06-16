# integration-tests-real-database Implementation Plan

## Overview

Replace the SQLite in-memory integration-test setup with a real SQL Server database (`Winterplein_integrationTests`): register `UseSqlServer` from a test `appsettings.json`, apply EF Core migrations on factory startup, clear all data before each test with Respawn (data persists for inspection), disable parallelization, add fluent seed builders, and migrate existing tests to the new base class.

See [design.md](./design.md) for technical approach and architecture decisions.

## Task list

- T1 [sqlserver-factory-and-respawn](./tasks/sqlserver-factory-and-respawn/task.md)
  - status: completed
  - dependencies: none

- T2 [seed-builders](./tasks/seed-builders/task.md)
  - status: completed
  - dependencies: (blockedBy: T1)

- T3 [migrate-existing-tests](./tasks/migrate-existing-tests/task.md)
  - status: completed
  - dependencies: (blockedBy: T1, T2)
