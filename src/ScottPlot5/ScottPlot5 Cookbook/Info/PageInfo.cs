using ScottPlotCookbook.Recipes;

namespace ScottPlotCookbook.Info;

/// <summary>
/// A cookbook page is a group of recipes.
/// Example: Signal Plots (contains a collection of recipes)
/// </summary>
public class PageInfo
{
    public string Name { get; }
    public string Description { get; }
    public List<RecipeInfo> Recipes { get; }
    public string FolderUrl { get; }
    public string Url => $"/cookbook/5.0/{UrlTools.UrlSafe(Name)}/";

    internal PageInfo(RecipePageBase page)
    {
        Name = page.PageDetails.PageName;
        Description = page.PageDetails.PageDescription;
        FolderUrl = UrlTools.UrlSafe(Name);
        Recipes = page.GetRecipes()
            .Select(x => new RecipeInfo(x, this))
            .ToList();
    }
}
