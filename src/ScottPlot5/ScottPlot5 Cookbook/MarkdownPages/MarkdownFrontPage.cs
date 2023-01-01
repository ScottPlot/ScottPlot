using ScottPlotCookbook.Info;

namespace ScottPlotCookbook.MarkdownPages;

internal class MarkdownFrontPage : MarkdownPage
{
    public void Generate()
    {
        AddVersionInformation();

        foreach (ChapterInfo chapter in Query.GetChapters())
        {
            if (!chapter.Pages.Any())
                continue;
            SB.AppendLine($"## {chapter.Name}");
            chapter.Pages.ForEach(x => AddPage(x));
            SB.AppendLine($"<hr class='my-5' />");
        }

        Save(Cookbook.OutputFolder,
            title: "ScottPlot 5.0 Cookbook",
            description: "Example plots shown next to the code used to create them",
            filename: "cookbook-5.0.md",
            url: "/cookbook/5.0/");
    }

    private void AddPage(PageInfo page)
    {
        SB.AppendLine($"<div class='fs-4 mt-4'>{page.Name}</div>");
        SB.AppendLine($"<div>{page.Description}</div>");
        page.Recipes.ForEach(x => AddRecipeImage(x, page));
    }

    private void AddRecipeImage(RecipeInfo recipe, PageInfo page)
    {
        SB.AppendLine("<div class='row my-4'>");

        SB.AppendLine("<div class='col'>");
        SB.AppendLine($"<a href='{page.FolderUrl}/#{recipe.AnchorName}'><img class='img-fluid' src='{page.FolderUrl}/{recipe.ImageFilename}' /></a>");
        SB.AppendLine("</div>");

        SB.AppendLine("<div class='col'>");
        SB.AppendLine($"<div><a href='{page.FolderUrl}/#{recipe.AnchorName}'><b>{recipe.Name}</b></a></div>");
        SB.AppendLine($"<div>{recipe.Description}</div>");
        SB.AppendLine("</div>");

        SB.AppendLine("</div>");
    }
}
