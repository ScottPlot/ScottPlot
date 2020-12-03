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
        const string OutputFolder = "./recipes";

        [Test]
        public void Test_Cookbook_Generate()
        {
            // ensure clean output folder exists
            if (!System.IO.Directory.Exists(OutputFolder))
                System.IO.Directory.CreateDirectory(OutputFolder);
            foreach (var fname in System.IO.Directory.GetFiles(OutputFolder))
                System.IO.File.Delete(fname);

            // create recipe folder
            var chef = new Chef();
            chef.CreateCookbookImages(OutputFolder);
            chef.CreateCookbookSource(SourceFolder, OutputFolder);

            // start the cookbook website maker
            var gen = new ScottPlot.Cookbook.Site.SiteGenerator(OutputFolder);
            var recipes = Reflection.GetRecipes();

            // single page of all examples
            string[] allIDs = recipes.Select(x => x.ID).ToArray();
            gen.MakeCookbookPage(allIDs, "All Cookbook Examples");

            // single page per category
            string[] categories = Reflection.GetRecipes().Select(x => x.Category).Distinct().ToArray();

            foreach (string category in categories)
            {
                var categoryIDs = recipes.Where(x => x.Category == category).Select(x => x.ID).ToArray();
                gen.MakeCookbookPage(categoryIDs, category);
            }
        }
    }
}
