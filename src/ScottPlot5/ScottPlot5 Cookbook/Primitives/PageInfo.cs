namespace ScottPlotCookbook;

// TODO: collapse this datatype with Page???

/// <summary>
/// A cookbook page is a group of recipes.
/// Example: Signal Plots (contains a collection of recipes)
/// </summary>
public class PageInfo
{
    public string Name { get; }
    public string Description { get; }
    public List<RecipeInfo> RecipeInfos { get; }
    public string FolderUrl => UrlTools.UrlSafe(Name);
    public string Url => $"/cookbook/5.0/{FolderUrl}/";

    internal PageInfo(Recipes.RecipePageBase page)
    {
        Name = page.PageDetails.PageName;
        Description = page.PageDetails.PageDescription;
        RecipeInfos = page.GetRecipes()
            .Select(x => new RecipeInfo(x, this))
            .ToList();
    }
}
