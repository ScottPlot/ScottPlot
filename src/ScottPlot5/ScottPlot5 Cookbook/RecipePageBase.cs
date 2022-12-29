using System.Reflection;

namespace ScottPlotCookbook;

public abstract class RecipePageBase
{
    public abstract RecipePageDetails PageDetails { get; }

    public List<IRecipe> GetRecipes() => GetType()
        .GetNestedTypes(BindingFlags.NonPublic)
        .Where(x => typeof(IRecipe).IsAssignableFrom(x))
        .Select(x => (IRecipe)(Activator.CreateInstance(x) ?? throw new NullReferenceException()))
        .ToList();
}
