---
name: create-epic
description: Create a new Epic for the Winterplein project — folder, ROADMAP.md section, and story table. Use this skill whenever the user wants to add a new epic, plan a major feature area, or decompose a large initiative into stories. Trigger on phrases like "create an epic", "add an epic", "plan epic N", "I need an epic for", or any time a new feature area needs its own epic in the roadmap.
---

# Create Epic

## Goal

Register a new Epic in the project: create its `.tasks/` folder, add a section to `ROADMAP.md`, and populate the story table with the planned stories (titles + descriptions). Story files are created separately using the `create-user-story` skill.

## What to gather before writing

Before writing anything, make sure you know:

1. **Epic number** — read `ROADMAP.md` to find the highest existing epic number and increment by 1.
2. **Epic title** — short, imperative phrase (e.g. "Match Statistics", "PDF Export", "User Authentication").
3. **Epic slug** — kebab-case version of the title (e.g. `match-statistics`). Combined with the number: `epicN-<slug>`.
4. **Goal sentence** — one sentence describing what this epic delivers and why. Appears as the `> blockquote` under the epic heading.
5. **Planned stories** — a list of stories for this epic (title + one-line description). Stories should follow the standard layer breakdown for this stack: Domain → CQRS & Repository → API Endpoints → Blazor UI → Tests. Not all layers are required — only include what the feature touches.
6. **Dependencies** — does this epic depend on another epic being complete first? Check the existing epics in `ROADMAP.md`.

If the user's request is vague, ask one clarifying question before writing — don't ask multiple at once.

## Actions to take

### 1. Create the epic folder

Create `.tasks/epicN-<slug>/` by writing a placeholder (the folder must exist for story files later). Do this by noting it in the output — actual folder creation happens when the first story file is written, or create it explicitly if needed.

### 2. Add the epic section to ROADMAP.md

Insert the new epic section **before** the `## Future Epics` section. If the epic was listed in the Future Epics table, remove that row.

The section format:

```markdown
## Epic N — <Title>

> <One-sentence goal.>

| # | Story | Description | Status |
| --- | ------- | ------------- | -------- |
| 1 | <Story Title> | <one-line description> | Pending |
| 2 | <Story Title> | <one-line description> | Pending |
```

Story links are added later (once story files exist). Use plain text for the title in the table until story files are created — do **not** invent file paths that don't exist yet.

### 3. Update CLAUDE.md summary (optional)

If `CLAUDE.md` has a "Current State" section listing epics and their status, add a row for the new epic.

## Key rules

- Epic titles use short, imperative phrases — same convention as story titles.
- The epic slug must be lowercase kebab-case, matching the folder name exactly.
- Stories in the table follow the layer order: Domain → CQRS & Repository → API → UI → Tests. Skip layers the epic doesn't touch.
- Status for all new stories is `Pending`.
- Do not create story files — tell the user to run `/create-user-story` for each story after the epic is set up.

## After writing

Tell the user:
1. What was created (epic number, title, folder path).
2. How many stories are planned.
3. That they can now run `/create-user-story` to write each story file.
