using System.Text;

namespace ScottPlotCookbook.Generation;

public static class Output
{
    public static string OutputFolder => Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "cookbook-output"));

    public static string GetPagePath(RecipePageDetails pageDetails, bool create = true)
    {
        string folder = Path.Combine(OutputFolder, UrlSafe(pageDetails.PageName));
        if (create)
            Directory.CreateDirectory(folder);
        return folder;
    }

    public static string GetBaseImagePath(RecipePageDetails pageDetails, IRecipe recipe, bool create = true)
    {
        string folder = GetPagePath(pageDetails, create);
        return Path.Combine(folder, UrlSafe(recipe.Name));
    }

    public static string GetBaseImageUrl(RecipePageDetails pageDetails, IRecipe recipe)
    {
        return GetBaseImagePath(pageDetails, recipe).Replace(OutputFolder, "").Replace("\\", "/").TrimStart('/');
    }

    public static string UrlSafe(string text)
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
}
