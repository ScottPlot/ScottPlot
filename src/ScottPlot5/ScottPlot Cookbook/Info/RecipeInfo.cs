namespace ScottPlotCookbook.Info;

public class RecipeInfo
{
    public string Name { get; }
    public string Description { get; }
    public string ImageUrl { get; }
    public string SourceCode { get; } = string.Empty;
    public IRecipe Recipe { get; }

    internal RecipeInfo(IRecipe recipe, PageInfo page)
    {
        Name = recipe.Name;
        Description = recipe.Description;
        ImageUrl = $"{page.FolderUrl}/{Html.UrlSafe(Name)}.png";
        Recipe = recipe;
    }
}
