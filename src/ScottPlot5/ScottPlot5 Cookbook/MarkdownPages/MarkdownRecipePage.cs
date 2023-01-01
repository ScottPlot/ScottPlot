using ScottPlotCookbook.Info;

namespace ScottPlotCookbook.MarkdownPages;

internal class MarkdownRecipePage : MarkdownPage
{
    private readonly PageInfo Page;

    internal MarkdownRecipePage(PageInfo page, List<RecipeSource> sources)
    {
        Page = page;

        foreach (RecipeInfo recipe in Page.Recipes)
            recipe.AddSource(sources);

        SB.AppendLine("This page is part of the [ScottPlot 5.0 Cookbook](../)");
    }

    public void Generate()
    {
        AddVersionInformation();

        foreach (RecipeInfo recipe in Page.Recipes)
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

        string outputFolder = Path.Combine(Cookbook.OutputFolder, Page.FolderUrl);
        Save(outputFolder,
            title: Page.Name + " - ScottPlot 5.0 Cookbook",
            description: Page.Description,
            filename: "index.md",
            url: $"/cookbook/5.0/{Page.FolderUrl}/");
    }
}
