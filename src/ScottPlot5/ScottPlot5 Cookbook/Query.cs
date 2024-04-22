namespace ScottPlotCookbook;

public static class Query
{
    public static readonly Dictionary<ICategory, IEnumerable<IRecipe>> RecipesByCategory = GetRecipesByCategory();

    public readonly record struct RecipeInfo(string Chapter, ICategory Category, IRecipe Recipe);

    public static List<RecipeInfo> GetRecipes()
    {
        List<RecipeInfo> list = new();

        Dictionary<string, ICategory> categoriesByName = RecipesByCategory.ToDictionary(x => x.Key.CategoryName, x => x.Key);

        foreach (string categoryName in categoriesByName.Keys)
        {
            ICategory category = categoriesByName[categoryName];

            foreach (IRecipe recipe in RecipesByCategory[category])
            {
                list.Add(new(category.Chapter, category, recipe));
            }
        }

        return list;
    }

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

        foreach (ICategory category in GetCategories())
        {
            recipesByCategory[category] = category
                .GetType()
                .GetNestedTypes()
                .Where(type => typeof(IRecipe).IsAssignableFrom(type))
                .Select(type => Activator.CreateInstance(type))
                .Cast<IRecipe>();
        }

        return recipesByCategory;
    }
}
