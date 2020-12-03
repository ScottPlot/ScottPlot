using NUnit.Framework;
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
            var chef = new ScottPlot.Cookbook.Chef();
            chef.CreateCookbookImages(OutputFolder);
            chef.CreateCookbookSource(SourceFolder, OutputFolder);

            // create website
            var gen = new ScottPlot.Cookbook.Site.SiteGenerator(OutputFolder);
        }
    }
}
