using System.Text;
using ScottPlotCookbook.Info;

namespace ScottPlotCookbook.HtmlPages;

/// <summary>
/// The front page is /index.html and it lists all recipes.
/// Recipes are grouped by page, and pages are grouped by category.
/// </summary>
internal class HtmlFrontPage : HtmlPageBase
{
    public void Generate()
    {
        foreach (ChapterInfo chapter in Query.GetChapters())
        {
            if (!chapter.Pages.Any())
                continue;
            SB.AppendLine($"<h2>{chapter.Name}</h2>");
            chapter.Pages.ForEach(x => AddPage(x));
            SB.AppendLine($"<hr class='my-5' />");
        }

        Save(Cookbook.OutputFolder, "ScottPlot 5.0 Cookbook");
        Save(Cookbook.OutputFolder, "ScottPlot 5.0 Cookbook", localFile: true);
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
