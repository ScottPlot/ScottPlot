namespace ScottPlotCookbook.Website;

public static class RecipeHtml
{
    public static string GetMarkdown(JsonCookbookInfo.JsonRecipeInfo recipe)
    {
        StringBuilder sb = new();
        sb.AppendLine();
        sb.AppendLine($"<h2 style='border-bottom: 0;'><a href='{recipe.RecipeUrl}'>{recipe.Name}</a></h2>");
        sb.AppendLine();
        sb.AppendLine($"""
                <div class="d-flex mb-2">
                <a class="btn btn-sm btn-primary me-1" href="{recipe.RecipeUrl}">Recipe Permalink</a>
                <a class="btn btn-sm btn-success me-1" href="{recipe.CategoryUrl}">Category: {recipe.Category}</a>
                </div>
                """);
        sb.AppendLine();
        sb.AppendLine(recipe.Description);
        sb.AppendLine();
        sb.AppendLine($"[![]({recipe.ImageUrl})]({recipe.ImageUrl})");
        sb.AppendLine();
        sb.AppendLine("{{< recipe-sp5 >}}" + recipe.Source + "{{< /recipe-sp5 >}}");
        sb.AppendLine();
        sb.AppendLine("<hr class='my-5 invisible'>");
        sb.AppendLine();
        return sb.ToString();
    }
}
