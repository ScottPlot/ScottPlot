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
- Add new entries to the bottom of the current bullet list in the top unpublished section.
- Edit only the changelog unless asked otherwise.
- If the PR has no user-facing change, ask before editing.

## Workflow

1. Find the changelog. In ScottPlot, use root `CHANGELOG.md`.
2. Identify the current branch and upstream with `git status --short --branch`.
3. Use the branch and upstream only as hints to find the associated PR; do not infer the PR number from the branch name.
4. Fetch authoritative PR metadata when available: PR number, title, author, base branch, and linked issues.
5. Inspect only this branch's diff against its target/merge base.
6. Read the top few changelog entries for style.
7. Add or update the entry in the top unpublished section only.
8. If the same PR number is already listed, update that entry instead of adding a duplicate.

## Fast Path

Prefer this order:

1. `git status --short --branch`
2. Use the current branch and upstream as head-ref hints for PR lookup.
3. Fetch authoritative PR metadata for the matching PR.
4. Diff against the PR base branch from metadata, usually `origin/main`.

Branch names are contributor-controlled and may contain inaccurate issue or PR numbers. Never treat a number in a branch name as authoritative.

If Git reports dubious ownership, add the repository to `safe.directory` once, then retry the Git command.

## Minimal Diff Inspection

For small PRs, inspect:

- PR title and body.
- Changed file list.
- Diff stat.
- Relevant patch only if the title, body, and changed file list are insufficient.

Do not inspect full diffs when PR metadata clearly describes a small mechanical fix.

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
