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
        string SourceFolder = "../../../../cookbook/ScottPlot.Cookbook";
        string CookbookFolder = "./cookbook";
        string RecipeFolder = "./cookbook/source";
        string ImagesFolder = "./website/images";
        string XmlDocPath = "../../../../../src/ScottPlot/ScottPlot.xml";
        string WebsitePath = "./website";
        string TemplatePath = "../../../../../src/cookbook/ScottPlot.Cookbook/Resources/Template-4.1.html";
        string TemplateHtml;
        XmlDoc XD;
        MethodInfo[] PlotMethods;

        [OneTimeSetUp]
        public void Setup()
        {
            SourceFolder = Path.GetFullPath(SourceFolder);
            CookbookFolder = Path.GetFullPath(CookbookFolder);
            RecipeFolder = Path.GetFullPath(RecipeFolder);
            ImagesFolder = Path.GetFullPath(ImagesFolder);
            XmlDocPath = Path.GetFullPath(XmlDocPath);
            TemplatePath = Path.GetFullPath(TemplatePath);
            WebsitePath = Path.GetFullPath(WebsitePath);
            TemplateHtml = File.ReadAllText(TemplatePath);

            EnsureDirectoryExistsAndIsEmpty(CookbookFolder);
            EnsureDirectoryExistsAndIsEmpty(RecipeFolder);
            EnsureDirectoryExistsAndIsEmpty(WebsitePath);
            EnsureDirectoryExistsAndIsEmpty(ImagesFolder);

            XD = new XmlDoc(XmlDocPath);
            PlotMethods = typeof(ScottPlot.Plot).GetMethods()
                                                .Where(x => x.IsPublic)
                                                .Where(x => !x.GetCustomAttributes<ObsoleteAttribute>().Any())
                                                .Where(x => x.Name != "GetType") // ignore special methods
                                                .OrderBy(x => x.Name.Replace("get_", "").Replace("set_", "").ToLower())
                                                .ToArray();
        }

        private void EnsureDirectoryExistsAndIsEmpty(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            foreach (string filePath in Directory.GetFiles(path, "*.*"))
                File.Delete(filePath);
        }

        [Test]
        public void Test_Cookbook_GenerateCodeAndImages()
        {
            var chef = new Chef();
            chef.CreateCookbookImages(ImagesFolder);
            chef.CreateCookbookSource(SourceFolder, RecipeFolder);

            //CopyResourceFiles();
            //BuildRecipePages();
            //BuildPlotApiPage();
            //BuildPlottableApiPages();
            //BuildIndexPage();
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

        private void BuildPlotApiPage()
        {
            var index = new ScottPlot.Cookbook.Site.IndexPage(CookbookFolder, SourceFolder);
            index.AddPlotApiTableWithoutPlottables(XD, PlotMethods);
            index.AddPlotApiTablePlottables(XD, PlotMethods);
            index.AddPlotApiDetails(XD, PlotMethods);
            index.SaveAs("api-plot.html", "Plot API");
        }

        private void BuildPlottableApiPages()
        {
            foreach (Type plottableType in Locate.GetPlottableTypes())
            {
                var index = new ScottPlot.Cookbook.Site.IndexPage(CookbookFolder, SourceFolder);
                index.AddPlottableDetails(XD, plottableType);

                string typeName = Locate.TypeName(plottableType);
                string typeUrl = Locate.TypeName(plottableType, urlSafe: true);
                index.SaveAs($"api-plottable-{typeUrl}.html", typeName);
            }
        }

        private void BuildIndexPage()
        {
            var index = new ScottPlot.Cookbook.Site.IndexPage(CookbookFolder, SourceFolder);

            // add recipes
            index.AddLinksToRecipes();

            // add API tables
            index.AddPlotApiTableWithoutPlottables(XD, PlotMethods);
            index.AddPlotApiTablePlottables(XD, PlotMethods);
            index.AddPlottableApiTable(XD);

            index.SaveAs("index.html", null);
            Console.WriteLine($"View Cookbook: {System.IO.Path.GetFullPath(CookbookFolder)}/index.html");
        }


        [Test]
        public void Test_Generate_CookbookCategoryPages()
        {
            foreach (var categoryName in Locate.GetCategories())
            {
                var page = new RecipeCategoryPage(categoryName);
                string htmlPath = Path.Combine(WebsitePath, $"cookbook-{Page.Sanitize(categoryName)}.html");
                page.SaveHtml(htmlPath, TemplateHtml);
                Console.WriteLine(htmlPath);
            }
        }

        [Test]
        public void Test_Generate_CookIndexPage()
        {
            var page = new CookbookIndex();
            string htmlPath = Path.Combine(WebsitePath, "index.html");
            page.SaveHtml(htmlPath, TemplateHtml);
            Console.WriteLine(htmlPath);
        }
    }
}
