using System.Text.Json;

namespace ScottPlotCookbook.Website;

internal static class JsonFile
{
    public static string Generate()
    {
        Dictionary<ICategory, IEnumerable<RecipeInfo>> rbc = Query.GetWebRecipesByCategory();

        using MemoryStream stream = new();
        JsonWriterOptions options = new() { Indented = true };
        using Utf8JsonWriter writer = new(stream, options);

        IEnumerable<RecipeInfo> allRecipes = rbc.Values.SelectMany(x => x);

        writer.WriteStartObject();

        // library and cookbook metadata
        writer.WriteString("version", ScottPlot.Version.VersionString);
        writer.WriteString("dateUtc", DateTime.UtcNow.ToString("s"));
        writer.WriteNumber("recipeCount", allRecipes.Count());
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
        foreach (ICategory category in rbc.Keys)
        {
            writer.WriteStartObject();
            writer.WriteString("chapter", category.Chapter);
            writer.WriteString("name", category.CategoryName);
            writer.WriteString("description", category.CategoryDescription);
            writer.WriteString("url", allRecipes.Where(x => x.Category == category.CategoryName).First().CategoryUrl);
            writer.WriteEndObject();
        }
        writer.WriteEndArray();

        // recipes
        writer.WriteStartArray("recipes");
        foreach (RecipeInfo recipe in rbc.Values.SelectMany(x => x))
        {
            writer.WriteStartObject();
            writer.WriteString("categoryClassName", recipe.CategoryClassName);
            writer.WriteString("recipeClassName", recipe.RecipeClassName);
            writer.WriteString("chapter", recipe.Chapter);
            writer.WriteString("category", recipe.Category);
            writer.WriteString("name", recipe.Name);
            writer.WriteString("description", recipe.Description);
            writer.WriteString("anchorUrl", recipe.AnchoredCategoryUrl);
            writer.WriteString("recipeUrl", recipe.RecipeUrl);
            writer.WriteString("imageUrl", recipe.ImageUrl);
            writer.WriteString("sourceUrl", recipe.Sourceurl);
            writer.WriteString("source", recipe.Source);
            writer.WriteEndObject();
        }
        writer.WriteEndArray();

        writer.WriteEndObject();

        writer.Flush();
        string json = Encoding.UTF8.GetString(stream.ToArray());
        return json.Replace("\"JSON_SIZE\"", (json.Length / 1000).ToString());
    }
}
