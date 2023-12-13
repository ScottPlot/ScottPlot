using ScottPlotCookbook.Recipes;
using ScottPlotCookbook.Website;

namespace ScottPlotCookbook;

internal static class Query
{
    public static IEnumerable<string> GetChapterNamesInOrder(Dictionary<ICategory, IEnumerable<RecipeInfo>> rbc)
    {
        // todo: add logic for good order
        return rbc.Values.SelectMany(x => x).Select(x => x.Chapter).Distinct();
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
        SourceDatabase sb = new();

        Dictionary<ICategory, IEnumerable<RecipeInfo>> recipesByCategory = new();

        foreach (ICategory categoryClass in GetCategoryClasses())
        {
            recipesByCategory[categoryClass] = sb.Recipes
                .Where(x => x.Category == categoryClass.CategoryName)
                .ToList();
        }

        return recipesByCategory;
    }
}
