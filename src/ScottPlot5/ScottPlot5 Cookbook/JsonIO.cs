using System.Text.Json;

namespace ScottPlotCookbook;
public static class JsonIO
{
    public static string Generate(Dictionary<ICategory, IEnumerable<WebRecipe>> rbc)
    {
        using MemoryStream stream = new();
        JsonWriterOptions options = new() { Indented = true };
        using Utf8JsonWriter writer = new(stream, options);

        writer.WriteStartObject();

        // library and cookbook metadata
        writer.WriteString("version", ScottPlot.Version.VersionString);
        writer.WriteString("dateUtc", DateTime.UtcNow.ToString("s"));
        writer.WriteNumber("recipeCount", rbc.Values.SelectMany(x => x).Count());
        writer.WriteString("jsonSizeKb", "JSON_SIZE");

        // chapters
        writer.WriteStartArray("chapters");
        foreach (string chatper in Query.GetChapterNamesInOrder(rbc))
        {
            writer.WriteStringValue(chatper);
        }
        writer.WriteEndArray();

        // categories
        writer.WriteStartObject("categoryDescriptions");
        foreach (ICategory category in rbc.Keys)
        {
            writer.WriteString(category.CategoryName, category.CategoryDescription);
        }
        writer.WriteEndObject();

        // recipes
        writer.WriteStartArray("recipes");
        foreach (WebRecipe recipe in rbc.Values.SelectMany(x => x))
        {
            writer.WriteStartObject();
            writer.WriteString("chapter", recipe.Chapter);
            writer.WriteString("category", recipe.Category);
            writer.WriteString("name", recipe.Name);
            writer.WriteString("description", recipe.Description);
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
