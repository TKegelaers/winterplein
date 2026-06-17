# season-match-pool Implementation Plan

## Overview

Add a read-only `GetSeasonMatchPoolQuery` that computes a season's full doubles
match pool on demand from its enrolled players via `IMatchGeneratorService`,
never persisting it. Expose it at `GET /api/seasons/{id}/match-pool` (200 with
matches, 200 empty for <4 players, 404 for unknown season), add a client method,
and render the pool as a collapsible, paged `MudTable` on the season detail page.
Reuses the existing `GenerateMatchesResponse` family of DTOs.

## Task list

- T1 [add-match-pool-query-handler](./tasks/add-match-pool-query-handler/task.md)
  - status: completed
  - dependencies: none

- T2 [add-match-pool-endpoint](./tasks/add-match-pool-endpoint/task.md)
  - status: completed
  - dependencies: T1

- T3 [add-match-pool-ui](./tasks/add-match-pool-ui/task.md)
  - status: completed
  - dependencies: T2

## Design

See [`design.md`](design.md).
