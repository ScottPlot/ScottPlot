using ScottPlotCookbook.Recipes;

namespace ScottPlotCookbook.Website;

/// <summary>
/// This data structure contains information about a single recipe
/// and should be the only source of truth for generating filenames and URLs
/// </summary>
public struct RecipeInfo
{
    public string Chapter { get; }
    public string Category { get; }
    public string Name { get; }
    public string Description { get; }
    public string Source { get; }
    public string RecipeClassName { get; }
    public string CategoryClassName { get; }
    public readonly string SourceFilePath { get; }

    public readonly string CookbookUrl = "/cookbook/5.0";
    public readonly string CategoryUrl => $"{CookbookUrl}/{CategoryClassName}";
    public readonly string AnchoredCategoryUrl => $"{CategoryUrl}#{RecipeClassName}";
    public readonly string RecipeUrl => $"{CookbookUrl}/{CategoryClassName}/{RecipeClassName}";
    public readonly string MarkdownFilename => $"{CategoryClassName}-{RecipeClassName}.md";
    public readonly string ImageUrl => $"{CookbookUrl}/images/{RecipeClassName}.png";
    public readonly string Sourceurl => $"https://github.com/ScottPlot/ScottPlot/blob/main/{SourceFilePath}";

    public RecipeInfo(string chapter, string category, string name, string description, string source, string categoryClassName, string recipeClassName, string sourceFilePath)
    {
        Chapter = chapter;
        Category = category;
        Name = name;
        Description = description;
        Source = source;
        CategoryClassName = categoryClassName;
        RecipeClassName = recipeClassName;
        SourceFilePath = sourceFilePath;
    }

    public override readonly string ToString()
    {
        return $"[{Chapter}] [{Category}] {Name}";
    }
}
