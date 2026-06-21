---
name: tests-run
description: Run or add ScottPlot 5 tests. Use for unit-test and cookbook-test work; unless explicitly asked otherwise, restrict manual test execution to the Unit Tests and Cookbook projects.
---

# ScottPlot Unit Tests

Allowed manual test projects:

```powershell
dotnet test "src/ScottPlot5/ScottPlot5 Tests/Unit Tests/ScottPlot Unit Tests.csproj"
dotnet test "src/ScottPlot5/ScottPlot5 Cookbook/ScottPlot Cookbook.csproj"
```

For iteration, prefer a focused NUnit filter:

```powershell
dotnet test "src\ScottPlot5\ScottPlot5 Tests\Unit Tests\ScottPlot Unit Tests.csproj" --filter "FullyQualifiedName~CoordinateTests"
dotnet test "src\ScottPlot5\ScottPlot5 Cookbook\ScottPlot Cookbook.csproj" --filter "FullyQualifiedName~RecipeTests"
```

When adding tests, read nearby coverage first. Place tests near related code, match namespace/folder style, use NUnit `[Test]`/`[TestCase]`, prefer `Assert.That(...)` and `Assert.Multiple(...)`, and avoid new FluentAssertions usage. Name tests like `Test_Component_Behavior`. For render coverage, reuse existing helpers such as `plt.Should().RenderInMemoryWithoutThrowing()`, `plt.Should().SavePngWithoutThrowing()`, or `plt.SaveTestImage()`. Keep tests deterministic and focused.
