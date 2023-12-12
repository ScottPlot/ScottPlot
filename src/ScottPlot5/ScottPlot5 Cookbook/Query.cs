namespace ScottPlotCookbook;

/// <summary>
/// Helper functions for obtaining cookbook recipes and sections
/// </summary>
public static class Query
{
    public static List<ChapterInfo> GetChapters() =>
        Cookbook.GetChapters()
        .Select(x => new ChapterInfo(x))
        .ToList();

    public static List<PageInfo> GetPages() =>
        GetChapters()
        .SelectMany(x => x.Pages)
        .ToList();

    public static List<RecipeInfo> GetRecipes() =>
        GetPages()
        .SelectMany(x => x.RecipeInfos)
        .ToList();
}
