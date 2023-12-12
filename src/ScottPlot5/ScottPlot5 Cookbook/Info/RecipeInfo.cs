namespace ScottPlotCookbook.Info;

/// <summary>
/// Contains extensive information about a single cookbook recipe
/// </summary>
public class RecipeInfo
{
    public string Name { get; }
    public string PageName { get; }
    public string Description { get; }
    public string ImageFilename { get; }
    public string AnchorName { get; }
    public string SourceCode { get; private set; } = string.Empty;
    public Recipe Recipe { get; }

    internal RecipeInfo(Recipe recipe, PageInfo page)
    {
        Name = recipe.Name;
        Description = recipe.Description;
        ImageFilename = $"{UrlTools.UrlSafe(Name)}.png";
        AnchorName = UrlTools.UrlSafe(Name);
        Recipe = recipe;
        PageName = page.Name;
    }

    /// <summary>
    /// Scan a collection of recipe sources and populate this recipe's source code from the matching one
    /// </summary>
    internal void AddSource(IEnumerable<RecipeSource> sources)
    {
        var matchingSources = sources.Where(x => x.PageName == PageName && x.RecipeName == Name);

        if (matchingSources.Any())
        {
            SourceCode = matchingSources.First().SourceCode;
        }
    }
}
