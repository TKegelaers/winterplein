---
name: create-user-story
description: Create a new User Story for the Winterplein project following the established story file format (.tasks/epicN-name/storyN-slug.md). Use this skill whenever the user wants to add a new story, plan a feature as a story, expand an epic, or write up acceptance criteria and tasks for new work. Trigger on phrases like "create a story", "add a user story", "plan story N", "write up a story for X", "I need a story for", or any time a new feature needs to be decomposed and tracked as a story in the roadmap.
---

# Create User Story

## Goal

Produce a well-structured User Story file that fits the project's `.tasks/` format, then register it in ROADMAP.md.

## What to gather before writing

Before writing anything, make sure you know:

1. **Which epic** — read `ROADMAP.md` to see existing epics and their slugs (e.g. `epic1-match-generation`). Ask the user if unclear.
2. **Story number** — look at existing story files in the epic's `.tasks/` folder to determine the next available number.
3. **Story title** — short, imperative phrase (e.g. "Player Statistics", "PDF Export").
4. **Dependencies** — read all existing story files in the epic to understand story-level dependencies *and* which specific tasks in other stories this story's tasks will depend on. Cross-story task dependencies are common (e.g. `(blockedBy: Story 1 T5)`).
5. **Feature description** — what the story delivers and why. May come from the user's prompt.
6. **Stack context** — this project uses Clean Architecture: Domain → Application (CQRS/MediatR) → Infrastructure (EF Core) → Api (Controllers) → Client (Blazor WASM/MudBlazor). Organize acceptance criteria by layer accordingly.

Read the existing story files in the epic before writing — not just to find the next number, but to match their style, understand context, and identify task-level cross-story dependencies.

If the user's request is vague, ask one clarifying question before writing — don't ask multiple at once.

## File format

Save to: `.tasks/<epic-slug>/story<N>-<kebab-title>.md`

```markdown
# Story <N> — <Title>

**Epic:** <Epic name>
**Dependencies:** <Story N (reason)> or "None"

## Description

<2–4 sentences: what is being built and the user/business value.>

## Acceptance Criteria

### <Layer or component name> (`<ProjectName>/<Path>/`)

- Specific, verifiable requirement
- Another requirement

### <Next layer or component>

- ...

### API Endpoints (table format — use for controller stories)

| Method | Route | Request | Response |
|--------|-------|---------|----------|
| GET | `/api/resource` | — | `List<Dto>` 200 |
| POST | `/api/resource` | `CreateRequest` | `Dto` 201 / 400 |

## Technical Notes

- Constraint, convention, or non-obvious implementation decision relevant to this project's architecture
- Reference the relevant project (e.g. `Winterplein.Application`) and pattern (e.g. MediatR command + handler)

## Tasks

- [ ] T1: <First concrete implementation step>
- [ ] T2: <Next step> (blockedBy: T1)
- [ ] T3: <Another step> (blockedBy: T1)
```

### Key formatting rules

- **Acceptance criteria** are organized by layer/component following this order: Domain → CQRS & Repository → API → UI → Tests. Skip layers the story doesn't touch. Each story is a vertical slice — include all relevant layers, not just one.
- **Tasks** are small, individually completable units of work. Use `(blockedBy: TX)` to indicate a task that can't start until TX is done. The annotation goes on the dependent task (e.g. T2 can't start until T1 is done → T2's entry says `(blockedBy: T1)`). Cross-story references are allowed and common: `(blockedBy: Story 1 T5)` or multiple: `(blockedBy: T1, Story 2 T3, Story 2 T4)`.
- **Technical Notes** capture anything a developer needs to know that isn't obvious from the acceptance criteria alone: naming conventions, EF Core considerations, CORS, MediatR registration, etc.

## After writing the file

Update `ROADMAP.md`:
- Add a new row to the correct epic's table with: `| N | [<Title>](.tasks/<epic-slug>/story<N>-<slug>.md) | <one-line description> | Pending |`

Tell the user what was created and where, in one line.
