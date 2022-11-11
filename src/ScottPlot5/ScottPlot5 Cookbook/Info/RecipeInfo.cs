namespace ScottPlotCookbook.Info;

public class RecipeInfo
{
    public string Name { get; }
    public string Description { get; }
    public string ImageFilename { get; }
    public string AnchorName { get; }
    public string SourceCode { get; } = string.Empty;
    public IRecipe Recipe { get; }

    internal RecipeInfo(IRecipe recipe, PageInfo page)
    {
        Name = recipe.Name;
        Description = recipe.Description;
        ImageFilename = $"{UrlTools.UrlSafe(Name)}.png";
        AnchorName = $"#{UrlTools.UrlSafe(Name)}";
        Recipe = recipe;
    }
}
