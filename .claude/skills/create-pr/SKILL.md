---
name: create-pr
description: 'Create GitHub pull request from the current branch to develop. Use when the user asks to create a PR, open a pull request, push and PR, or mentions "/pr". Automatically pushes the branch if not already pushed, derives the title from the recent commits and creates the PR via GitHub.'
allowed-tools: Bash, mcp__github__create_pull_request, mcp__github__list_pull_requests
---

# Create Pull Request to Develop

## Overview

Push the current branch (if needed) and open a pull request targeting `develop` using the GitHub MCP.

## Workflow

### 1. Get Branch Info

```powershell
# Current branch name
git branch --show-current

# Get GitHub remote URL to extract owner/repo
git remote get-url origin

# Check upstream and ahead/behind counts
git status --porcelain=v2 --branch
```

Parse the remote URL to extract `owner` and `repo`:

- HTTPS: `https://github.com/<owner>/<repo>.git`
- SSH: `git@github.com:<owner>/<repo>.git`

From `git status --porcelain=v2 --branch` output, read:

- `# branch.upstream` — present means an upstream is set; absent means no upstream
- `# branch.ab +N -M` — `N` is commits ahead of remote; `M` is commits behind

### 2. Verify No Uncommitted Changes

```powershell
git status --porcelain
```

If there are uncommitted changes, stop and prompt the user to commit first.

### 3. Push Branch (if needed)

Push if **either** of these is true:

- No upstream is set (`# branch.upstream` absent in step 1 output)
- Branch is ahead of remote (`+N` where N > 0 in step 1 output)

```powershell
git push --set-upstream origin <branch-name>
```

Skip only if the branch already tracks a remote **and** is not ahead of it.

### 4. Check for Existing PR

Use `mcp__github__list_pull_requests` to check if an open PR already exists for this branch:

```
mcp__github__list_pull_requests(
  owner: "<owner>",
  repo:  "<repo>",
  head:  "<owner>:<branch-name>",
  base:  "develop",
  state: "open"
)
```

If a PR is found, stop and report its URL — do not create a duplicate.

### 5. Derive PR Title

1. Look for a GitHub issue number in the branch name (e.g. `feat/42-short-desc` → `#42`) or in commit messages. If none found, skip the prefix.
2. Fetch and read commits on this branch not yet in `develop`:
   ```powershell
   git fetch origin develop
   git log origin/develop..HEAD --oneline
   ```
3. Compose a title (≤72 chars, present tense). Prefix with `#<issue> - ` if an issue number was found.

### 6. Compose PR Description

Build a short markdown description:

- **What** changed — bullet points derived from commit messages (3–8 bullets max)
- **Closes** — `Closes #<issue>` if an issue number was found

### 7. Create the PR

Use `mcp__github__create_pull_request` with:

| Field   | Value                                                  |
| ------- | ------------------------------------------------------ |
| `owner` | extracted from remote URL                              |
| `repo`  | extracted from remote URL                              |
| `title` | derived in step 5                                      |
| `body`  | derived in step 6                                      |
| `head`  | current branch name                                    |
| `base`  | `develop`                                              |
| `draft` | `false` by default; `true` if user asks for a draft PR |

### 8. Report Result

Output:

- PR URL
- PR title
- Source → target branch

## Safety Rules

- NEVER force-push (`--force`) unless explicitly requested.
- NEVER target `main` or `master` — always target `develop`.
- NEVER create a PR if there are uncommitted changes; prompt the user to commit first.
- NEVER create a duplicate PR; check for an existing open PR first (step 4).
