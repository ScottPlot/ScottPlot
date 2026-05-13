# Documentation / Ask Rules (Non-Obvious)

- **`ARCHITECTURE.md` is the canonical architecture reference** — it contains the only maintained architecture diagram and component map for this project. See [`ARCHITECTURE.md`](ARCHITECTURE.md).
- **The Cookbook is auto-generated from source code** — cookbook pages at `scottplot.net/cookbook/5.0/` are generated from `src/ScottPlot5/ScottPlot5 Cookbook/Recipes/`. Recipe source code IS the documentation. See [`RecipeBase.cs`](src/ScottPlot5/ScottPlot5%20Cookbook/RecipeBase.cs) for the base class.
- **`PlottableAdder.cs` is the "API index"** — every plottable type has a corresponding `Add.Xxx()` method here. This is the best file to search when learning what plot types are available, because it lists ALL plottables in one place (~1580 lines, alphabetized).
- **"Controls" (plural) means UI integration packages** — `ScottPlot5 Controls/` contains platform-specific NuGet packages (WinForms, WPF, etc.), NOT reusable WPF controls. Do not confuse this with WPF `UserControl` terminology.
- **`AxisManager.DateTimeTicksBottom()` replaces all bottom axes** — it removes ALL existing bottom axes, creates a `DateTimeXAxis`, updates plottable references, and re-autoscales. This is a destructive operation, not additive. See [`AxisManager.cs:253`](src/ScottPlot5/ScottPlot5/AxisManager.cs:253).
- **`PlottableAdder` auto-color cycling resets on empty plot** — if `PlottableList.Count == 0`, `NextColorIndex` resets to 0. This means 'first plottable added to an empty plot' always gets the first palette color.
- **The `dev/` directory is NOT source code** — it contains development scripts (batch files for version bumping, cookbook generation, demo generation) and Python helper scripts. These are build tools, not part of the library.
