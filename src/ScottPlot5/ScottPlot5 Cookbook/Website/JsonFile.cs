using System.Text.Json;

namespace ScottPlotCookbook.Website;

internal static class JsonFile
{
    internal static string BuildID = GetBuildID();

    private static string GetBuildID()
    {
        DateTime dt = DateTime.Now;
        return $"{dt.Year - 2000:D2}{dt.Month:D2}{dt.Day:D2}{dt.Hour:D2}{dt.Minute:D2}{dt.Second:D2}";
    }

    public static string Generate()
    {
        SourceDatabase db = new();

        using MemoryStream stream = new();
        JsonWriterOptions options = new() { Indented = true };
        using Utf8JsonWriter writer = new(stream, options);

        writer.WriteStartObject();

        // library and cookbook metadata
        writer.WriteString("version", ScottPlot.Version.VersionString);
        writer.WriteString("dateUtc", DateTime.UtcNow.ToString("s"));
        writer.WriteNumber("recipeCount", db.Recipes.Count);
        writer.WriteString("jsonSizeKb", "JSON_SIZE");

        // chapters
        writer.WriteStartArray("chapters");
        foreach (string chatper in Query.GetChapterNamesInOrder())
        {
            writer.WriteStringValue(chatper);
        }
        writer.WriteEndArray();

        // categories
        writer.WriteStartArray("categories");
        foreach (ICategory category in Query.GetCategories())
        {
            writer.WriteStartObject();
            writer.WriteString("chapter", category.Chapter);
            writer.WriteString("name", category.CategoryName);
            writer.WriteString("description", category.CategoryDescription);
            writer.WriteString("url", db.Recipes.Where(x => x.Category == category.CategoryName).First().CategoryUrl);
            writer.WriteEndObject();
        }
        writer.WriteEndArray();

        // recipes
        writer.WriteStartArray("recipes");
        foreach (RecipeInfo recipe in db.Recipes)
        {
            writer.WriteStartObject();

            // human readable
            writer.WriteString("chapter", recipe.Chapter);
            writer.WriteString("category", recipe.Category);
            writer.WriteString("name", recipe.Name);
            writer.WriteString("description", recipe.Description);
            writer.WriteString("source", recipe.Source.Replace("\r", ""));

            // organization
            writer.WriteString("categoryClassName", recipe.CategoryClassName);
            writer.WriteString("recipeClassName", recipe.RecipeClassName);

            // web links
            writer.WriteString("anchorUrl", recipe.AnchoredCategoryUrl);
            writer.WriteString("categoryUrl", recipe.CategoryUrl);
            writer.WriteString("recipeUrl", recipe.RecipeUrl);
            writer.WriteString("imageUrl", recipe.ImageUrl + "?" + BuildID);
            writer.WriteString("sourceUrl", recipe.Sourceurl);

            writer.WriteEndObject();
        }
        writer.WriteEndArray();

        writer.WriteEndObject();

        writer.Flush();
        string json = Encoding.UTF8.GetString(stream.ToArray());
        return json.Replace("\"JSON_SIZE\"", (json.Length / 1000).ToString());
    }
}
