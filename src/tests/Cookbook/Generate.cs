using NUnit.Framework;
using ScottPlot.Cookbook;
using ScottPlot.Cookbook.Website;
using ScottPlot.Cookbook.Website.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Cookbook
{
    class Generate
    {
        /// <summary>
        /// Source folder containing the original .cs files that will be parsed to get recipe source code
        /// </summary>
        const string SourceFolder = "../../../../cookbook/ScottPlot.Cookbook";

        /// <summary>
        /// The folder that will store isolated code example text files
        /// </summary>
        const string RecipeFolder = "./cookbook/source";

        /// <summary>
        /// The folder to put the website into
        /// </summary>
        const string WebsitePath = "./website";

        [OneTimeSetUp]
        public void Setup()
        {
            CreateEmptyDirectory(RecipeFolder);
            CreateEmptyDirectory(WebsitePath);
        }

        private static void CreateEmptyDirectory(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, recursive: true);
            Directory.CreateDirectory(path);
        }

        [Test]
        public void Test_Cookbook_GenerateCodeAndImages()
        {
            var chef = new Chef();
            chef.CreateCookbookImages(Path.Join(WebsitePath, "images"));
            chef.CreateCookbookSource(SourceFolder, RecipeFolder);
        }

        [Test]
        public void Test_Generate_CookbookCategoryPages()
        {
            foreach (var categoryName in Locate.GetCategories())
            {
                string folderName = $"cookbook-{MarkdownPage.Sanitize(categoryName)}";
                string folderPath = Path.Combine(WebsitePath, folderName);
                CreateEmptyDirectory(folderPath);
                string mdFilePath = Path.Combine(folderPath, "index.md");

                var page = new RecipeCategoryPage(categoryName);
                page.SaveMarkdown(mdFilePath);
                Console.WriteLine(Path.GetFullPath(mdFilePath));
            }
        }

        [Test]
        public void Test_Generate_CookIndexPage()
        {
            var page = new CookbookIndex();
            string mdPath = Path.Combine(WebsitePath, "index.md");
            page.SaveMarkdown(mdPath);
            Console.WriteLine(Path.GetFullPath(mdPath));
        }
    }
}
