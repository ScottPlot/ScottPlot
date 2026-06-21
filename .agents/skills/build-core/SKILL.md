---
name: build-core
description: Build the core ScottPlot 5 library and unit-test project for routine validation after core or unit-test changes. Prefer this over full solution builds unless platform projects, controls, demos, or workloads are affected.
---

# Core Build

Run from repo root:

```sh
dotnet build "src/ScottPlot5/ScottPlot5/ScottPlot.csproj"
dotnet build "src/ScottPlot5/ScottPlot5 Tests/Unit Tests/ScottPlot Unit Tests.csproj"
```

Use `tests-run` for test execution and `build-full` only for solution-wide/platform validation.
