using System.Reflection;

namespace ScottPlotCookbook.Recipes;

public abstract class RecipePageBase
{
    public abstract RecipePageDetails PageDetails { get; }

    public List<Recipe> GetRecipes() => GetType()
        .GetNestedTypes(BindingFlags.NonPublic)
        .Where(x => typeof(Recipe).IsAssignableFrom(x))
        .Select(x => (Recipe)(Activator.CreateInstance(x) ?? throw new NullReferenceException()))
        .ToList();
}
