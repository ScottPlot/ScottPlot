namespace ScottPlotCookbook;

public struct WebRecipe
{
    // TODO: class names may be useful for searching source code

    public string Chapter { get; set; }
    public string Category { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Source { get; set; }

    public readonly string CookbookUrl = "/cookbook/5.0";
    public readonly string CategoryUrl => $"{CookbookUrl}/{UrlSafe(Category)}";
    public readonly string RecipeUrl => $"{CookbookUrl}/{UrlSafe(Category)}/{UrlSafe(Name)}";
    public readonly string ImageUrl => $"{CookbookUrl}/images/{Category}-{UrlSafe(Name)}.png";

    public WebRecipe(string chapter, string category, string name, string description, string source)
    {
        Chapter = chapter;
        Category = category;
        Name = name;
        Description = description;
        Source = source;
    }

    public override string ToString()
    {
        return $"[{Chapter}] [{Category}] {Name}";
    }

    /// <summary>
    /// convert to lowercase characters, numbers, and dashes
    /// </summary>
    private static string UrlSafe(string text)
    {
        StringBuilder sb = new();

        string charsToReplaceWithDash = " _-+:";

        foreach (char c in text.ToLower().ToCharArray())
        {
            if (charsToReplaceWithDash.Contains(c))
            {
                sb.Append('-');
            }
            else if (char.IsLetterOrDigit(c))
            {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}
