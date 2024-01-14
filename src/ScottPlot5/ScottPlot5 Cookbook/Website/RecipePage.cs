namespace ScottPlotCookbook.Website;

internal class RecipePage : PageBase
{
    readonly JsonCookbookInfo.JsonRecipeInfo Recipe;

    public RecipePage(JsonCookbookInfo.JsonRecipeInfo recipe)
    {
        Recipe = recipe;
    }

    public void Generate(string outputFolder)
    {
        SB.AppendLine($"# {Recipe.Name}");
        SB.AppendLine();

        AddVersionInformation();

        SB.AppendLine();
        SB.AppendLine(Recipe.Description);
        SB.AppendLine();
        SB.AppendLine($"[![]({Recipe.ImageUrl})]({Recipe.ImageUrl})");
        SB.AppendLine();
        SB.AppendLine("{{< code-sp5 >}}");
        SB.AppendLine();
        SB.AppendLine("```cs");
        SB.AppendLine(Recipe.Source);
        SB.AppendLine("```");
        SB.AppendLine();
        SB.AppendLine("{{< /code-sp5 >}}");
        SB.AppendLine();
        SB.AppendLine($"<a href='{Recipe.SourceUrl}'>{InlineIcons.GitHubIcon()} Edit on GitHub</a>");
        SB.AppendLine();

        string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        string breadcrumbUrl1 = "/cookbook/5.0/";

        string breadcrumbName2 = Recipe.Category;
        string breadcrumbUrl2 = Recipe.AnchorUrl.Split("#")[0];

        string breadcrumbName3 = Recipe.Name;
        string breadcrumbUrl3 = Recipe.RecipeUrl;

        string[] fm =
        {
            $"BreadcrumbNames: [\"{breadcrumbName1}\", \"{breadcrumbName2}\", \"{breadcrumbName3}\"]",
            $"BreadcrumbUrls: [\"{breadcrumbUrl1}\", \"{breadcrumbUrl2}\", \"{breadcrumbUrl3}\"]",
        };

        string recipeBaseName = Path.GetFileName(Recipe.RecipeUrl);
        string categoryBaseName = Path.GetFileName(Path.GetDirectoryName(Recipe.RecipeUrl)!);

        Save(outputFolder,
            title: Recipe.Name + " - ScottPlot 5.0 Cookbook",
            description: Recipe.Description,
            filename: $"{categoryBaseName}.{recipeBaseName}.md",
            url: Recipe.RecipeUrl,
            fm);
    }
}
