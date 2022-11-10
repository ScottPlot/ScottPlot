using System.Reflection;

namespace ScottPlotCookbook;

public static class Cookbook
{
    internal static List<Chapter> GetChapters() => Enum.GetValues<Chapter>().ToList();

    internal static List<RecipePageBase> GetPages() => GetInstantiated<RecipePageBase>();

    internal static List<RecipePageBase> GetPagesInChapter(Chapter chapter) => GetInstantiated<RecipePageBase>()
        .Where(x => x.PageDetails.Chapter == chapter)
        .ToList();

    internal static List<IRecipe> GetRecipes() => GetPages().SelectMany(x => x.GetRecipes()).ToList();

    private static List<T> GetInstantiated<T>()
    {
        List<T> pages = new();

        foreach (Assembly? assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type assemblyType in assembly.GetTypes())
            {
                if (assemblyType.IsAbstract || assemblyType.IsInterface)
                    continue;

                if (typeof(T).IsAssignableFrom(assemblyType))
                {
                    pages.Add((T)(Activator.CreateInstance(assemblyType) ?? throw new NullReferenceException()));
                }
            }
        }

        return pages;
    }
}
