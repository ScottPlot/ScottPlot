namespace ScottPlotCookbook.Info;

public class RecipeInfo
{
    public string Name { get; }
    public string PageName { get; }
    public string Description { get; }
    public string ImageFilename { get; }
    public string AnchorName { get; }
    public string SourceCode { get; private set; } = string.Empty;
    public IRecipe Recipe { get; }

    internal RecipeInfo(IRecipe recipe, PageInfo page)
    {
        Name = recipe.Name;
        Description = recipe.Description;
        ImageFilename = $"{UrlTools.UrlSafe(Name)}.png";
        AnchorName = UrlTools.UrlSafe(Name);
        Recipe = recipe;
        PageName = page.Name;
    }

    internal void AddSource(List<RecipeSource> sources)
    {
        foreach (RecipeSource source in sources)
        {
            if (source.PageName == PageName && source.RecipeName == Name)
            {
                SourceCode = source.SourceCode;
                break;
            }
        }
    }
}
