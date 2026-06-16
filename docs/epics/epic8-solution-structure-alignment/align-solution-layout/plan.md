# align-solution-layout Implementation Plan

## Overview

Pure structural refactor aligning the Winterplein solution to the KOAla service
project layout (`docs/service-project-layout.md`), adapted for a single service.
No behaviour changes. Sequenced so the solution builds and the full test suite
stays green after every task: contracts rename -> application internals ->
infrastructure -> host rename -> test split. All project add/remove/rename go
through `dotnet sln` (never hand-edit `.slnx`). Design and the Application.IO
no-Domain audit live in `docs/changes/align-solution-layout/design.md`.

## Task list

- 1 [01-rename-shared-to-application-io](./tasks/01-rename-shared-to-application-io/task.md)
  - status: completed
  - dependencies: none

- 2 [02-restructure-application](./tasks/02-restructure-application/task.md)
  - status: completed
  - dependencies: 1

- 3 [03-restructure-infrastructure](./tasks/03-restructure-infrastructure/task.md)
  - status: completed
  - dependencies: 2

- 4 [04-rename-api-to-webapi](./tasks/04-rename-api-to-webapi/task.md)
  - status: completed
  - dependencies: 3

- 5 [05-split-test-projects](./tasks/05-split-test-projects/task.md)
  - status: completed
  - dependencies: 4
