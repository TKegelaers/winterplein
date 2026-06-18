# season-schedule-management Implementation Plan

## Overview

Add a matchday-by-matchday schedule view and clear (single + all) operations. New IO contracts, repository delete methods, query/clear handlers, three API endpoints + client methods, and a replacement schedule UI on the season detail page. Clear handlers are void and signal 404 via `KeyNotFoundException`. See `design.md` for the technical approach and `change.md` for the requirements.

## Task list

- 1 [io-contracts](./tasks/io-contracts/task.md)
  - status: completed
  - dependencies: none

- 2 [repository-deletes](./tasks/repository-deletes/task.md)
  - status: completed
  - dependencies: none

- 3 [schedule-and-clear-handlers](./tasks/schedule-and-clear-handlers/task.md)
  - status: completed
  - dependencies: 1, 2

- 4 [api-and-client](./tasks/api-and-client/task.md)
  - status: completed
  - dependencies: 3

- 5 [season-detail-ui](./tasks/season-detail-ui/task.md)
  - status: completed
  - dependencies: 4

## Notes

- Tasks 1 and 2 are independent and can run in parallel.
- Acceptance round-trip (generate → clear one → re-generate refills), clear-empty → 404, and Clear All idempotent → 204 are verified by the integration tests in task 4.
