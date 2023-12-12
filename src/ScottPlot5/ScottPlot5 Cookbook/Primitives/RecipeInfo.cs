namespace ScottPlotCookbook;

// TODO: collapse this datatype with Recipe???

/// <summary>
/// Contains extensive information about a single cookbook recipe
/// </summary>
public class RecipeInfo
{
    public string Name { get; }
    public string Category { get; }
    public string CategoryFolderName => UrlTools.UrlSafe(Category);
    public string CategoryUrl => $"/cookbook/5.0/category/{CategoryFolderName}/";
    public string Description { get; }
    public string ImageFilename => $"{UrlTools.UrlSafe(Name)}.png";
    public string AnchorName { get; }
    public string SourceCode { get; private set; } = string.Empty;
    public Recipe Recipe { get; }
    public string ImageUrl => $"/cookbook/5.0/images/{ImageFilename}";
    public string FolderName => UrlTools.UrlSafe(Name);
    public string Url => $"/cookbook/5.0/recipes/{FolderName}/";

    internal RecipeInfo(Recipe recipe, CategoryInfo page)
    {
        Name = recipe.Name;
        Description = recipe.Description;
        AnchorName = UrlTools.UrlSafe(Name);
        Recipe = recipe;
        Category = page.Name;
    }

    public void AddSource(string source)
    {
        SourceCode = source;
    }

    // TODO: obsolete this

    /// <summary>
    /// Scan a collection of recipe sources and populate this recipe's source code from the matching one
    /// </summary>
    internal void AddSource(IEnumerable<RecipeSource> sources)
    {
        var matchingSources = sources.Where(x => x.Category == Category && x.Recipe.Name == Name);

        if (matchingSources.Any())
        {
            SourceCode = matchingSources.First().SourceCode;
        }
    }
}
