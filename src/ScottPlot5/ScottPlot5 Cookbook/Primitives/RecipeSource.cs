namespace ScottPlotCookbook;

/// <summary>
/// Information about a cookbook recipe read from a JSON file
/// </summary>
public readonly struct RecipeSource
{
    public string Category { get; }
    public string SourceCode { get; }
    public Recipe Recipe { get; }

    public RecipeSource(Recipe recipe, string category, string source)
    {
        Category = category;
        Recipe = recipe;
        SourceCode = source;
    }
}
