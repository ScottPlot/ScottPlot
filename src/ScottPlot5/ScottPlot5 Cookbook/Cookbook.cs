using System.Reflection;

namespace ScottPlotCookbook;

/// <summary>
/// These functions are used to locate components internally.
/// Consumers of the cookbook will interact with the <see cref="Query"/> class to get this information.
/// </summary>
public static class Cookbook
{
    public static readonly string OutputFolder = GetCookbookFolder();

    private static string GetCookbookFolder()
    {
        string defaultFolder = Path.GetFullPath(TestContext.CurrentContext.TestDirectory);
        string cookbookOutputSubFolder = "www/cookbook/5.0";

        string? repoFolder = defaultFolder;
        while (repoFolder is not null)
        {
            if (File.Exists(Path.Combine(repoFolder, "LICENSE")))
            {
                return Path.Combine(repoFolder, cookbookOutputSubFolder);
            }
            else
            {
                repoFolder = Path.GetDirectoryName(repoFolder);
            }
        }

        return Path.Combine(defaultFolder, cookbookOutputSubFolder);
    }

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
