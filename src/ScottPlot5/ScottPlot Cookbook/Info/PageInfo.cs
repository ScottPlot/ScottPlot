namespace ScottPlotCookbook.Info;

public class PageInfo
{
    public string Name { get; }
    public string Description { get; }
    public List<RecipeInfo> Recipes { get; }
    public string FolderUrl { get; }

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
