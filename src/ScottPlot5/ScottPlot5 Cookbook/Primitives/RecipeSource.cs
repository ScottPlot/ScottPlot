namespace ScottPlotCookbook;

/// <summary>
/// Information about a cookbook recipe read from a JSON file
/// </summary>
internal readonly struct RecipeSource
{
    public string PageName { get; }
    public string RecipeName { get; }
    public string SourceCode { get; }

    public RecipeSource(string page, string recipe, string source)
    {
        PageName = page;
        RecipeName = recipe;
        SourceCode = source;
    }
}
