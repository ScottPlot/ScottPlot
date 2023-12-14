using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ScottPlot.Cookbook
{
    public static class RecipeJson
    {
        /// <summary>
        /// Use SOURCE CODE FILE PARSING to locate all recipes in the project and store their information in a JSON file
        /// </summary>
        /// <returns>array of recipes found using source code file parsing</returns>
        public static string Generate(string cookbookFolder, int width = 600, int height = 400)
        {
            RecipeSource[] recipes = SourceParsing.GetRecipeSources(cookbookFolder, width, height);

            using MemoryStream stream = new();
            var options = new JsonWriterOptions() { Indented = true };
            using var writer = new Utf8JsonWriter(stream, options);

            writer.WriteStartObject();
            writer.WriteString("version", ScottPlot.Plot.Version);
            writer.WriteString("generated", DateTime.UtcNow);

            writer.WriteStartArray("recipes");
            foreach (RecipeSource recipe in recipes)
            {
                writer.WriteStartObject();
                writer.WriteString("id", recipe.ID);
                writer.WriteString("category", recipe.Category);
                writer.WriteString("categoryFolder", recipe.CategoryFolder);
                writer.WriteString("title", recipe.Title);
                writer.WriteString("description", recipe.Description);
                writer.WriteString("categoryUrl", recipe.CategoryUrl);
                writer.WriteString("recipeUrl", recipe.Url);
                writer.WriteString("imageUrl", recipe.ImageUrl);
                writer.WriteString("code", recipe.Code.Replace("\r", ""));
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();

            writer.Flush();
            string json = Encoding.UTF8.GetString(stream.ToArray());

            return json;
        }

        /// <summary>
        /// Locate and read the recipes JSON file and return a dictionary with source code information for each recipe.
        /// Returns null if the JSON file could not be located.
        /// </summary>
        public static Dictionary<string, RecipeSource> GetRecipes(string jsonFilePath = null)
        {
            if (string.IsNullOrEmpty(jsonFilePath))
                jsonFilePath = LocateRecipesSourceFile();

            if (jsonFilePath is null)
                return null;

            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(jsonFilePath));

            string version = document.RootElement.GetProperty("version").GetString();
            string generated = document.RootElement.GetProperty("generated").GetString();

            Dictionary<string, RecipeSource> recipes = new();
            foreach (JsonElement recipeElement in document.RootElement.GetProperty("recipes").EnumerateArray())
            {
                string id = recipeElement.GetProperty("id").GetString();
                string category = recipeElement.GetProperty("category").GetString();
                string categoryFolder = recipeElement.GetProperty("categoryFolder").GetString();
                string title = recipeElement.GetProperty("title").GetString();
                string description = recipeElement.GetProperty("description").GetString();
                string code = recipeElement.GetProperty("code").GetString();
                recipes[id] = new RecipeSource(id, category, categoryFolder, title, description, code);
            }

            return recipes;
        }

        /// <summary>
        /// Returns the full path to the recipes json file (or null if not found)
        /// </summary>
        public static string LocateRecipesSourceFile()
        {
            string[] possiblePaths =
            {
                "./",
                "./cookbook",
                "./cookbook/source",
                "../../../../../ScottPlot.Cookbook/CookbookOutput",
            };

            foreach (string path in possiblePaths)
            {
                string jsonFilePath = Path.GetFullPath(Path.Combine(path, "recipes.json"));
                if (File.Exists(jsonFilePath))
                    return jsonFilePath;
            }

            return null;
        }

        public static string NotFoundMessage =>
            "ERROR: Recipe source file (recipes.json) was not found!\n" +
            "Run the tests to generate this file.";
    }
}
