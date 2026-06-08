# test-player-match-generation Implementation Plan

## Overview

Add `data-testid` attributes to the Players and Matches Blazor pages, then write a `PlayerMatchGenerationTests` E2E test class with five Playwright tests covering the add/remove player and generate-matches flow.

Depends on Story 1 (scaffold-playwright-project): the `PlaywrightFixture`, `PageTest` base class, and `[Collection("Playwright")]` xUnit collection must exist before the test class can be written.

## Task list

- T1 [add-testid-attributes](./tasks/add-testid-attributes/task.md)
  - status: pending
  - dependencies: none

- T2 [create-e2e-test-class](./tasks/create-e2e-test-class/task.md)
  - status: pending
  - dependencies: T1, Story 1 (scaffold-playwright-project)
