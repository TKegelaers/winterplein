# ef-core-domain-compatibility Implementation Plan

## Overview

Add private parameterless constructors and change get-only properties to `{ get; private set; }` (or `{ get; init; }` for the `Name` record) across all domain entities and value objects, enabling EF Core to materialize them from the database without changing any public API.

## Task list

- T1 [make-name-ef-compatible](./tasks/make-name-ef-compatible/task.md)
  - status: completed
  - dependencies: none

- T2 [make-player-ef-compatible](./tasks/make-player-ef-compatible/task.md)
  - status: completed
  - dependencies: none

- T3 [make-team-match-ef-compatible](./tasks/make-team-match-ef-compatible/task.md)
  - status: completed
  - dependencies: none

- T4 [make-season-ef-compatible](./tasks/make-season-ef-compatible/task.md)
  - status: pending
  - dependencies: none
