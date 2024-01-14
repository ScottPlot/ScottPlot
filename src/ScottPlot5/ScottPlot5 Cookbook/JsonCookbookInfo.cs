using ScottPlotCookbook.Recipes;
using System.Text.Json;

namespace ScottPlotCookbook;

/// <summary>
/// This object uses reflection to get recipes and pairs them 
/// with metadata and source code from a JSON file
/// </summary>
public class JsonCookbookInfo
{
    public record struct JsonCategoryInfo(
        string Chapter,
        string Name,
        string Description,
        string Url);

    public record struct JsonRecipeInfo(
        string Chapter,
        string Category,
        string Name,
        string Description,
        string Source,
        string SourceUrl,
        string AnchorUrl,
        string RecipeUrl,
        string ImageUrl);

    public readonly string Version;
    public readonly string[] Chapters;
    public readonly JsonCategoryInfo[] Categories;
    public readonly JsonRecipeInfo[] Recipes;

    public JsonCookbookInfo(string json)
    {
        using JsonDocument document = JsonDocument.Parse(json);
        Version = document.RootElement.GetProperty("version").GetString()!;
        Chapters = document.RootElement.GetProperty("chapters").EnumerateArray().Select(x => x.GetString())!.ToArray()!;
        Categories = document.RootElement.GetProperty("categories").EnumerateArray().Select(x => GetCategoryInfo(x))!.ToArray()!;
        Recipes = document.RootElement.GetProperty("recipes").EnumerateArray().Select(x => GetRecipeInfo(x))!.ToArray()!;
    }

    public static JsonCookbookInfo FromJsonFile(string jsonFilePath)
    {
        string json = File.ReadAllText(jsonFilePath);
        return new JsonCookbookInfo(json);
    }

    private static JsonCategoryInfo GetCategoryInfo(JsonElement el)
    {
        return new JsonCategoryInfo()
        {
            Chapter = el.GetProperty("chapter").ToString(),
            Name = el.GetProperty("name").ToString(),
            Description = el.GetProperty("description").ToString(),
            Url = el.GetProperty("url").ToString(),
        };
    }

    private static JsonRecipeInfo GetRecipeInfo(JsonElement el)
    {
        return new JsonRecipeInfo()
        {
            Chapter = el.GetProperty("chapter").ToString(),
            Category = el.GetProperty("category").ToString(),
            Name = el.GetProperty("name").ToString(),
            Description = el.GetProperty("description").ToString(),
            Source = el.GetProperty("source").ToString(),
            SourceUrl = el.GetProperty("sourceUrl").ToString(),
            AnchorUrl = el.GetProperty("anchorUrl").ToString(),
            RecipeUrl = el.GetProperty("recipeUrl").ToString(),
            ImageUrl = el.GetProperty("imageUrl").ToString(),
        };
    }
}
