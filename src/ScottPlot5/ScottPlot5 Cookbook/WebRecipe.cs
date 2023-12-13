namespace ScottPlotCookbook;

/// <summary>
/// This data structure contains information about a single recipe
/// and should be the only source of truth for generating filenames and URLs
/// </summary>
public struct WebRecipe
{
    // TODO: add RecipeClassName

    public string Chapter { get; }
    public string Category { get; }
    public string Name { get; }
    public string Description { get; }
    public string Source { get; }
    public string ClassName { get; }

    public readonly string CategoryFolderName => UrlSafe(Category);
    public readonly string AnchorName => FolderName;
    public readonly string FolderName => UrlSafe(Name);
    public readonly string CookbookUrl = "/cookbook/5.0";
    public readonly string CategoryUrl => $"{CookbookUrl}/{CategoryFolderName}";
    public readonly string AnchoredCategoryUrl => $"{CategoryUrl}#{AnchorName}";
    public readonly string RecipeUrl => $"{CookbookUrl}/{CategoryFolderName}/{FolderName}";
    public readonly string ImageFilename => $"{CategoryFolderName}-{FolderName}.png";
    public readonly string MarkdownFilename => $"{CategoryFolderName}-{FolderName}.md";
    public readonly string ImageUrl => $"{CookbookUrl}/images/{ImageFilename}";

    internal static string GetImageFilename(string category, string recipe)
    {
        return "";
    }

    public WebRecipe(string chapter, string category, string name, string description, string source, string className)
    {
        Chapter = chapter;
        Category = category;
        Name = name;
        Description = description;
        Source = source;
        ClassName = className;
    }

    public override string ToString()
    {
        string chp = string.IsNullOrEmpty(Chapter) ? "unknown chapter" : Chapter;
        string cat = string.IsNullOrEmpty(Category) ? "unknown category" : Category;
        string nm = string.IsNullOrEmpty(Name) ? "unknown name" : Name;
        return $"[{chp}] [{cat}] {nm}";
    }

    /// <summary>
    /// convert to lowercase characters, numbers, and dashes
    /// </summary>
    public static string UrlSafe(string text)
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
