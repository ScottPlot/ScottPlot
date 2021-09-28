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
                string title = recipeElement.GetProperty("title").GetString();
                string description = recipeElement.GetProperty("description").GetString();
                string code = recipeElement.GetProperty("code").GetString();
                recipes[id] = new RecipeSource(id, category, title, description, code);
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
