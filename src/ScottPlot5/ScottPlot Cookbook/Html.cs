using System.Text;

namespace ScottPlotCookbook;

internal static class Html
{
    internal static string UrlSafe(string text)
    {
        StringBuilder sb = new();
        string charsToReplaceWithDash = " _-+:";
        foreach (char c in text.ToLower().ToCharArray())
        {
            if (charsToReplaceWithDash.Contains(c))
                sb.Append("-");
            else if (char.IsLetterOrDigit(c))
                sb.Append(c);
        }
        return sb.ToString();
    }

    internal static string GetPageUrl(RecipePageBase page)
    {
        return Html.UrlSafe(page.PageDetails.PageName);
    }

    internal static string GetImageUrl(RecipePageBase page, IRecipe recipe, string extension = ".png")
    {
        string folder = GetPageUrl(page);
        string filename = Html.UrlSafe(recipe.Name) + extension;
        return Path.Combine(folder, filename).Replace("\\", "/");
    }
}
