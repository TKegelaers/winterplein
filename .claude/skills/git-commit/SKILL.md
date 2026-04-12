---
name: git-commit
description: 'Execute git commit with conventional commit message analysis, intelligent staging, and message generation. Use when user asks to commit changes, create a git commit, or mentions "/commit". Supports: (1) Auto-detecting type and scope from changes, (2) Generating conventional commit messages from diff, (3) Interactive commit with optional type/scope/description overrides, (4) Intelligent file staging for logical grouping'
license: MIT
allowed-tools: Bash, Read, mcp__github__push_files
---

# Git Commit with Conventional Commits

## Overview

Create standardized, semantic git commits using the Conventional Commits specification. Analyze the actual diff to determine appropriate type, scope, and message.

The workflow is two-step:

1. **Local commit first** (`git add` + `git commit`) — local history is updated immediately and pre-commit hooks run as normal.
2. **Push via GitHub MCP** (`push_files`) — syncs the same changes to GitHub.

Because `push_files` creates its own commit SHA on GitHub (distinct from the local commit SHA), a final `git pull --rebase` reconciles them: since both commits carry identical changes from the same parent, git drops the now-empty local replay and local HEAD advances to the push_files commit.

## Conventional Commit Format

```
<type>[optional scope]: <description>

[optional body]

[optional footer(s)]
```

## Commit Types

| Type       | Purpose                        |
| ---------- | ------------------------------ |
| `feat`     | New feature                    |
| `fix`      | Bug fix                        |
| `docs`     | Documentation only             |
| `style`    | Formatting/style (no logic)    |
| `refactor` | Code refactor (no feature/fix) |
| `perf`     | Performance improvement        |
| `test`     | Add/update tests               |
| `build`    | Build system/dependencies      |
| `ci`       | CI/config changes              |
| `chore`    | Maintenance/misc               |
| `revert`   | Revert commit                  |

## Breaking Changes

```
# Exclamation mark after type/scope
feat!: remove deprecated endpoint

# BREAKING CHANGE footer
feat: allow config to extend other configs

BREAKING CHANGE: `extends` key behavior changed
```

## Workflow

### 1. Gather Context

Use Bash to inspect local state:

```powershell
# Get current branch
git branch --show-current

# Get GitHub remote URL to extract owner/repo
git remote get-url origin

# See changed files and their status
git status --porcelain

# See the full diff to understand what changed
git diff HEAD
```

Parse the remote URL to extract `owner` and `repo`:

- HTTPS: `https://github.com/<owner>/<repo>.git`
- SSH: `git@github.com:<owner>/<repo>.git`

### 2. Identify Files to Commit

From `git status --porcelain`, classify each changed file:

| Code | Meaning             | Action                                                                   |
| ---- | ------------------- | ------------------------------------------------------------------------ |
| `M ` | modified (staged)   | include — read and push                                                  |
| ` M` | modified (unstaged) | include — read and push                                                  |
| `A ` | added (staged)      | include — read and push                                                  |
| `??` | untracked (new)     | include — read and push                                                  |
| `R ` | renamed             | push the new path; old path is auto-removed on GitHub when content moves |
| `D ` | deleted (staged)    | **skip** — `push_files` cannot delete files; warn the user               |
| ` D` | deleted (unstaged)  | **skip** — `push_files` cannot delete files; warn the user               |

**Deletions:** Tell the user which deleted files were skipped and that they must be removed manually (e.g., via a separate `git rm` + `git push` using the git CLI) or via the GitHub web UI.

**Binary files:** Skip any binary files (.png, .jpg, .pdf, .dll, .exe, etc.) — `push_files` takes text content and will corrupt binaries. Warn the user which files were skipped.

**Never commit secrets** (.env, credentials.json, private keys).

### 3. Read File Contents

Use the Read tool to get the current content of each text file to be committed.

### 4. Generate Commit Message

Analyze the diff to determine:

- **Type**: What kind of change is this?
- **Scope**: What area/module is affected?
- **Description**: One-line summary of what changed (present tense, imperative mood, <72 chars)

### 5. Stage Files

Stage all included text files (skip deleted and binary):

```powershell
git add <file1> <file2> ...
```

For untracked (`??`) files, `git add` is required. For already-staged files (`M `, `A `), running `git add` again is harmless.

### 6. Local Commit

Commit locally so local history is immediately up to date and pre-commit hooks run:

```powershell
git commit -m "$(cat <<'EOF'
<type>[scope]: <description>

<optional body>

EOF
)"
```

If the commit is rejected by a pre-commit hook, fix the issue and retry — do NOT use `--no-verify`.

### 7. Check Branch Has Upstream

Verify the branch exists on GitHub before pushing:

```powershell
git ls-remote --exit-code --heads origin (git branch --show-current)
```

If the branch has no upstream, stop and tell the user to push it first:

```powershell
git push -u origin <branch>
```

Then retry from step 7.

### 8. Push via GitHub MCP

Use `mcp__github__push_files` to write the same changes to GitHub:

```
mcp__github__push_files(
  owner:   "<owner>",
  repo:    "<repo>",
  branch:  "<current-branch>",
  message: "<type>[scope]: <description>",
  files: [
    { path: "relative/path/to/file", content: "<file content>" },
    ...
  ]
)
```

### 9. Resync Local Repo

`push_files` created a new commit SHA on GitHub that differs from the local commit SHA. Reconcile with:

```powershell
git pull --rebase
```

Because both commits carry identical changes from the same parent, git drops the now-empty local replay and local HEAD advances to the push_files commit.

## Best Practices

- One logical change per commit
- Present tense: "add" not "added"
- Imperative mood: "fix bug" not "fixes bug"
- Reference issues: `Closes #123`, `Refs #456`
- Keep description under 72 characters
- NEVER write Co-Authored-By trailers

## Safety Rules

- NEVER commit secrets (.env, credentials.json, private keys)
- NEVER force-push to main/master
- Pre-commit hooks run during step 6 (`git commit`) — NEVER skip them with `--no-verify`
- If the user interrupts or corrects mid-flow, stop and ask before proceeding
