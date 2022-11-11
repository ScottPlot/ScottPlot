namespace ScottPlotCookbook;

internal struct RecipeSource
{
    public string PageName { get; set; }
    public string RecipeName { get; set; }
    public string SourceCode { get; set; }

    public RecipeSource(string page, string recipe, string source)
    {
        PageName = page;
        RecipeName = recipe;
        SourceCode = source;
    }
}
