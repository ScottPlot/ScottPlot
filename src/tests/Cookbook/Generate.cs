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

        [OneTimeSetUp]
        public void EmptyOutputFolder()
        {
            if (!System.IO.Directory.Exists(OutputFolder))
                System.IO.Directory.CreateDirectory(OutputFolder);

            foreach (var fname in System.IO.Directory.GetFiles(OutputFolder))
                System.IO.File.Delete(fname);
        }

        [Test]
        public void Test_Cookbook_GenerateImages()
        {
            var chef = new ScottPlot.Cookbook.Chef();
            chef.CreateCookbookImages(OutputFolder);
        }

        [Test]
        public void Test_Cookbook_GenerateSource()
        {
            var chef = new ScottPlot.Cookbook.Chef();
            chef.CreateCookbookSource(SourceFolder, OutputFolder);
        }
    }
}
