namespace ScottPlotCookbook;

/// <summary>
/// Public-facing functions for obtaining cookbook recipes and sections
/// </summary>
public static class Query
{
    public static List<ChapterInfo> GetChapters() =>
        Cookbook.GetChapters()
        .Select(x => new ChapterInfo(x))
        .ToList();

    public static List<PageInfo> GetCategoryPages() =>
        GetChapters()
        .SelectMany(x => x.Pages)
        .ToList();

    public static List<RecipeInfo> GetRecipes() =>
        GetCategoryPages()
        .SelectMany(x => x.RecipeInfos)
        .ToList();
}
