using ScottPlotCookbook.Info;

namespace ScottPlotCookbook.HtmlPages;

internal class HtmlRecipePage : HtmlPageBase
{
    private readonly PageInfo Page;

    public HtmlRecipePage(PageInfo page, List<RecipeSource> sources)
    {
        Page = page;

        foreach (RecipeInfo recipe in Page.Recipes)
            recipe.AddSource(sources);
    }

    private static string CodeToHtml(string code)
    {
        return code
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace(" ", "&nbsp;")
            .Replace("\r", "")
            .Replace("\n", "<br>")
            ;
    }

    public void Generate()
    {
        string recipeTemplate = File.ReadAllText("HtmlTemplates/Recipe.html");

        foreach (RecipeInfo recipe in Page.Recipes)
        {
            string recipeHtml = recipeTemplate
                .Replace("{{NAME}}", recipe.Name)
                .Replace("{{DESCRIPTION}}", recipe.Description)
                .Replace("{{CODE}}", CodeToHtml(recipe.SourceCode))
                .Replace("{{IMAGE_URL}}", recipe.ImageFilename)
                ;

            SB.AppendLine(recipeHtml);
        }

        string outputFolder = Path.Combine(Cookbook.OutputFolder, Page.FolderUrl);
        Save(outputFolder, Page.Name, rootUrl: "../#");
        Save(outputFolder, Page.Name, rootUrl: "../#", localFile: true);
    }
}
