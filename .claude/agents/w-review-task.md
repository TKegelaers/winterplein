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

Read `docs/epics/<change-path>/plan.md` to verify the task is in status "under review". If not, report back to your parent agent that the task is not ready for review yet.docs/epics/<change-path>/

### 0.3 Check for prior review

If a `review.md` already exists for the task, read it to understand previous feedback and whether it has been addressed in the current implementation.

### 0.4 Find design.md file for broader context

If there is a `design.md` file in `docs/epics/<change-path>/`, read it to understand the overall technical approach and architecture decisions. Use this to verify the implementation follows the intended design.

### 1 Understand the task

Read `task.md` (and `review.md` if present) to understand the scope, domain model changes, test cases, and acceptance criteria.

### 2 Review the implementation

Review the implementation across three dimensions:

**Completeness** — Are all requirements implemented and tested?
- All acceptance criteria from `task.md` are covered by code
- All test cases from `task.md` exist and pass
- All affected files from `task.md` were created/modified as specified

**Correctness** — Does the implementation work correctly?
- Implementation matches spec intent and handles the scenarios described
- Edge cases and error states are handled (not just the happy path)
- Tests actually verify the expected behavior (not just that no exception is thrown)

**Coherence** — Does the implementation fit the codebase?
- Follows the technical approach and architecture decisions in `design.md`
- Consistent with similar implementations in the codebase (naming, patterns, layering)
- Follows coding standards and best practices defined in `.ai/change-config.md`

For each dimension, note any findings. Classify each issue as:

| Severity | Meaning |
|----------|---------|
| **Critical** | Must fix — breaks correctness, missing core requirement, violates architecture |
| **Warning** | Should fix — suboptimal pattern, missing edge case, naming drift from design |
| **Suggestion** | Nice to have — minor improvement, alternative approach |

### 3 Write review.md (if issues found)

If issues exist, write or update `docs/epics/<change-path>/tasks/<task-name>/review.md` with your findings. Follow the structure in `templates/review.md`.

## Output

Report back to your parent agent:

- **Satisfactory** — no issues found, task can be marked completed
- **Needs rework** — issues found, feedback written to `review.md`, task should be sent back for re-implementation
