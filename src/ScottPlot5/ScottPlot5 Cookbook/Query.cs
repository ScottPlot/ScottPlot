using ScottPlotCookbook.Recipes;

namespace ScottPlotCookbook;

public static class Query
{
    public static string[] GetChapterNamesInOrder()
    {
        return new string[]
        {
            "Introduction",
            "Axis",
            "Plot Types",
            "Statistics",
            "Miscellaneous"
        };
    }

    // TODO: private
    public static IEnumerable<ICategory> GetCategories()
    {
        List<ICategory> categories = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract)
            .Where(type => typeof(ICategory).IsAssignableFrom(type))
            .Select(type => Activator.CreateInstance(type))
            .Cast<ICategory>()
            .ToList();

        foreach (string name in GetChapterNamesInOrder().Reverse())
        {
            ICategory? match = categories
                .Where(x => string.Equals(x.Chapter, name, StringComparison.InvariantCultureIgnoreCase))
                .First();

            categories.Remove(match);
            categories.Insert(0, match);
        }

        return categories;
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
