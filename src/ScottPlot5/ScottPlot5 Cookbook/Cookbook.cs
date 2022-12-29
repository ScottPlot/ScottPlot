using NUnit.Framework.Internal.Execution;
using System.Reflection;

namespace ScottPlotCookbook;

/// <summary>
/// These functions are used to locate components internally.
/// Consumers of the cookbook will interact with the <see cref="Query"/> class to get this information.
/// </summary>
public static class Cookbook
{
    public static readonly string OutputFolder = Path.Combine(GetRepoFolder(), "dev/www/cookbook/5.0");

    public static readonly string RecipeSourceFolder = Path.Combine(GetRepoFolder(), "src/ScottPlot5/ScottPlot5 Cookbook/Recipes");

    public static int ImageWidth = 400;

    public static int ImageHeight = 300;

    private static string GetRepoFolder()
    {
        string defaultFolder = Path.GetFullPath(TestContext.CurrentContext.TestDirectory); ;
        string? repoFolder = defaultFolder;
        while (repoFolder is not null)
        {
            if (File.Exists(Path.Combine(repoFolder, "LICENSE")))
            {
                return repoFolder;
            }
            else
            {
                repoFolder = Path.GetDirectoryName(repoFolder);
            }
        }

        throw new InvalidOperationException($"repository folder not found in any folder above {defaultFolder}");
    }

    /// <summary>
    /// Return a collection of chapters.
    /// Chapters are groups of pages (each page has many recipes).
    /// </summary>
    public static List<Chapter> GetChapters() => Enum.GetValues<Chapter>().ToList();

    /// <summary>
    /// Return a list of all pages (each page has many recipes) not grouped by chapter.
    /// This function should only be used for running tests on all pages.
    /// </summary>
    internal static List<RecipePageBase> GetPages() => GetInstantiated<RecipePageBase>();

    /// <summary>
    /// Return all pages for the given chapter.
    /// Each page contains many recipes.
    /// </summary>
    public static List<RecipePageBase> GetPagesInChapter(Chapter chapter) => GetInstantiated<RecipePageBase>()
        .Where(x => x.PageDetails.Chapter == chapter)
        .ToList();

    public static List<IRecipe> GetRecipes() => GetPages().SelectMany(x => x.GetRecipes()).ToList();

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
