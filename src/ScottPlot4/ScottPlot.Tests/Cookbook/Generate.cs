using NUnit.Framework;
using ScottPlot.Cookbook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ScottPlotTests.Cookbook
{
    class Generate
    {
        string COOKBOOK_PROJECT_FOLDER => Path.GetFullPath("../../../../ScottPlot.Cookbook");
        string OUTPUT_FOLDER => Path.GetFullPath("../../../../ScottPlot.Cookbook/CookbookOutput");
        string JSON_FILE => Path.Join(OUTPUT_FOLDER, "recipes.json");

        [Test]
        public void Test_Generate_Cookbook()
        {
            Console.WriteLine($"Generating cookbook in:\n{OUTPUT_FOLDER}");

            // DELETE OLD COOKBOOK
            if (Directory.Exists(OUTPUT_FOLDER))
                Directory.Delete(OUTPUT_FOLDER, recursive: true);
            Directory.CreateDirectory(OUTPUT_FOLDER);

            // GENERATE IMAGES
            Console.WriteLine($"Generating PNGs...");
            Stopwatch sw = Stopwatch.StartNew();
            IRecipe[] imageRecipes = RecipeImages.Generate(Path.Join(OUTPUT_FOLDER, "images"));
            Console.WriteLine($"Generated {imageRecipes.Length} PNGs in {sw.Elapsed.TotalSeconds:F4} sec");

            // GENERATE JSON
            Console.Write($"Generating JSON...");
            sw.Restart();
            RecipeSource[] sourceRecipes = RecipeJson.Generate(COOKBOOK_PROJECT_FOLDER, JSON_FILE);
            Console.WriteLine($" {sw.Elapsed.TotalSeconds:F4} sec");

            // READ JSON BACK
            Console.Write($"Validating JSON...");
            sw.Restart();
            List<string> readIDs = new();
            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(JSON_FILE));
            string version = document.RootElement.GetProperty("version").GetString();
            string generated = document.RootElement.GetProperty("generated").GetString();
            foreach (JsonElement recipeElement in document.RootElement.GetProperty("recipes").EnumerateArray())
            {
                string id = recipeElement.GetProperty("id").GetString();
                string category = recipeElement.GetProperty("category").GetString();
                string title = recipeElement.GetProperty("title").GetString();
                string description = recipeElement.GetProperty("description").GetString();
                string code = recipeElement.GetProperty("code").GetString();
                readIDs.Add(id);
            }
            Console.WriteLine($" {sw.Elapsed.TotalSeconds:F4} sec");

            // VALIDATE
            Assert.AreEqual(imageRecipes.Length, sourceRecipes.Length);
            Assert.AreEqual(sourceRecipes.Length, readIDs.Count);
        }
    }
}
