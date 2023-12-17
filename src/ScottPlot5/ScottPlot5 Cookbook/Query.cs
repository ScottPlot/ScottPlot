using ScottPlotCookbook.Recipes;
using ScottPlotCookbook.Website;

namespace ScottPlotCookbook;

public static class Query
{
    public static IEnumerable<IRecipe> GetInstantiatedRecipes()
    {
        return GetRecipesByCategory().Values.SelectMany(x => x);
    }

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

    public static IEnumerable<IRecipe> GetRecipeClasses()
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract)
            .Where(type => typeof(IRecipe).IsAssignableFrom(type))
            .Select(type => Activator.CreateInstance(type))
            .Cast<IRecipe>();
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

    public static Dictionary<ICategory, IEnumerable<RecipeInfo>> GetWebRecipesByCategory()
    {
        Dictionary<ICategory, IEnumerable<RecipeInfo>> recipesByCategory = new();

        foreach (ICategory categoryClass in GetCategoryClasses())
        {
            recipesByCategory[categoryClass] = categoryClass
                .GetType()
                .GetNestedTypes()
                .Where(type => typeof(IRecipe).IsAssignableFrom(type))
                .Select(type => Activator.CreateInstance(type))
                .Cast<IRecipe>()
                .Select(recipe => new RecipeInfo(
                    chapter: categoryClass.Chapter,
                    category: categoryClass.CategoryName,
                    name: recipe.Name,
                    description: recipe.Description,
                    source: string.Empty,
                    categoryClassName: string.Empty,
                    recipeClassName: string.Empty,
                    sourceFilePath: string.Empty));
        }

        return recipesByCategory;
    }
}
