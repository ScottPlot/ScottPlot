namespace ScottPlotCookbook.Website;

internal class FrontPage : PageBase
{
    Dictionary<ICategory, IEnumerable<WebRecipe>> RecipesByCategory;

    public FrontPage(Dictionary<ICategory, IEnumerable<WebRecipe>> rbc)
    {
        RecipesByCategory = rbc;
    }

    public void Generate(string outputFolder)
    {
        SB.AppendLine($"# ScottPlot 5.0 Cookbook");
        SB.AppendLine();

        AddVersionInformation();

        // TODO: SORT BY CHAPTER

        foreach (ICategory category in RecipesByCategory.Keys)
        {
            AddCategory(category);
        }

        string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        string breadcrumbUrl1 = "/cookbook/5.0/";

        string[] fm =
        {
            $"BreadcrumbNames: [\"{breadcrumbName1}\"]",
            $"BreadcrumbUrls: [\"{breadcrumbUrl1}\"]",
        };

        Save(outputFolder,
            title: "ScottPlot 5.0 Cookbook",
            description: "Example plots shown next to the code used to create them",
            filename: "cookbook-5.0.md",
            url: "/cookbook/5.0/",
            frontmatter: fm);
    }

    private void AddCategory(ICategory category)
    {
        IEnumerable<WebRecipe> recipes = RecipesByCategory[category];
        string categoryUrl = recipes.First().CategoryUrl;

        SB.AppendLine($"<h2 class=''><a href='{categoryUrl}' class='text-dark'>{category.CategoryName}</a></h2>");
        SB.AppendLine($"<div>{category.CategoryDescription}</div>");

        foreach (WebRecipe recipe in RecipesByCategory[category])
        {
            AddRecipe(recipe);
        }
    }

    private void AddRecipe(WebRecipe recipe)
    {
        SB.AppendLine("<div class='row my-4'>");

        SB.AppendLine("<div class='col'>");
        SB.AppendLine($"<a href='{recipe.RecipeUrl}'><img class='img-fluid' src='{recipe.ImageUrl}' /></a>");
        SB.AppendLine("</div>");

        SB.AppendLine("<div class='col'>");
        SB.AppendLine($"<div><a href='{recipe.RecipeUrl}'><b>{recipe.Name}</b></a></div>");
        SB.AppendLine($"<div>{recipe.Description}</div>");
        SB.AppendLine("</div>");

        SB.AppendLine("</div>");
    }
}
