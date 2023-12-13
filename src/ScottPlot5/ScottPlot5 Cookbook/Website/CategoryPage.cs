namespace ScottPlotCookbook.Website;

internal class CategoryPage : PageBase
{
    private readonly Dictionary<ICategory, IEnumerable<WebRecipe>> RecipesByCategory;
    private readonly ICategory Category;

    internal CategoryPage(Dictionary<ICategory, IEnumerable<WebRecipe>> rbc, ICategory category)
    {
        RecipesByCategory = rbc;
        Category = category;
    }

    public void Generate(string outputFolder)
    {
        SB.AppendLine($"# {Category.CategoryName}");
        SB.AppendLine();

        AddVersionInformation();

        WebRecipe firstRecipe = RecipesByCategory[Category].First();

        foreach (WebRecipe recipe in RecipesByCategory[Category])
        {
            SB.AppendLine();
            SB.AppendLine($"## {recipe.Name}");
            SB.AppendLine();
            SB.AppendLine(recipe.Description);
            SB.AppendLine();
            SB.AppendLine($"[![]({recipe.ImageUrl})]({recipe.ImageUrl})");
            SB.AppendLine();
            SB.AppendLine("```cs");
            SB.AppendLine(recipe.Source);
            SB.AppendLine("```");
            SB.AppendLine();
        }

        string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        string breadcrumbUrl1 = "/cookbook/5.0/";

        string breadcrumbName2 = Category.CategoryName;
        string breadcrumbUrl2 = $"/cookbook/5.0/{firstRecipe.CategoryUrl}/";

        string[] fm =
        {
            $"BreadcrumbNames: [\"{breadcrumbName1}\", \"{breadcrumbName2}\"]",
            $"BreadcrumbUrls: [\"{breadcrumbUrl1}\", \"{breadcrumbUrl2}\"]",
        };

        Save(outputFolder,
            title: Category.CategoryName + " - ScottPlot 5.0 Cookbook",
            description: Category.CategoryDescription,
            filename: $"{firstRecipe.CategoryClassName}.md",
            url: firstRecipe.CategoryUrl,
            fm);
    }
}
