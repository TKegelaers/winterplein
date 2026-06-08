# scaffold-playwright-project Implementation Plan

## Overview

Create the `Winterplein.E2eTests` xUnit project with Playwright, a shared browser fixture, a `PageTest` base class, and a home page smoke test. The project is added to the solution but excluded from the default `dotnet test` run at solution root.

## Task list

- T1 [create-e2e-test-project](./tasks/create-e2e-test-project/task.md)
  - status: pending
  - dependencies: none

- T2 [add-playwright-infrastructure](./tasks/add-playwright-infrastructure/task.md)
  - status: pending
  - dependencies: T1

- T3 [add-smoke-test](./tasks/add-smoke-test/task.md)
  - status: pending
  - dependencies: T2

- T4 [update-claude-md](./tasks/update-claude-md/task.md)
  - status: pending
  - dependencies: T3
