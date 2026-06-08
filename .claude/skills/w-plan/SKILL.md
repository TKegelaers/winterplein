---
name: w-plan
description: 'Create a detailed implementation plan with atomic tasks'
argument-hint: <change-name>
user-invocable: true
---

# Plan Change

## Context

$change-name = $

We defined a change document outlining the problem, solution, and acceptance criteria for a new change.
You can find it in `docs/changes/{{$change-name}}/change.md`.

Understand the required changes and create a high-level plan for implementation.

## Process

### 0 require change.md

If you cannot find the `change.md` file, ask the user to provide the necessary context to create it first using the `explore` skill.

### 1 Review change.md

Read the `change.md` document for the change to understand the problem, proposed solution, and acceptance criteria.

### 2 Investigate existing codebase

Explore the existing codebase to understand where and how to implement the change.

### 3 Define atomic tasks

Break the change down into clear, atomic tasks that can be completed in separate sessions.

For each task, define:

- A short name that captures the essence of the task (e.g. "update-domain-model", "implement-api-endpoints", "dynamodb-book-access")
- The scope: what exactly needs to be done in this task?
- Domain model changes: what changes, if any, need to be made to which domain models for this task?
  (mermaid diagrams can be helpful here)
- Test cases: what tests need to be written to verify correct implementation?
- Affected files: which files will be created, modified or deleted as part of this task?

### 4 Determine task dependencies

Identify the dependencies between the tasks defined in step 3 to determine the order of implementation. Some tasks may be independent and can be done in parallel, while others may require the completion of previous tasks.

## Output

Create a `task.md` file for each atomic task in directory `docs/changes/{{change-name}}/tasks/{{task-name}}/`.
Each task file should adhere to the structure outlined in `templates/task.md`.

Create a `plan.md` file in directory `docs/changes/{{change-name}}/` that outlines the overall implementation plan, including the list of tasks, their implementation status and the dependencies between them. The plan should adhere to the structure outlined in `./templates/plan.md`.

### keep in mind

- Do not provide code snippets. The goal is to investigate the codebase and define the tasks, not to start implementation.
- Do not be verbose. Be concise and clear in describing the scope, domain model changes, test cases and affected files for each task.
