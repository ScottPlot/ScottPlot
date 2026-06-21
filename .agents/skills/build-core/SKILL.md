---
name: build-core
description: Build the core ScottPlot 5 project and the unit test project that depends on it. Use for routine validation after changes to src/ScottPlot5/ScottPlot5 or unit tests. Do not build the full solution: it has many targets and takes a long time to build.
---

# Core Build

Run from the repo root:

```powershell
dotnet build "src/ScottPlot5/ScottPlot5/ScottPlot.csproj"
dotnet build "src/ScottPlot5/ScottPlot5 Tests/Unit Tests/ScottPlot Unit Tests.csproj"
```

Use this for most code changes. It validates the package and the unit-test project without building controls, demos, sandboxes, benchmarks, or advanced test runners.

For test execution, use the `tests-run` skill. For solution-wide or platform-control validation, use `build-full`.
