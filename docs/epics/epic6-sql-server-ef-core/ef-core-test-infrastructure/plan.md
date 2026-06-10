# ef-core-test-infrastructure Implementation Plan

## Overview

Swap the integration test factory's in-memory repository stubs for a SQLite in-memory `WinterpleinDbContext`, so integration tests exercise the full EF Core pipeline without requiring SQL Server.

## Task list

- T1 [update-project-references](./tasks/update-project-references/task.md)
  - status: done
  - dependencies: none

- T2 [refactor-api-factory](./tasks/refactor-api-factory/task.md)
  - status: done
  - dependencies: (blockedBy: T1)
