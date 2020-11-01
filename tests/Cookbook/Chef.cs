using NUnit.Framework;
using ScottPlot.Demo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ScottPlot.Tests.Cookbook
{
    class Chef
    {
        [Test]
        public void Test_Cookbook_Generator()
        {
            // don't run test on MacOS
            //if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
            //return; // TODO: figure out how to get this working in MacOS

            string sourceFolder = System.IO.Path.GetFullPath("../../../../src/ScottPlot.Demo");
            var reportGen = new ReportGenerator(useDLL: true, sourceFolder: sourceFolder);
            Console.WriteLine("Generating cookbook in: " + reportGen.outputFolder);

            reportGen.ClearFolders();
            foreach (IPlotDemo recipe in reportGen.recipes.Where(r => r.categoryMajor != "DataDiagnostic"))
            {
                Console.WriteLine($"Generating {recipe}...");
                reportGen.CreateImage(recipe);
            }
            reportGen.MakeReports();
        }
    }
}
