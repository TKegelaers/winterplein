# season-schedule-generation Implementation Plan

## Overview

Add `GenerateScheduleCommand` that assigns one randomly chosen, season-wide-unique match from the pool to each open matchday, persisting a frozen `PlannedMatch` snapshot (denormalized, no FK to Player). Generation is idempotent and exposed via `POST /api/seasons/{id}/schedule/generate` plus a Blazor button. See [design.md](./design.md) for the technical approach and decisions.

## Task list

- 1 [planned-match-domain-and-dtos](./tasks/planned-match-domain-and-dtos/task.md)
  - status: completed
  - dependencies: none

- 2 [planned-match-persistence](./tasks/planned-match-persistence/task.md)
  - status: completed
  - dependencies: 1 (planned-match-domain-and-dtos)

- 3 [generate-schedule-handler](./tasks/generate-schedule-handler/task.md)
  - status: completed
  - dependencies: 1 (domain + DTOs), 2 (IPlannedMatchRepository)

- 4 [schedule-api-endpoint](./tasks/schedule-api-endpoint/task.md)
  - status: completed
  - dependencies: 2 (repository registration), 3 (handler)

- 5 [schedule-blazor-ui](./tasks/schedule-blazor-ui/task.md) - status: completed - dependencies: 4 (client method + endpoint)
  </content>
