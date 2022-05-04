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
        public static RecipeSource[] Generate(string cookbookFolder, string saveFilePath, int width = 600, int height = 400)
        {
            RecipeSource[] recipes = SourceParsing.GetRecipeSources(cookbookFolder, width, height);

            using var stream = File.OpenWrite(saveFilePath);
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
                writer.WriteString("code", recipe.Code.Replace("\r", ""));
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();

            return recipes;
        }

        /// <summary>
        /// Return information about all recipes stored in a JSON file
        /// </summary>
        /// <param name="jsonFile">Path to the JSON file. If not provided it will attempt to be found.</param>
        /// <returns>The dictionary if the JSON file exists or was found (otherwise null)</returns>
        public static Dictionary<string, RecipeSource> GetRecipes(FileInfo jsonFile = null)
        {
            if (jsonFile is null)
                jsonFile = Locate();

            if (jsonFile is null)
                return null;

            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(jsonFile.FullName));

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
        /// Search typical folders to find recipes.json
        /// </summary>
        /// <returns>full path to recipes.json or null if not found</returns>
        public static FileInfo Locate()
        {
            string[] possiblePaths =
            {
                // potential paths relative to this EXE
                "",
                "cookbook",
                "cookbook/source",

                // potential paths relative to the test runner
                "../../../../../tests/bin/Debug/net5.0/cookbook/source",
                "../../../../../cookbook/output",
            };

            foreach (string path in possiblePaths)
            {
                FileInfo fi = new(Path.Combine(path, "recipes.json"));
                if (fi.Exists)
                    return fi;
            }

            return null;
        }

        public static string NotFoundMessage =>
            "ERROR: Recipe source file (recipes.json) was not found!\n" +
            "Developers can generate these files by running the tests:\n" +
            "To run tests from Visual Studio, click 'Test' and select 'Run All Tests'.\n" +
            "To run tests from the command line, run 'dotnet test' in the src folder.\n";
    }
}
