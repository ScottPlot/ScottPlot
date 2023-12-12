namespace ScottPlotCookbook.MarkdownPages;

internal class MarkdownFrontPage : MarkdownPage
{
    public void Generate()
    {
        SB.AppendLine($"# ScottPlot 5.0 Cookbook");
        SB.AppendLine();

        AddVersionInformation();

        foreach (ChapterInfo chapter in Query.GetChapters())
        {
            if (!chapter.Pages.Any())
                continue;
            SB.AppendLine($"<div class='fs-2 mt-4'>{chapter.Name}</div>");
            chapter.Pages.ForEach(x => AddPage(x));
            SB.AppendLine($"<hr class='my-5' />");
        }

        string breadcrumbName1 = "ScottPlot 5.0 Cookbook";
        string breadcrumbUrl1 = "/cookbook/5.0/";

        string[] fm =
        {
            $"BreadcrumbNames: [\"{breadcrumbName1}\"]",
            $"BreadcrumbUrls: [\"{breadcrumbUrl1}\"]",
        };

        Save(Cookbook.OutputFolder,
            title: "ScottPlot 5.0 Cookbook",
            description: "Example plots shown next to the code used to create them",
            filename: "cookbook-5.0.md",
            url: "/cookbook/5.0/",
            frontmatter: fm);
    }

    private void AddPage(PageInfo page)
    {
        SB.AppendLine($"<h2 class=''><a href='{page.Url}' class='text-dark'>{page.Name}</a></h2>");
        SB.AppendLine($"<div>{page.Description}</div>");
        page.RecipeInfos.ForEach(x => AddRecipeImage(x, page));
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
