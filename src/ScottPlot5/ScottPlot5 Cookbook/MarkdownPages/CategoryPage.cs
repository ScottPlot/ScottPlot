namespace ScottPlotCookbook.MarkdownPages;

internal class CategoryPage : PageBase
{
    private readonly PageInfo Page;

    internal CategoryPage(PageInfo page, IEnumerable<RecipeSource> sources)
    {
        Page = page;

        foreach (RecipeInfo recipe in Page.RecipeInfos)
            recipe.AddSource(sources);
    }

    public void Generate()
    {
        SB.AppendLine($"# {Page.Name}");
        SB.AppendLine();

        AddVersionInformation();

        foreach (RecipeInfo recipe in Page.RecipeInfos)
        {
            SB.AppendLine();
            SB.AppendLine($"## {recipe.Name}");
            SB.AppendLine();
            SB.AppendLine(recipe.Description);
            SB.AppendLine();
            SB.AppendLine($"[![]({recipe.ImageFilename})]({recipe.ImageFilename})");
            SB.AppendLine();
            SB.AppendLine("```cs");
            SB.AppendLine(recipe.SourceCode);
            SB.AppendLine("```");
            SB.AppendLine();
        }

        string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        string breadcrumbUrl1 = "/cookbook/5.0/";

        string breadcrumbName2 = Page.Name;
        string breadcrumbUrl2 = $"/cookbook/5.0/{Page.FolderName}/";

        string[] fm =
        {
            $"BreadcrumbNames: [\"{breadcrumbName1}\", \"{breadcrumbName2}\"]",
            $"BreadcrumbUrls: [\"{breadcrumbUrl1}\", \"{breadcrumbUrl2}\"]",
        };

        string outputFolder = Path.Combine(Cookbook.OutputFolder, "category");

        Save(outputFolder,
            title: Page.Name + " - ScottPlot 5.0 Cookbook",
            description: Page.Description,
            filename: $"{Page.FolderName}.md",
            url: $"/cookbook/5.0/{Page.FolderName}/",
            fm);
    }
}
