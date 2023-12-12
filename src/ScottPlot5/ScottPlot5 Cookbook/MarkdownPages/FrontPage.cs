namespace ScottPlotCookbook.MarkdownPages;

internal class FrontPage : PageBase
{
    readonly List<RecipeInfo> Sources;

    public FrontPage(List<RecipeInfo> sources)
    {
        Sources = sources;
    }

    public void Generate()
    {
        SB.AppendLine($"# ScottPlot 5.0 Cookbook");
        SB.AppendLine();

        AddVersionInformation();

        IEnumerable<Chapter> chapters = Enum.GetValues<Chapter>().ToList();

        foreach (Chapter chapter in chapters)
        {
            ChapterInfo ci = new(chapter, Sources);

            SB.AppendLine($"<div class='fs-2 mt-4'>{ci.Name}</div>");
            ci.Categories.ForEach(x => AddPage(x));
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

    private void AddPage(CategoryInfo page)
    {
        SB.AppendLine($"<h2 class=''><a href='{page.Url}' class='text-dark'>{page.Name}</a></h2>");
        SB.AppendLine($"<div>{page.Description}</div>");
        page.RecipeInfos.ForEach(x => AddRecipeImage(x, page));
    }

    private void AddRecipeImage(RecipeInfo recipe, CategoryInfo page)
    {
        SB.AppendLine("<div class='row my-4'>");

        SB.AppendLine("<div class='col'>");
        SB.AppendLine($"<a href='{page.FolderName}/#{recipe.AnchorName}'><img class='img-fluid' src='{page.FolderName}/{recipe.ImageFilename}' /></a>");
        SB.AppendLine("</div>");

        SB.AppendLine("<div class='col'>");
        SB.AppendLine($"<div><a href='{page.FolderName}/#{recipe.AnchorName}'><b>{recipe.Name}</b></a></div>");
        SB.AppendLine($"<div>{recipe.Description}</div>");
        SB.AppendLine("</div>");

        SB.AppendLine("</div>");
    }
}
