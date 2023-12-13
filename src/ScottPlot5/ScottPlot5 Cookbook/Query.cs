namespace ScottPlotCookbook;

/// <summary>
/// Public-facing functions for obtaining cookbook recipes and sections
/// </summary>
public static class Query
{
    public static IEnumerable<ICategory> GetCategoryClasses()
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

        foreach (ICategory categoryClass in GetCategoryClasses())
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

    public static Dictionary<ICategory, IEnumerable<WebRecipe>> GetWebRecipesByCategory()
    {
        SourceDatabase sb = new();

        Dictionary<ICategory, IEnumerable<WebRecipe>> recipesByCategory = new();

        foreach (ICategory categoryClass in GetCategoryClasses())
        {
            recipesByCategory[categoryClass] = sb.Recipes
                .Where(x => x.Category == categoryClass.CategoryName)
                .ToList();
        }

        return recipesByCategory;
    }
}
