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

### 1 Understand the task

Read `task.md` (and `review.md` if present) to understand: scope, domain model changes, test cases to implement, and affected files.

### 2 Find a similar implementation

Explore the codebase for an existing implementation similar to this task. Use it as a reference to ensure consistency with existing coding standards and patterns.

### 3 Implement

Implement the task as defined in `task.md`, incorporating any reviewer feedback.

- Use a TDD approach: write the failing test first, then implement the code to make it pass.
- Stay within the scope defined in `task.md`. If you identify out-of-scope work, note it and add a new task to `plan.md` instead.
- Match the coding style and conventions found in the existing codebase.

### 4 Report completion

Report back to your parent agent that the task is complete and ready for review.
