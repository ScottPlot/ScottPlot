---
name: changelog-update-pr
description: Update a changelog for exactly one pull request: the PR associated with the current checked-out branch. Use when an agent is already on a contributor branch and needs to inspect that branch's changes, add one concise changelog entry, avoid summarizing unrelated PRs, and preserve the existing changelog style.
---

# PR Changelog Entry

Update the changelog for the PR represented by the current branch only.

## Rules

- Summarize exactly one PR: the current branch's PR.
- Do not summarize recent releases, merged PRs, or unrelated history.
- Add one entry unless the current PR clearly has multiple independent user-facing changes.
- Edit only the changelog unless asked otherwise.
- If the PR has no user-facing change, ask before editing.

## Workflow

1. Find the changelog. In ScottPlot, use root `CHANGELOG.md`.
2. Identify the current branch and PR metadata when available: PR number, title, author, linked issues.
3. Inspect only this branch's diff against its target/merge base.
4. Read the top few changelog entries for style.
5. Add or update the entry in the top unpublished section only.
6. If the same PR number is already listed, update that entry instead of adding a duplicate.

## ScottPlot Style

ScottPlot entries currently use:

```markdown
* Component: Concise user-facing description (#PR, #issue) @author
```

- Use a component prefix when obvious, such as `Axes:`, `Controls:`, `Avalonia:`, `Legend:`, `Rendering:`, `Cookbook:`, `WinUI:`, or `Maui:`.
- Prefer release-note verbs like `Add`, `Improve`, `Fix`, `Prevent`, `Expose`, or `Reduce`.
- Keep the entry concise and user-facing.
- Include PR number and author when known.

Final response: report the exact changelog entry added or updated, plus any missing PR metadata.
