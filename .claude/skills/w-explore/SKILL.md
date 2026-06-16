---
name: w-explore
description: "Explore a new change by asking structured questions to gather context and align understanding"
user-invocable: true
---

# Explore Change

## Context

We are about to start implementing a new feature or bug fix in our application.

Before we begin, we will define the context for this change through a structured questioning process.
Ask questions to build a change.md file containing the following sections:

- Problem statement ( required )
- Proposed solution ( required )
- Business requirements ( required )
- Acceptance criteria ( required )

Optionally, also ask questions to fill out these additional sections if relevant:

- Testing plan
- Potential pitfalls
- Refactors

## Process

### 0 Resolve the epic (required)

Every change must belong to an epic. Resolve this before any exploration.

- If the user provided an epic, use it.
- If not, ask which epic the change belongs to. List the existing epic folders under `docs/epics/` so the user can pick one (e.g. `epic2-season-management`).
- The epic must map to an existing folder in `docs/epics/`.
- The user may also choose to start a **new** epic. In that case, run the `w-create-epic` skill (`/create-epic`) to register the epic first, then use its folder. Do not invent an epic folder ad hoc.

Do not proceed to the steps below until an epic is confirmed (existing or newly created).

### 1 Initial change description

If the user did not provide a broad idea of the feature or bug, ask them to describe the change they want to make.

### 2 Explore existing business context

Ask targeted questions about the current domain/business rules, constraints, and impacted users so the change fits existing behavior.

### 3 Ask questions

Ask questions about missing details in the change description, and about the sections of the change.md file mentioned above, to gather all the necessary context for implementation.

Continue until you have a complete understanding of the change.

- **Be specific**: Prefer concrete questions over open-ended ones
- **Provide context**: Briefly explain why you're asking when it helps clarity
- **Listen actively**: Use answers to inform follow-up questions
- **Confirm understanding**: Summarize the task once you have enough context

## Output

Use the epic resolved in step 0 — its folder name from `docs/epics/` (e.g. `epic2-season-management`).

Think of a short name for the feature or fix that captures its essence.
(e.g. "add-user-session-cache", "fix-session-expiry-bug" )

Create a `change.md` file at `docs/epics/{{epic-folder}}/{{change-name}}/change.md`.
Adhere to the structure outlined in `templates/change.md`.

### keep in mind

- Do not provide implementation details or code snippets in the change.md file. The goal is to define the problem, solution, and acceptance criteria, not to start implementation.
- Do not be verbose in the change.md file. Be concise and clear in describing the problem, solution, and acceptance criteria.
