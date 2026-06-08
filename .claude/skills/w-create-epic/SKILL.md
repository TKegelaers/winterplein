---
name: w-create-epic
description: Create a new Epic for the Winterplein project — ROADMAP.md section and story table. Use this skill whenever the user wants to add a new epic, plan a major feature area, or decompose a large initiative into stories. Trigger on phrases like "create an epic", "add an epic", "plan epic N", "I need an epic for", or any time a new feature area needs its own epic in the roadmap.
---

# Create Epic

## Goal

Register a new Epic in the project: add a section to `ROADMAP.md` and populate the story table with the planned stories (titles + descriptions). Stories are explored and planned separately using `/w-explore` then `/w-plan`.

## Formats (canonical — do not read existing epic or story files to verify these)

### ROADMAP.md epic section

```markdown
## Epic N — <Title>

> <One-sentence goal.>

| #   | Story         | Description            | Status  |
| --- | ------------- | ---------------------- | ------- |
| 1   | <Story Title> | <one-line description> | Pending |
| 2   | <Story Title> | <one-line description> | Pending |
```

Story links are added after `/w-explore` creates the `change.md`. Use plain text for titles initially — do **not** invent file paths that don't exist yet.

Once `/w-explore` has run for a story, update the title cell to:

```
[<Story Title>](docs/epics/<change-name>/change.md)
```

Once `/w-plan` has also run, append a plan link in the same cell:

```
[<Story Title>](docs/epics/<change-name>/change.md) · [plan](docs/epics/<change-name>/plan.md)
```

### CLAUDE.md Current State entry

Append under the last epic entry in the `## Current State` section:

```markdown
**Epic N — <Title>**

- Stories 1–M: all Pending
```

## What to gather before writing

Before writing anything, make sure you know:

1. **Epic number** — scan the `## Epic N` headings in `ROADMAP.md` to find the highest N; the new epic is N+1.
2. **Epic title** — short, imperative phrase (e.g. "Match Statistics", "PDF Export", "User Authentication").
3. **Goal sentence** — one sentence describing what this epic delivers and why. Appears as the `> blockquote` under the epic heading.
4. **Planned stories** — a list of stories for this epic (title + one-line description). Stories should be **functional slices** — each story delivers a complete, user-facing capability that cuts vertically through the stack (domain, application, API, UI, tests as needed). Avoid single-layer technical stories like "Domain Models" or "Write Tests". Instead, group by what the user can do: "Enroll Players in a Season", "Generate Matches for a Matchday", etc.
5. **Dependencies** — does this epic depend on another epic being complete first?

If the user's request is vague, ask one clarifying question before writing — don't ask multiple at once.

## Actions to take

### 1. Add the epic section to ROADMAP.md

Insert the new epic section **before** the `## Future Epics` section (or at the end if that section doesn't exist). If the epic was listed in the Future Epics table, remove that row. Use the format from the **Formats** section above.

### 2. Update CLAUDE.md Current State

Append the new epic entry to the `## Current State` section using the format from the **Formats** section above.

## Key rules

- Epic titles use short, imperative phrases.
- Stories represent functional capabilities, ordered by dependency (foundational features first). Each story should be a vertical slice delivering end-to-end value across all layers it touches.
- Status for all new stories is `Pending`.
- Do not create story files or folders.

## After writing

Tell the user:

1. What was created (epic number, title).
2. How many stories are planned.
3. That they can now run `/w-explore` for each story to define the change, then `/w-plan` to break it into tasks — and to update the ROADMAP.md story link after each.
