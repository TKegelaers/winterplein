---
name: w-plan
description: "Create a detailed implementation plan with atomic tasks"
argument-hint: <epic-folder>/<change-name>
user-invocable: true
---

# Plan Change

## Context

$path = $ (format: `<epic-folder>/<change-name>`, e.g. `epic2-season-management/season-domain-and-dtos`)

## Action

Spawn a subagent using `subagent_type: w-plan`. In the prompt, provide the path so the agent can locate `docs/epics/{{$path}}/change.md`.

The agent runs on Opus and handles all codebase investigation, task decomposition, and file writing.
