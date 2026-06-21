---
name: build-full
description: Build the full ScottPlot 5 solution only for solution-wide validation or changes affecting controls, demos, sandboxes, workloads, or platform projects. For core or unit-test-only changes, use build-core.
---

# Full Build

Run from `src/ScottPlot5`:

```sh
dotnet workload restore
dotnet restore
dotnet build --configuration Release
```

If the full build succeeds and tests are requested, run:

```sh
dotnet test --configuration Release --no-build --verbosity minimal
```

If SDK/workload/network setup blocks this, report the blocker; do not silently substitute a narrower build.
