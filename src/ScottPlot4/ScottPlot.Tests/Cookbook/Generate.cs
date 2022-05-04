using NUnit.Framework;
using ScottPlot.Cookbook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScottPlotTests.Cookbook
{
    [TestFixture]
    class Generate
    {
        string COOKBOOK_PROJECT_FOLDER => Path.GetFullPath("../../../../ScottPlot.Cookbook");
        string OUTPUT_FOLDER => Path.GetFullPath("../../../../ScottPlot.Cookbook/CookbookOutput");


        [Test]
        public void Test_Json_IsValid()
        {
            string jsonFilePath = Path.GetFullPath("cookbook-valid-test.json");

            IRecipe[] recipes = Locate.GetRecipes();

            RecipeJson.Generate(COOKBOOK_PROJECT_FOLDER, jsonFilePath);

            Dictionary<string, RecipeSource> readRecipes = RecipeJson.GetRecipes(new FileInfo(jsonFilePath));
            Console.WriteLine($"Read {readRecipes.Count} recipes from JSON");

            Assert.AreEqual(recipes.Length, readRecipes.Count);
        }

        [Ignore("replace with a standalone generator")]
        [Test]
        public void Test_Generate_Cookbook()
        {
            Generator gen = new(COOKBOOK_PROJECT_FOLDER, OUTPUT_FOLDER, regenerate: true);
            gen.MakeCategoryPages();
            gen.MakeIndexPage();
        }

        [Test]
        public void Test_Recipes_RenderInMemory()
        {
            IRecipe[] recipes = Locate.GetRecipes();

            Parallel.ForEach(recipes, recipe =>
            {
                var sw = Stopwatch.StartNew();
                var plt = new ScottPlot.Plot(600, 400);
                recipe.ExecuteRecipe(plt);
                var bmp = plt.GetBitmap();
                Console.WriteLine($"{recipe.ID}, {ScottPlot.Tools.BitmapHash(bmp)}, {sw.Elapsed.TotalMilliseconds}");
            });
        }
    }
}
