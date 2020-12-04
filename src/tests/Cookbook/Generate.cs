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
        const string CookbookFolder = "./cookbook";
        const string RecipeFolder = "./cookbook/source";

        [Test]
        public void Test_Cookbook_Generate()
        {
            EnsureCookbookFoldersExist();
            CleanCookbookFolders();
            GenerateRecipeImagesAndCodeFiles();
            BuildIndividualCookbookPages();
            BuildIndexPage();
        }

        private void EnsureCookbookFoldersExist()
        {
            if (!System.IO.Directory.Exists(CookbookFolder))
                System.IO.Directory.CreateDirectory(CookbookFolder);
            if (!System.IO.Directory.Exists(RecipeFolder))
                System.IO.Directory.CreateDirectory(RecipeFolder);
        }

        private void CleanCookbookFolders()
        {
            foreach (string filePath in System.IO.Directory.GetFiles(CookbookFolder, "*.*"))
                System.IO.File.Delete(filePath);
            foreach (string filePath in System.IO.Directory.GetFiles(RecipeFolder, "*.*"))
                System.IO.File.Delete(filePath);
        }

        private void GenerateRecipeImagesAndCodeFiles()
        {
            var chef = new Chef();
            chef.CreateCookbookImages(RecipeFolder);
            chef.CreateCookbookSource(SourceFolder, RecipeFolder);
        }

        private void BuildIndividualCookbookPages()
        {
            foreach (string category in Locate.GetRecipes().Select(x => x.Category).Distinct())
            {
                var categoryPage = new ScottPlot.Cookbook.Site.RecipesPage(CookbookFolder);
                categoryPage.AddRecipiesFromCategory(category);
                categoryPage.SaveAs(category, category + " Recipes");
            }

            var allPage = new ScottPlot.Cookbook.Site.RecipesPage(CookbookFolder);
            allPage.AddAllRecipies();
            allPage.SaveAs("all_recipes.html", "All Recipes");
        }

        private void BuildIndexPage()
        {
            var index = new ScottPlot.Cookbook.Site.IndexPage(CookbookFolder);
            index.AddLinksToRecipes();
            index.SaveAs("index.html", "ScottPlot Cookbook");
        }
    }
}
