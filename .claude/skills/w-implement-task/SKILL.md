---
name: w-implement-task
description: "Instructions for implementing a subtask of a feature, use this skill when you want to implement a specific task defined in the plan.md for a change"
argument-hint: <change-name> <task-name>
user-invocable: false
disable-model-invocation: false
---

# Implement Task

## Context

$change-name = $1
$task-name = $2

We are about to implement a specific task defined in the plan.md for a change.
Find the information related to the task in `docs/epics/{{$change-name}}/tasks/{{$task-name}}/`.

There should be a task.md file that outlines what has to be done for the task.
There could be a review.md file if the task has been reviewed before and sent back for re-implementation.

## Process

### 0.1 Require task.md file

If you cannot find the `task.md` file this agent is not the right one to implement the task. Report back to your parent agent that you cannot find the `task.md` file for the task.

### 0.2 Find review.md file if task is in status review

If there is a `review.md` file for the task, read it to understand the feedback provided by the reviewer and take it into account for the implementation of the task.

### 1 Understand the task/review requirements

Read the `task.md` file and optionally the `review.md` file to understand the scope of the task, the domain model changes required, the test cases that need to be implemented and the affected files.

### 2 Find a similar implementation in the codebase

Explore the existing codebase to find a similar implementation to the task at hand. This will help you understand how to implement the task in a way that is consistent with other existing implementations in the codebase and follows the defined coding standards and best practices.

### 3 Implement task/review

Implement the task as defined in `task.md`, taking into account any feedback from the reviewer if the task is in status "under review".

- Work using a TDD approach by writing a unit test that defines the expected behavior for the task, then implement the necessary code to make the test pass.
- Make sure to only implement the specific scope defined for the task, and not to go beyond it. If you identify additional work that needs to be done that is outside of the scope of the task, make a note of it and create a new task for it in `plan.md`.
- Adhere to the existing coding standards and best practices in the codebase, and make sure that the implementation is consistent with other existing implementations in the codebase.

### 4 Report completion

Once you have completed the implementation of the task, report back to your parent agent that the task is implemented and ready for review.
