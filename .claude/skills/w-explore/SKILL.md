---
name: w-explore
description: 'Explore a new change by asking structured questions to gather context and align understanding'
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

Think of a short name for the feature or fix that captures its essence.
(e.g. "add-user-session-cache", "fix-session-expiry-bug" )

Create a `change.md` file in the appropriate directory under `docs/changes/{{change-name}}`.
Adhere to the structure outlined in `templates/change.md`.

### keep in mind

- Do not provide implementation details or code snippets in the change.md file. The goal is to define the problem, solution, and acceptance criteria, not to start implementation.
- Do not be verbose in the change.md file. Be concise and clear in describing the problem, solution, and acceptance criteria.
