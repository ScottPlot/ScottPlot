using System.Reflection;

namespace ScottPlotCookbook;

public static class Cookbook
{
    public static List<IRecipe> GetRecipes() => GetInstantiated<IRecipe>();

    public static List<IRecipe> GetRecipes(RecipePage page) => page
        .GetType()
        .GetNestedTypes(BindingFlags.NonPublic)
        .Where(x => typeof(IRecipe).IsAssignableFrom(x))
        .Select(x => (IRecipe)(Activator.CreateInstance(x) ?? throw new NullReferenceException()))
        .ToList();

    internal static List<RecipePage> GetPages() => GetInstantiated<RecipePage>();

    internal static List<RecipeChapter> GetChapters() => GetInstantiated<RecipePage>()
        .Select(x => x.Chapter)
        .Distinct()
        .ToList();

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
