---
name: cookbook-add-recipe
description: Add or extend a ScottPlot Cookbook recipe for an IPlottable, finding the right PlotTypes category or creating one when needed, with minimal style-matched examples.
---

# Cookbook Recipe From IPlottable

If no plottable is named, ask before editing.

Paths: plottables in `src/ScottPlot5/ScottPlot5/Plottables`; recipes in `src/ScottPlot5/ScottPlot5 Cookbook/Recipes/PlotTypes`.

Fast path:
1. Read the target plottable public API.
2. Search existing PlotTypes recipes and read adjacent style.
3. Add a nested `RecipeBase` to an existing category, or create a matching `ICategory` file in `Recipes/PlotTypes`.
4. Keep code minimal, deterministic, and documentation-friendly.
5. Run focused cookbook tests.

```sh
rg "class .*:.*IPlottable|class TARGET" src/ScottPlot5/ScottPlot5
rg "TARGET|CategoryName =>" "src/ScottPlot5/ScottPlot5 Cookbook/Recipes/PlotTypes"
```

New category basics: namespace `ScottPlotCookbook.Recipes.PlotTypes`, implement `ICategory`, `Chapter => Chapter.PlotTypes`, one quickstart recipe unless the requested behavior needs something else. Prefer `myPlot.Add.*()` APIs and deterministic `Generate` data. Avoid unnecessary titles, labels, legends, limits, styling, comments, refactors, or cleanup.

Validation:

```sh
dotnet test "src/ScottPlot5/ScottPlot5 Cookbook/ScottPlot Cookbook.csproj" --filter "FullyQualifiedName~RecipeTests"
```

If the new recipe must render during validation, run the cookbook project without the filter. Report the command and result.
