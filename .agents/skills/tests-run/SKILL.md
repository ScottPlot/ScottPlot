---
name: tests-run
description: Run and contribute to ScottPlot 5 unit tests. Use when adding, editing, debugging, or manually running tests in ScottPlot; restrict manual test execution to src/ScottPlot5/ScottPlot5 Tests/Unit Tests/ and do not manually run other test projects.
---

# ScottPlot Unit Tests

Use only the ScottPlot 5 unit test project for manual test runs:

```powershell
dotnet test "src/ScottPlot5/ScottPlot5 Tests/Unit Tests/ScottPlot Unit Tests.csproj"
```

For iteration, prefer a focused NUnit filter:

```powershell
dotnet test "src\ScottPlot5\ScottPlot5 Tests\Unit Tests\ScottPlot Unit Tests.csproj" --filter "FullyQualifiedName~CoordinateTests"
```

Do not manually run other test projects unless the user explicitly overrides this skill.

## Adding Tests

- Add tests under `src\ScottPlot5\ScottPlot5 Tests\Unit Tests`, near related coverage.
- Match existing namespace folders, e.g. `ScottPlotTests.UnitTests...`, `ScottPlotTests.RenderTests...`, `ScottPlotTests.Statistics...`.
- Use NUnit `[Test]` and `[TestCase]`; prefer FluentAssertions for value assertions when nearby tests use it.
- Name tests like `Test_Component_Behavior`.
- For plot rendering coverage, use existing helpers: `plt.Should().RenderInMemoryWithoutThrowing()`, `plt.Should().SavePngWithoutThrowing()`, or `plt.SaveTestImage()`.
- Keep tests deterministic, small, and behavior-focused; avoid broad snapshots unless the surrounding render tests already use image output.

Before editing, read nearby tests and mimic their style, assertions, helpers, and file organization.