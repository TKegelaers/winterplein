---
name: create-epic
description: Create a new Epic for the Winterplein project — folder, ROADMAP.md section, and story table. Use this skill whenever the user wants to add a new epic, plan a major feature area, or decompose a large initiative into stories. Trigger on phrases like "create an epic", "add an epic", "plan epic N", "I need an epic for", or any time a new feature area needs its own epic in the roadmap.
---

# Create Epic

## Goal

Register a new Epic in the project: create its `.tasks/` folder, add a section to `ROADMAP.md`, and populate the story table with the planned stories (titles + descriptions). Story files are created separately using the `create-user-story` skill.

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

Story file links are added later once files exist. Use plain text for titles in the table — do **not** invent file paths that don't exist yet.

### CLAUDE.md Current State entry

Append under the last epic entry in the `## Current State` section:

```markdown
**Epic N — <Title>**

- Stories 1–M: all Pending
```

## What to gather before writing

Before writing anything, make sure you know:

1. **Epic number** — scan the `## Epic N` headings in `ROADMAP.md` to find the highest N; the new epic is N+1. As of the last skill update the next epic is **7**, but verify against the file.
2. **Epic title** — short, imperative phrase (e.g. "Match Statistics", "PDF Export", "User Authentication").
3. **Epic slug** — kebab-case version of the title (e.g. `match-statistics`). Combined with the number: `epicN-<slug>`.
4. **Goal sentence** — one sentence describing what this epic delivers and why. Appears as the `> blockquote` under the epic heading.
5. **Planned stories** — a list of stories for this epic (title + one-line description). Stories should be **functional slices** — each story delivers a complete, user-facing capability that cuts vertically through the stack (domain, application, API, UI, tests as needed). Avoid single-layer technical stories like "Domain Models" or "Write Tests". Instead, group by what the user can do: "Enroll Players in a Season", "Generate Matches for a Matchday", etc.
6. **Dependencies** — does this epic depend on another epic being complete first? Note the last epic in `ROADMAP.md` to check ordering.

If the user's request is vague, ask one clarifying question before writing — don't ask multiple at once.

## Actions to take

### 1. Create the epic folder

Create `.tasks/epicN-<slug>/` by writing a placeholder (the folder must exist for story files later). Do this by noting it in the output — actual folder creation happens when the first story file is written, or create it explicitly if needed.

### 2. Add the epic section to ROADMAP.md

Insert the new epic section **before** the `## Future Epics` section (or at the end if that section doesn't exist). If the epic was listed in the Future Epics table, remove that row. Use the format from the **Formats** section above.

### 3. Update CLAUDE.md Current State

Append the new epic entry to the `## Current State` section using the format from the **Formats** section above.

## Key rules

- Epic titles use short, imperative phrases — same convention as story titles.
- The epic slug must be lowercase kebab-case, matching the folder name exactly.
- Stories represent functional capabilities, ordered by dependency (foundational features first, then features that build on them). Each story should be a vertical slice delivering end-to-end value, including all layers it touches (domain, CQRS, API, UI, tests).
- Status for all new stories is `Pending`.
- Do not create story files — tell the user to run `/create-user-story` for each story after the epic is set up.

## After writing

Tell the user:

1. What was created (epic number, title, folder path).
2. How many stories are planned.
3. That they can now run `/create-user-story` to write each story file.
