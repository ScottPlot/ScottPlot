---
name: tests-run
description: Run and contribute to ScottPlot 5 unit tests. Use when adding, editing, debugging, or manually running tests in ScottPlot; restrict manual test execution to src/ScottPlot5/ScottPlot5 Tests/Unit Tests/ and src/ScottPlot5/ScottPlot5 Cookbook/ unless the user explicitly asks for another test project.
---

# ScottPlot Unit Tests

Use only these ScottPlot 5 test projects for manual test runs:

```powershell
dotnet test "src/ScottPlot5/ScottPlot5 Tests/Unit Tests/ScottPlot Unit Tests.csproj"
dotnet test "src/ScottPlot5/ScottPlot5 Cookbook/ScottPlot Cookbook.csproj"
```

For iteration, prefer a focused NUnit filter:

```powershell
dotnet test "src\ScottPlot5\ScottPlot5 Tests\Unit Tests\ScottPlot Unit Tests.csproj" --filter "FullyQualifiedName~CoordinateTests"
dotnet test "src\ScottPlot5\ScottPlot5 Cookbook\ScottPlot Cookbook.csproj" --filter "FullyQualifiedName~RecipeTests"
```

Do not manually run other test projects unless the user explicitly overrides this skill.

## Adding Tests

- Add tests under `src\ScottPlot5\ScottPlot5 Tests\Unit Tests`, near related coverage.
- Add cookbook tests under `src\ScottPlot5\ScottPlot5 Cookbook`, near related cookbook coverage.
- Match existing namespace folders, e.g. `ScottPlotTests.UnitTests...`, `ScottPlotTests.RenderTests...`, `ScottPlotTests.Statistics...`.
- Use NUnit `[Test]` and `[TestCase]`.
- Do not add new FluentAssertions usage; its current license model is undesirable for this project.
- Prefer modern NUnit assertion APIs for new and changed assertions, e.g. `Assert.That(value, Is.EqualTo(expected))`, `Assert.That(value, Is.Not.Null)`, and `Assert.Multiple(...)` when grouping related checks.
- Name tests like `Test_Component_Behavior`.
- For plot rendering coverage, use existing helpers: `plt.Should().RenderInMemoryWithoutThrowing()`, `plt.Should().SavePngWithoutThrowing()`, or `plt.SaveTestImage()`.
- Keep tests deterministic, small, and behavior-focused; avoid broad snapshots unless the surrounding render tests already use image output.

Before editing, read nearby tests and mimic their style, assertions, helpers, and file organization.
