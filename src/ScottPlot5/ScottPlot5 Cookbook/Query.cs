using ScottPlotCookbook.Recipes;

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

    public static List<CategoryInfo> GetCategories() =>
        GetChapters()
        .SelectMany(x => x.Categories)
        .ToList();

    public static List<RecipeInfo> GetRecipes() =>
        GetCategories()
        .SelectMany(x => x.RecipeInfos)
        .ToList();
}
