---
name: w-review-task
description: "Instructions for reviewing a subtask of a feature, use this skill when you want to review a specific task defined in the plan.md for a change"
argument-hint: <change-name> <task-name>
user-invocable: false
disable-model-invocation: false
---

# Review Task

## Context

$change-name = $1
$task-name = $2

We are about to review the implementation of a specific task
Be critical in your review and provide constructive feedback to ensure the quality of the implementation and that it meets the defined acceptance criteria for the task.

Find the information related to the task in `docs/epics/{{$change-name}}/tasks/{{$task-name}}/`.
Read `docs/epics/{{$change-name}}/plan.md` to determine task status.
If the task is not in status "under review", report back to your parent agent that the task is not ready for review yet.

## Process

### 0.1 Find the task.md file

If you cannot find the `task.md` file this agent is not the right one to review the task. Report back to your parent agent that you cannot find the `task.md` file for the task.

### 0.2 Find review.md file if it exists

If there is a `review.md` file for the task, read it to understand if there has been a previous review and what feedback was provided.

### 1 Understand the task requirements

Read the `task.md` file and optionally the `review.md` file to understand the scope of the task, the domain model changes required, the test cases that need to be reviewed.

### 2 Review the implementation

Review the implementation of the task by looking at the code changes made for the task, running the tests defined for the task and verifying that all acceptance criteria defined in `task.md` are met.

- Be critical
- Provide constructive feedback
- Validate that the implementation is in line with other existing implementations in the codebase and follows the defined coding standards and best practices.

### 3 Provide feedback (Optional)

If there are any issues with the implementation or if any acceptance criteria are not met, provide clear and actionable feedback in a `review.md` file for the task. If there was a previous review, take into account the feedback provided in the previous review and check if it has been addressed in the current implementation.

Do not force issues when there are none. A review can validly find zero issues.

## Output

Update the `review.md` file for the task with your feedback if there are any issues that need to be addressed.
Make sure the `review.md` file adheres to the structure outlined in `templates/review.md`.

Notify your parent agent whether the implementation is satisfactory or if it should be sent back for re-implementation based on the review.
