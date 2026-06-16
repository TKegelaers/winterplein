---
name: w-implement-task
description: Implement a specific task defined in plan.md for a Winterplein feature change. Spawned by the w-implement orchestrator — not for direct user invocation. The parent agent provides the change path and task name in the prompt.
---

# Implement Task

Your parent agent will tell you the change path (e.g. `epic2-season-management/season-domain-and-dtos`) and task name. Find all task information at `docs/epics/<change-path>/tasks/<task-name>/`.

## Process

### 0.1 Require task.md

If you cannot find the `task.md` file, report back to your parent agent that the task file is missing.

### 0.2 Check for review.md

If a `review.md` file exists for the task, read it to understand prior reviewer feedback and incorporate it into the implementation.

### 0.3 Find design.md file for broader context

If there is a `design.md` file in `docs/epics/<change-path>/`, read it to understand the overall technical approach and architecture decisions that inform this task.

### 1 Understand the task

Read `task.md` (and `review.md` if present) to understand: scope, domain model changes, test cases to implement, and affected files.

### 2 Find a similar implementation

Explore the codebase for an existing implementation similar to this task. Use it as a reference to ensure consistency with existing coding standards and patterns.

### 3 Implement

Implement the task as defined in `task.md`, incorporating any reviewer feedback.

- Work using a TDD approach by writing a unit test that defines the expected behavior for the task, then implement the necessary code to make the test pass.
- Make sure to only implement the specific scope defined for the task, and not to go beyond it. If you identify additional work that needs to be done that is outside of the scope of the task, make a note of it and create a new task for it in `plan.md`.
- Adhere to the existing coding standards and best practices in the codebase, and make sure that the implementation is consistent with other existing implementations in the codebase.

### 4 Run tests

Run the tests for the affected project to verify the implementation compiles and tests pass. If the affected service is not testable in isolation (e.g. integration-only), run at minimum `dotnet build` on the solution.

Fix any test failures before proceeding.

### 4 Report completion

Report back to your parent agent that the task is complete and ready for review.
