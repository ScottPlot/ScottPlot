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
            BuildRecipeImagesAndCode();
            CopyResourceFiles();
            BuildRecipePages();
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

        private void BuildRecipeImagesAndCode()
        {
            var chef = new Chef();
            chef.CreateCookbookImages(RecipeFolder);
            chef.CreateCookbookSource(SourceFolder, RecipeFolder);
        }

        private void CopyResourceFiles()
        {
            string resourceFolder = System.IO.Path.Join(SourceFolder, "Resources");
            string[] sourceFileNames = System.IO.Directory.GetFiles(resourceFolder, "*.*")
                                                          .Where(x => !x.EndsWith(".cs"))
                                                          .Where(x => !x.EndsWith(".csproj"))
                                                          .Select(x => System.IO.Path.GetFileName(x))
                                                          .ToArray();

            foreach (string fileName in sourceFileNames)
            {
                string sourcePath = System.IO.Path.Join(resourceFolder, fileName);
                string destPath = System.IO.Path.Join(CookbookFolder, fileName);
                System.IO.File.Copy(sourcePath, destPath);
            }
        }

        private void BuildRecipePages()
        {
            foreach (string category in Locate.GetRecipes().Select(x => x.Category).Distinct())
            {
                var categoryPage = new ScottPlot.Cookbook.Site.RecipesPage(CookbookFolder, SourceFolder);
                categoryPage.AddRecipiesFromCategory(category);
                categoryPage.SaveAs(category, category);
            }

            var allPage = new ScottPlot.Cookbook.Site.RecipesPage(CookbookFolder, SourceFolder);
            allPage.AddAllRecipies();
            allPage.SaveAs("all_recipes.html", "All Recipes");
        }

        private void BuildIndexPage()
        {
            var index = new ScottPlot.Cookbook.Site.IndexPage(CookbookFolder, SourceFolder);
            index.AddLinksToRecipes();
            index.SaveAs("index.html", null);
            Console.WriteLine($"View Cookbook: {System.IO.Path.GetFullPath(CookbookFolder)}/index.html");
        }
    }
}
