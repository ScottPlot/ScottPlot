namespace ScottPlotCookbook.Website;

internal class CategoryPage : PageBase
{
    readonly JsonCookbookInfo CB;
    readonly JsonCookbookInfo.JsonCategoryInfo Category;

    public CategoryPage(JsonCookbookInfo cb, JsonCookbookInfo.JsonCategoryInfo category)
    {
        CB = cb;
        Category = category;
    }

    public void Generate(string outputFolder)
    {
        SB.AppendLine($"# {Category.Name}");
        SB.AppendLine();

        AddVersionInformation();

        foreach (var recipe in CB.Recipes.Where(x => x.Category == Category.Name))
        {
            SB.AppendLine();
            SB.AppendLine($"<h2><a href='{recipe.RecipeUrl}'>{recipe.Name}</a></h2>");
            SB.AppendLine();
            SB.AppendLine(recipe.Description);
            SB.AppendLine();
            SB.AppendLine($"[![]({recipe.ImageUrl})]({recipe.ImageUrl})");
            SB.AppendLine();
            SB.AppendLine("{{< code-sp5 >}}");
            SB.AppendLine();
            SB.AppendLine("```cs");
            SB.AppendLine(recipe.Source);
            SB.AppendLine("```");
            SB.AppendLine();
            SB.AppendLine("{{< /code-sp5 >}}");
            SB.AppendLine();
            SB.AppendLine("<hr class='my-5 invisible'>");
            SB.AppendLine();
        }

        string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        string breadcrumbUrl1 = "/cookbook/5.0/";

        string breadcrumbName2 = Category.Name;
        string breadcrumbUrl2 = Category.Url;

        string[] fm =
        {
            $"BreadcrumbNames: [\"{breadcrumbName1}\", \"{breadcrumbName2}\"]",
            $"BreadcrumbUrls: [\"{breadcrumbUrl1}\", \"{breadcrumbUrl2}\"]",
        };

        Save(outputFolder,
            title: Category.Name + " - ScottPlot 5.0 Cookbook",
            description: Category.Description,
            filename: $"{Path.GetFileName(Category.Url)}.md",
            url: Category.Url,
            fm);
    }
}
