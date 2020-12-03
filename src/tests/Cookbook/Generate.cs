using NUnit.Framework;
using ScottPlot.Cookbook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Cookbook
{
    class Generate
    {
        const string SourceFolder = "../../../../cookbook/ScottPlot.Cookbook";
        const string RecipeFolder = "./cookbook/source";

        [Test]
        public void Test_Cookbook_Generate()
        {
            CleanRecipeFolder();
            GenerateRecipeImagesAndCodeFiles();
            BuildIndividualCookbookPages();
            BuildIndexPage();
        }

        private void CleanRecipeFolder()
        {
            void EnsureExistingCleanFolder(string folderPath)
            {
                if (!System.IO.Directory.Exists(folderPath))
                    System.IO.Directory.CreateDirectory(folderPath);
                foreach (string filePath in System.IO.Directory.GetFiles(folderPath, "*.*"))
                    System.IO.File.Delete(filePath);
            }

            EnsureExistingCleanFolder(System.IO.Path.GetDirectoryName(RecipeFolder));
            EnsureExistingCleanFolder(RecipeFolder);
        }

        private void GenerateRecipeImagesAndCodeFiles()
        {
            var chef = new Chef();
            chef.CreateCookbookImages(RecipeFolder);
            chef.CreateCookbookSource(SourceFolder, RecipeFolder);
        }

        private void BuildIndividualCookbookPages()
        {
            var pageGenerator = new ScottPlot.Cookbook.Site.SiteGenerator(RecipeFolder);
            var allRecipes = Locate.GetRecipes();

            // create a webpage for every category
            foreach (string category in Locate.GetRecipes().Select(x => x.Category).Distinct())
            {
                IRecipe[] recipes = allRecipes.Where(x => x.Category == category).ToArray();
                pageGenerator.MakeCookbookPage(recipes, category);
            }

            // create a special webpage for all recipes
            pageGenerator.MakeCookbookPage(allRecipes, "All Cookbook Recipes");
        }

        private void BuildIndexPage()
        {
            // start building a HTML index
            var index = new ScottPlot.Cookbook.Site.Index();

            index.AddHTML("<div style='font-size: 300%; font-weight: bold'>ScottPlot Cookbook</div>");
            index.AddHTML($"<div style='font-size: 150%; font-style: italic'>Version {ScottPlot.Plot.Version}</div>");

            // start with categories matching the GUI demo application
            foreach (var stuff in Locate.GetCategorizedRecipes())
            {
                string category = stuff.Key;
                IRecipe[] categoryRecipes = stuff.Value;
                index.AddRecipeGroup(category, categoryRecipes);
            }

            // create a final section for all recipes
            index.AddDiv("<a href='all_cookbook_examples.html'>view all Cookbook recipes on one page</a>");

            // save master index
            string indexFolder = System.IO.Path.GetDirectoryName(RecipeFolder);
            string indexFilePath = System.IO.Path.Join(indexFolder, "index.html");
            index.SaveAs(indexFilePath);
        }
    }
}
