# test-match-schedule-planning Implementation Plan

## Overview

Add `data-testid` attributes to the schedule and absence UI, implement a `SetupSeasonWithPlayersAsync` helper, then write seven E2E tests covering schedule generation, clearing, regeneration, absence recording, and absence-aware generation.

## Task list

- T1 [add-schedule-data-testids](./tasks/add-schedule-data-testids/task.md)
  - status: pending
  - dependencies: none

- T2 [add-absence-data-testids](./tasks/add-absence-data-testids/task.md)
  - status: pending
  - dependencies: none

- T3 [add-setup-helper](./tasks/add-setup-helper/task.md)
  - status: pending
  - dependencies: none

- T4 [write-generate-clear-tests](./tasks/write-generate-clear-tests/task.md)
  - status: pending
  - dependencies: T1, T3

- T5 [write-absence-tests](./tasks/write-absence-tests/task.md)
  - status: pending
  - dependencies: T2, T3
