using ScottPlotCookbook.Recipes;

namespace ScottPlotCookbook;

public static class Query
{
    public static string[] GetChapterNamesInOrder()
    {
        return new string[]
        {
            "Quickstart",
            "Introduction",
            "Axis",
            "Plot Types",
            "Statistics",
        };
    }
    
    // TODO: private
    public static IEnumerable<ICategory> GetCategories()
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract)
            .Where(type => typeof(ICategory).IsAssignableFrom(type))
            .Select(type => Activator.CreateInstance(type))
            .Cast<ICategory>();
    }

    public static Dictionary<ICategory, IEnumerable<IRecipe>> GetRecipesByCategory()
    {
        Dictionary<ICategory, IEnumerable<IRecipe>> recipesByCategory = new();

        foreach (ICategory categoryClass in GetCategories())
        {
            recipesByCategory[categoryClass] = categoryClass
                .GetType()
                .GetNestedTypes()
                .Where(type => typeof(IRecipe).IsAssignableFrom(type))
                .Select(type => Activator.CreateInstance(type))
                .Cast<IRecipe>();
        }

        return recipesByCategory;
    }
}
