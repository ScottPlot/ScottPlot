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

        [Test]
        public void Test_Json_IsValid()
        {
            string jsonFilePath = Path.GetFullPath("cookbook-valid-test.json");

            IRecipe[] recipes = Locate.GetRecipes();

            string json = RecipeJson.Generate(COOKBOOK_PROJECT_FOLDER);
            File.WriteAllText(jsonFilePath, json);
            Console.WriteLine(jsonFilePath);

            Dictionary<string, RecipeSource> readRecipes = RecipeJson.GetRecipes(jsonFilePath);
            Console.WriteLine($"Read {readRecipes.Count} recipes from JSON");

            Assert.AreEqual(recipes.Length, readRecipes.Count);
        }

        /// <summary>
        /// Executes every cookbook recipe
        /// </summary>
        [Test]
        public void Test_Recipes_GenerateWebsite()
        {
            Generator.GenerateImagesAndJson(COOKBOOK_PROJECT_FOLDER);
            Generator.GenerateWebsite(COOKBOOK_PROJECT_FOLDER);
        }

        [Test]
        public void Test_Html_GenerateTestFile()
        {
            IRecipe[] recipes = Locate.GetRecipes();
            string html = ScottPlot.Cookbook.Experimental.MenuBuilder.GetHtml(recipes);
            string saveAs = Path.GetFullPath("test-menu.html");
            Console.WriteLine(saveAs);
            File.WriteAllText(saveAs, html);
        }
    }
}
