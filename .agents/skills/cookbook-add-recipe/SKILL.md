---
name: cookbook-add-recipe
description: "Create or extend a ScottPlot Cookbook recipe for an IPlottable. Use when adding documentation recipes for ScottPlot plottables, including finding the right cookbook category, creating a new category if needed, and keeping examples minimal and style-matched."
---

# Cookbook Recipe From IPlottable

Create a ScottPlot Cookbook recipe for a specific `IPlottable`.

If the user does not name the plottable in their prompt, ask which `IPlottable` should be added to the cookbook before editing files.

## Key Paths

- Plottables: `src\ScottPlot5\ScottPlot5\Plottables`
- Cookbook recipes: `src\ScottPlot5\ScottPlot5 Cookbook\Recipes\PlotTypes`
- Cookbook project: `src\ScottPlot5\ScottPlot5 Cookbook\ScottPlot Cookbook.csproj`

## Workflow

1. Identify the target `IPlottable`.
2. Read its implementation and public API.
3. Search for an existing cookbook class in `src\ScottPlot5\ScottPlot5 Cookbook\Recipes\PlotTypes`.
4. Read adjacent cookbook recipe files before editing.
5. If a cookbook class already exists for this plottable, add a new nested `RecipeBase` class at the bottom of that class.
6. If no cookbook class exists, create a new file and `ICategory` class that mimics adjacent PlotTypes recipe files.
7. Keep the recipe minimal, deterministic, and documentation-friendly.
8. Run the focused cookbook tests.

Use `rg` to find related code:

```powershell
rg "class .*:.*IPlottable|class TARGET" src\ScottPlot5\ScottPlot5
rg "TARGET|CategoryName =>" "src\ScottPlot5\ScottPlot5 Cookbook\Recipes\PlotTypes"
```

## Recipe Style

Cookbook files in `Recipes\PlotTypes` typically use this shape:

```csharp
namespace ScottPlotCookbook.Recipes.PlotTypes;

public class CategoryClassName : ICategory
{
    public Chapter Chapter => Chapter.PlotTypes;
    public string CategoryName => "Display Name";
    public string CategoryDescription => "Short category description.";

    public class RecipeClassName : RecipeBase
    {
        public override string Name => "Recipe Name";
        public override string Description => "Short documentation-oriented description.";

        [Test]
        public override void Execute()
        {
            // minimal example code
        }
    }
}
```

Match the style of nearby recipe classes over this template whenever they differ.

## New Classes

When creating a new cookbook file:

- Put it in `src\ScottPlot5\ScottPlot5 Cookbook\Recipes\PlotTypes`.
- Use namespace `ScottPlotCookbook.Recipes.PlotTypes`.
- Implement `ICategory`.
- Set `Chapter` to `Chapter.PlotTypes`.
- Choose `CategoryName` and `CategoryDescription` that explain the plottable in plain documentation language.
- Add one quickstart recipe unless the requested behavior clearly calls for a more specific recipe.

## Existing Classes

When adding to an existing cookbook class:

- Add the new nested recipe class at the bottom of the existing `ICategory` class.
- Preserve existing recipe order and formatting.
- Avoid broad refactors, renames, or unrelated cleanup.

## Recipe Code Guidelines

- Code examples are copied onto the web, so keep them as small as practical.
- Prefer `myPlot.Add.*()` APIs when they exist.
- Prefer deterministic sample data from `Generate`.
- Avoid unnecessary titles, labels, legends, axis limits, and styling.
- Add inline comments only when they clarify something a reader would not immediately infer.
- Use short comments that explain intent, not mechanics.
- Avoid comments that merely restate the next line.
- Set axis limits only when needed to make the rendered example readable.

## Validation

Run the cookbook tests after editing:

```powershell
dotnet test "src\ScottPlot5\ScottPlot5 Cookbook\ScottPlot Cookbook.csproj" --filter "FullyQualifiedName~RecipeTests"
```

If the new recipe itself should be rendered during validation, run the cookbook project without a narrow filter:

```powershell
dotnet test "src\ScottPlot5\ScottPlot5 Cookbook\ScottPlot Cookbook.csproj"
```

Report which command was run and whether it passed.
