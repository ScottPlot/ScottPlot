namespace ScottPlotCookbook.Website;

public static class RecipeHtml
{
    public static string GetMarkdownForCategoryPage(JsonCookbookInfo.JsonRecipeInfo recipe)
    {
        return GetMarkdown(recipe, true, false);
    }

    public static string GetMarkdownForSingleRecipePage(JsonCookbookInfo.JsonRecipeInfo recipe)
    {
        return GetMarkdown(recipe, false, true);
    }

    private static string GetMarkdown(JsonCookbookInfo.JsonRecipeInfo recipe, bool newPageIcon, bool categoryButton)
    {
        StringBuilder sb = new();
        sb.AppendLine();
        sb.AppendLine($"<div class='d-flex align-items-center mt-5'>");
        sb.AppendLine($"<h1 class='me-2 text-dark my-0 border-0'>{recipe.Name}</h1>");
        if (newPageIcon)
        {
            sb.AppendLine($"<a href='{recipe.RecipeUrl}' target='_blank'>");
            sb.AppendLine($"<img src='/images/icons/new-window.svg' style='height: 2rem;' class='new-window-icon'>");
            sb.AppendLine($"</a>");
        }
        sb.AppendLine($"</div>");
        sb.AppendLine();
        sb.AppendLine(recipe.Description);
        sb.AppendLine();
        sb.AppendLine($"[![]({recipe.ImageUrl})]({recipe.ImageUrl})");
        sb.AppendLine();
        sb.AppendLine(
            "{{< recipe-sp5 sourceUrl=\"SOURCE_URL\" imageUrl=\"IMAGE_URL\" >}}SOURCE_CODE{{< /recipe-sp5 >}}"
            .Replace("SOURCE_CODE", CodeToHtml(recipe.Source))
            .Replace("SOURCE_URL", recipe.SourceUrl)
            .Replace("IMAGE_URL", recipe.ImageUrl));
        sb.AppendLine();
        if (categoryButton)
        {
            sb.AppendLine("<div class='my-5 text-center'>This recipe is one of many in the " +
                $"<a href='{recipe.CategoryUrl}'>{recipe.Category}</a> category</div>");
        }
        else
        {
            sb.AppendLine("<hr class='my-5 invisible'>");
        }
        sb.AppendLine();
        return sb.ToString();
    }

    private static string CodeToHtml(string code)
    {
        return code
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");
    }
}
