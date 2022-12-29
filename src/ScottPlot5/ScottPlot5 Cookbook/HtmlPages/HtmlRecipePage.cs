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

    private static string HtmlSafeCode(string code)
    {
        return code
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
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
                .Replace("{{CODE}}", HtmlSafeCode(recipe.SourceCode))
                .Replace("{{IMAGE_URL}}", recipe.ImageFilename)
                .Replace("{{ANCHOR}}", recipe.AnchorName)
                ;

            SB.AppendLine(recipeHtml);
        }

        string outputFolder = Path.Combine(Cookbook.OutputFolder, Page.FolderUrl);
        Save(outputFolder, Page.Name, rootUrl: "../#");
        Save(outputFolder, Page.Name, rootUrl: "../#", localFile: true);
    }
}
