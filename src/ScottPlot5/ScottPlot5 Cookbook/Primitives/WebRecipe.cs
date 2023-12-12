namespace ScottPlotCookbook;

public struct WebRecipe
{
    public string Chapter { get; set; }
    public string Category { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Source { get; set; }

    public readonly string CookbookUrl = "/cookbook/5.0";
    public string CategoryUrl => $"{CookbookUrl}/{UrlTools.UrlSafe(CategoryUrl)}";
    public string RecipeUrl => $"{CookbookUrl}/{UrlTools.UrlSafe(CategoryUrl)}/{UrlTools.UrlSafe(Name)}";
    public string ImageUrl => $"{CookbookUrl}/images/{CategoryUrl}-{UrlTools.UrlSafe(Name)}.png";

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
}
