using System.Reflection;

namespace ScottPlotCookbook;

public static class Cookbook
{
    public static List<IRecipe> GetRecipes() => GetInstantiated<IRecipe>();

    internal static List<RecipePage> GetPages() => GetInstantiated<RecipePage>();

    internal static List<RecipeChapter> GetChapters() => GetInstantiated<RecipePage>().Select(x => x.Chapter).Distinct().ToList();

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
                    object instance = Activator.CreateInstance(assemblyType) ?? throw new NullReferenceException();
                    T page = (T)instance;
                    pages.Add(page);
                }
            }
        }

        return pages;
    }
}
