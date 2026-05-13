# AGENTS.md

This file provides guidance to agents when working with code in this repository.

## Build & Run Commands

- **Restore**: `dotnet restore src/ScottPlot5/ScottPlot5.sln`
- **Build**: `dotnet build src/ScottPlot5/ScottPlot5.sln`
- **Run all tests**: `dotnet test src/ScottPlot5/ScottPlot5.sln`
- **Run single test**: `dotnet test src/ScottPlot5/ScottPlot5.sln --filter "FullyQualifiedName=Namespace.TestClass.TestMethod"`
- **Autoformat**: Run `autoformat.bat` in the relevant project directory (uses dotnet-format)

## Project-Specific Conventions

- **Target frameworks**: `net462;netstandard2.0;net8.0;net9.0;net10.0` — all new code must be compatible with all 5 TFMs. Use `#if` conditionals or target-conditional PackageReferences for platform-specific code.
- **Implicit usings disabled** — every file must have explicit `using` statements. The project does NOT use `<ImplicitUsings>enable</ImplicitUsings>`.
- **Nullable enabled** (`<Nullable>enable</Nullable>`) — all reference types must be properly annotated.
- **LangVersion 12** — C# 12 features (primary constructors, collection expressions) are available.
- **Assembly is strong-name signed** — uses `Key.snk` in `src/ScottPlot5/`.

## Core Architecture (Non-Obvious)

- **`Plot.Sync` lock object MUST be acquired before modifying `PlottableList` or axis limits** from background threads, or rendering artifacts will occur. See [`Plot.cs`](src/ScottPlot5/ScottPlot5/Plot.cs:50).
- **`RenderManager.RenderActions` is a public `List<IRenderAction>`** — the 25-step render pipeline is extensible. Injection order matters: pre-layout axis rules run at step 7, post-layout at step 10. See [`RenderManager.cs`](src/ScottPlot5/ScottPlot5/Rendering/RenderManager.cs:9).
- **`Render()` retries up to 5 times** when axis limits change during a render (auto-scale feedback loop). This is intentional for stabilizing multi-axis layouts.
- **`PlottableAdder.cs` methods MUST be in alphabetical order** — enforced by `CodeFormatTests.cs`. Adding a new method out of order will fail CI.
- **`ScottPlot.OpenGL` GL programs** must save/restore GL state via `StoreGLState`/`RestoreGLState` render actions (steps 14 & 16), otherwise SkiaSharp rendering breaks.
- **Each UI control package is a separate NuGet package** with its own `.csproj`. The core `ScottPlot` package has NO UI framework dependency.

## Testing

- Tests are in `src/ScottPlot5/ScottPlot5 Tests/Unit Tests/` using xUnit + FluentAssertions.
- `Graphical Test Runner` is a separate project for visual diff testing.
- Test images are stored in `TestImages/` subdirectory.

## Key Files for Understanding Code

- [`ARCHITECTURE.md`](ARCHITECTURE.md) — full architecture documentation with diagrams
- [`Plot.cs`](src/ScottPlot5/ScottPlot5/Plot.cs) — top-level API entry point
- [`PlottableAdder.cs`](src/ScottPlot5/ScottPlot5/PlottableAdder.cs) — factory for all plottable types
- [`IPlottable.cs`](src/ScottPlot5/ScottPlot5/Interfaces/IPlottable.cs) — core interface every plottable implements
- [`RenderManager.cs`](src/ScottPlot5/ScottPlot5/Rendering/RenderManager.cs) — render pipeline orchestration
- [`AxisManager.cs`](src/ScottPlot5/ScottPlot5/AxisManager.cs) — axis/zoom/pan management
