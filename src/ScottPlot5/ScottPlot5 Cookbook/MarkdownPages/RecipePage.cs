namespace ScottPlotCookbook.MarkdownPages;

internal class RecipePage : PageBase
{
    private readonly RecipeInfo Recipe;

    internal RecipePage(RecipeInfo recipe)
    {
        Recipe = recipe;
    }

    public void Generate()
    {
        SB.AppendLine($"# {Recipe.Name}");
        SB.AppendLine();

        AddVersionInformation();

        SB.AppendLine();
        SB.AppendLine(Recipe.Description);
        SB.AppendLine();
        SB.AppendLine($"[![]({Recipe.ImageUrl})]({Recipe.ImageUrl})");
        SB.AppendLine();
        SB.AppendLine("```cs");
        SB.AppendLine(Recipe.SourceCode);
        SB.AppendLine("```");
        SB.AppendLine();

        string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        string breadcrumbUrl1 = "/cookbook/5.0/";

        string breadcrumbName2 = Recipe.Category;
        string breadcrumbUrl2 = Recipe.CategoryUrl;

        string breadcrumbName3 = Recipe.Name;
        string breadcrumbUrl3 = Recipe.Url;

        string[] fm =
        {
            $"BreadcrumbNames: [\"{breadcrumbName1}\", \"{breadcrumbName2}\", \"{breadcrumbName3}\"]",
            $"BreadcrumbUrls: [\"{breadcrumbUrl1}\", \"{breadcrumbUrl2}\", \"{breadcrumbUrl3}\"]",
        };

        string outputFolder = Path.Combine(Cookbook.OutputFolder, "recipes");

        Save(outputFolder,
            title: Recipe.Name + " - ScottPlot 5.0 Cookbook",
            description: Recipe.Description,
            filename: $"{Recipe.FolderName}.md",
            url: Recipe.Url,
            fm);
    }
}
