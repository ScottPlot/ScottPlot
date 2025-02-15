using ScottPlotCookbook.Recipes.Miscellaneous;
using System.Reflection;

namespace ScottPlotCookbook;

public static class Query
{
    public static readonly Dictionary<ICategory, IEnumerable<IRecipe>> RecipesByCategory = GetRecipesByCategory();

    public readonly record struct RecipeInfo(Chapter Chapter, ICategory Category, IRecipe Recipe);

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

    public static Chapter[] GetChaptersInOrder()
    {
        return
        [
            Chapter.General,
            Chapter.PlotTypes,
        ];
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

        foreach (Chapter chapter in GetChaptersInOrder().Reverse())
        {
            ICategory? match = categories
                .Where(category => category.Chapter == chapter)
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

    public static Dictionary<string, string> GetMultiplotDescriptions()
    {
        return typeof(MultiplotRecipes)
            .GetNestedTypes()
            .Where(type => type.IsSubclassOf(typeof(MultiplotRecipeBase)))
            .Select(type => Activator.CreateInstance(type))
            .Cast<MultiplotRecipeBase>()
            .ToDictionary(x => x.Name, x => x.Description);
    }
}
