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
            CreateFolder(RecipeFolder);
            CreateFolder(WebsitePath);
        }

        private static void CreateFolder(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        [Test]
        public void Test_Cookbook_ImagesAndCode()
        {
            var chef = new Chef();
            chef.CreateCookbookImages(Path.Join(WebsitePath, "images"));
            chef.CreateCookbookSource(SourceFolder, RecipeFolder);
        }

        [Test]
        public void Test_Webpage_CookbookCategoryPages()
        {
            string cookbookFolder = Path.Combine(WebsitePath, "category");
            CreateFolder(cookbookFolder);
            foreach (var categoryName in Locate.GetCategories())
            {
                var page = new CookbookCategoryPage(categoryName);

                string categoryFolder = Path.Combine(cookbookFolder, Page.Sanitize(categoryName));
                CreateFolder(categoryFolder);
                string mdFilePath = Path.Combine(categoryFolder, "index.md");
                page.SaveMarkdown(mdFilePath);
                Console.WriteLine(Path.GetFullPath(mdFilePath));
            }
        }

        [Test]
        public void Test_Webpage_CookIndexPage()
        {
            var page = new CookbookHomePage();
            string mdPath = Path.Combine(WebsitePath, "index.md");
            page.SaveMarkdown(mdPath);
            Console.WriteLine(Path.GetFullPath(mdPath));
        }

        [Test]
        public void Test_Webpage_PlotApi()
        {
            var page = new PlotApiPage();
            string mdFilePath = Path.Combine(WebsitePath, "api/plot/index.md");
            CreateFolder(Path.GetDirectoryName(mdFilePath));
            page.SaveMarkdown(mdFilePath);
            Console.WriteLine();
            Console.WriteLine(Path.GetFullPath(mdFilePath));
        }

        [Test]
        public void Test_Webpage_ForEveryPlottable()
        {
            string plottableFolder = WebsitePath + "/api/plottable";
            CreateFolder(plottableFolder);
            foreach (var plottableType in Locate.GetPlottableTypes())
            {
                var page = new PlottableApiPage(plottableType);
                string thisPlottableFolder = plottableFolder + "/" + Page.Sanitize(plottableType.Name);
                CreateFolder(thisPlottableFolder);
                string mdPath = thisPlottableFolder + "/index.md";
                //CreateFolder(thisPlottableFolder);
                page.SaveMarkdown(mdPath);
                Console.WriteLine(Path.GetFullPath(mdPath));
            }
        }
    }
}
