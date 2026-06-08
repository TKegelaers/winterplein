# update-claude-md

## Scope

Update `CLAUDE.md` with E2E test information:

- Add `dotnet test tests/Winterplein.E2eTests` to the Commands section
- Add a note that Playwright browser binaries must be installed once after project creation: `pwsh tests/Winterplein.E2eTests/bin/Debug/net10.0/playwright.ps1 install --with-deps chromium`
- Add a note that E2E tests require the API and Client to be running, and are excluded from `dotnet test` at solution root

## Domain model changes

None.

## Test cases

None — documentation change only.

## Affected files

- modify: `CLAUDE.md`
