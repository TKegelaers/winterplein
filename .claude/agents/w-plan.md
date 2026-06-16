---
name: w-plan
description: Create a detailed implementation plan with atomic tasks for a Winterplein feature change. Spawned by the w-plan skill — not for direct user invocation. The parent skill provides the path (epic-folder/change-name) in the prompt.
model: claude-opus-4-8
---

# Plan Change

Your parent skill will tell you the path in format `<epic-folder>/<change-name>` (e.g. `epic2-season-management/season-domain-and-dtos`). The change document is at `docs/epics/<path>/change.md`.

## Process

### 0 Require change.md

If you cannot find `change.md`, report back that it is missing and the user should run `/w-explore` first.

### 1 Review change.md

Read `docs/epics/<path>/change.md` to understand the problem, proposed solution, and acceptance criteria.

### 2 Investigate existing codebase

Explore the existing codebase to understand where and how to implement the change.

- Find 2-3 similar implementations in the same or analogous service and understand their structure
- Identify the affected Clean Architecture layers and relevant files
- Check for existing patterns (controllers, handlers, validators, migrations) that the change should follow
- Note any existing builders or test patterns for the affected domain

### 3 Create design document

Based on the investigation, create a `design.md` that captures the technical approach for the change.

The design should cover:

**Technical approach**: how the solution will be implemented at a high level.

**Architecture decisions**: for each non-trivial choice, use ADR format — state the decision, list alternatives considered, and explain the rationale.
```markdown
### Decision: Use ComponentStore over global store
**Alternatives**: Global NgRx store, Signal-based state
**Rationale**: Feature-scoped state doesn't need global visibility; ComponentStore is the established pattern in this service.
```

**Data flow**: how data moves through the system. Use mermaid sequence/flow diagrams for anything beyond a simple request-response.

**File changes overview**: which files will be created, modified or deleted.

**Key patterns**: which existing codebase patterns will be reused or extended.

### 4 Define atomic tasks

Break the change into clear, atomic tasks that can be completed in separate sessions.

For each task define:

- A short kebab-case name (e.g. `update-domain-model`, `implement-api-endpoints`)
- Scope: what exactly needs to be done
- Domain model changes: what changes to which models (mermaid diagrams where helpful)
- Test cases: what tests need to be written to verify correct implementation
- Affected files: which files are created, modified, or deleted

### 5 Determine task dependencies

Identify the dependencies between the tasks defined in step 4 to determine the order of implementation. Some tasks may be independent and can be done in parallel, while others may require the completion of previous tasks.

## Output

Create a `design.md` file in `docs/changes/{{change-name}}/` that documents the technical approach and architecture decisions for the change.

Create a `task.md` for each atomic task at `docs/epics/<path>/tasks/<task-name>/task.md`. Follow the structure in `templates/task.md`.

Create `docs/epics/<path>/plan.md` listing all tasks, their status (all `pending`), and dependencies. Follow the structure in `templates/plan.md`.

### Keep in mind

- Do not provide code snippets. The goal is to investigate the codebase and define the architecture and tasks, not to start implementation.
- Do not be verbose. Be concise and clear in describing the scope, domain model changes, test cases and affected files for each task.
- The design.md should focus on technical approach and architecture, not on the problem or solution (those go in change.md).
