---
name: changelog-update-pr
description: "Update root CHANGELOG.md for exactly the current branch's pull request in ScottPlot. Use when on a contributor PR branch and asked to add or update one concise unpublished changelog entry without summarizing unrelated changes."
---

# PR Changelog

Edit only `CHANGELOG.md`.

Fast path:
1. Run `git status --short --branch`.
2. Use branch/upstream only as hints to find the PR; never trust numbers in branch names.
3. Fetch PR metadata: number, title, author, base branch, linked issues.
4. Inspect the smallest useful scope, usually changed filenames and `git diff --stat <base>...HEAD`; read patches only if metadata and filenames are unclear.
5. In the top unpublished section, update an existing `#PR` bullet or append one bullet to the current list.

Rules:
- Summarize only this PR.
- Keep the entry concise and user-facing.
- Add new bullets to the end of the latest block.
- Do not modify text outside this bullet before asking.
- Include `(#PR, #issue) @author` when known.

Style:
```markdown
* Component: Add/Fix/Improve concise description (#PR, #issue) @author
```

Choose `Component` from the PR's changed public API, feature, control, cookbook area, or affected file names. Prefer the smallest accurate user-facing name; do not default to a memorized category list. Reuse nearby changelog component wording only when it clearly matches this PR.

Final response: report the text added to the changelog.
