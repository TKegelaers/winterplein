---
name: w-implement
description: "Instructions for implementing a feature, use this skill when you want to implement a new feature or make changes to an existing one"
argument-hint: <change-name>
user-invocable: true
---

# Implement Change

## Context

$change-name = $

We defined a plan document outlining the atomic tasks required to implement a change.
Find the document in `docs/epics/{{$change-name}}/plan.md`.

You are to coordinate the implementation of the change by following the plan, delegating each task to a separate subagent and keeping track of the implementation status of each task.

## Process

### 0 require plan.md

If you cannot find the `plan.md` file, ask the user to provide the necessary context to create it first using the `w-explore` and `w-plan` skills.

### 1 Read plan.md

Read the `plan.md` document for the change to understand the overall implementation plan, the list of tasks, their dependencies and their current implementation status. Do not read the individual tasks.

### 2 Implement tasks

Orchestrate the implementation of the plan by delegating each task to a separate subagent.

#### 2.1 Determine next task

Based on the dependencies and implementation status of the tasks outlined in the plan.md, determine which task(s) can be implemented next.

#### 2.2 Delegate task to subagent

For each task identified in step 2.1, spawn a subagent using `subagent_type: w-implement-task`. In the prompt, provide the change path and task name so the agent can locate `docs/epics/{{$change-name}}/tasks/{{$task-name}}/task.md`.

Update the task status to "in progress" in the `plan.md` document after delegating the task to a subagent.

### 2.3 Delegate review of the task to a subagent

Once the subagent has completed the implementation of the task, spawn a separate subagent using `subagent_type: w-review-task`. In the prompt, provide the change path and task name so the agent can locate `docs/epics/{{$change-name}}/tasks/{{$task-name}}/task.md`.

Update the task status to "under review" in the `plan.md` document after delegating the review of the task to a subagent.

### 2.4 Re-iterate on the task implementation if necessary

If the review subagent provides feedback that requires changes to the implementation, reiterate on the implementation by going back to step 2.2 and delegating the task again to a new subagent for re-implementation.

If the review subagent reports no issues, update the task status to "completed" in the `plan.md` document.

### 3 Validate Implementation

After all tasks have been implemented, validate the implementation of the change by verifying that all acceptance criteria outlined in `change.md` are met.

- run all tests
- validate that the changes meet all acceptance criteria defined in `change.md`
