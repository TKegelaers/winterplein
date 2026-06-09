---
name: w-review-task
description: Review the implementation of a specific task defined in plan.md for a Winterplein feature change. Spawned by the w-implement orchestrator — not for direct user invocation. The parent agent provides the change path and task name in the prompt.
---

# Review Task

Your parent agent will tell you the change path (e.g. `epic2-season-management/season-domain-and-dtos`) and task name. Find all task information at `docs/epics/<change-path>/tasks/<task-name>/`.

Be critical and constructive — the goal is to catch real issues before they reach production.

## Process

### 0.1 Require task.md

If you cannot find the `task.md` file, report back to your parent agent that the task file is missing.

### 0.2 Check task status

Read `docs/epics/<change-path>/plan.md` to verify the task is in status "under review". If not, report back to your parent agent that the task is not ready for review yet.

### 0.3 Check for prior review

If a `review.md` already exists for the task, read it to understand previous feedback and whether it has been addressed in the current implementation.

### 1 Understand the task

Read `task.md` (and `review.md` if present) to understand the scope, domain model changes, test cases, and acceptance criteria.

### 2 Review the implementation

Examine the code changes, run the tests defined for the task, and verify all acceptance criteria in `task.md` are met.

- Be critical — flag real issues clearly
- Provide actionable, specific feedback
- Verify the implementation is consistent with existing code conventions and patterns in the codebase
- Do not invent issues. A review can validly find zero problems.

### 3 Write review.md (if issues found)

If issues exist, write or update `docs/epics/<change-path>/tasks/<task-name>/review.md` with your findings. Follow the structure in `templates/review.md`.

## Output

Report back to your parent agent:

- **Satisfactory** — no issues found, task can be marked completed
- **Needs rework** — issues found, feedback written to `review.md`, task should be sent back for re-implementation
