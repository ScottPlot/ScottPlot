using NUnit.Framework;
using ScottPlot.Demo;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Cookbook
{
    class Chef
    {
        [Test]
        public void Test_Cookbook_Generate()
        {
            // don't run test on MacOS
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                return; // TODO: figure out how to get this working in MacOS

            var chef = new ScottPlot.Demo.Cookbook.Chef("../../../../src/ScottPlot.Demo/");
            chef.ClearFolders();
            foreach (IPlotDemo recipe in Reflection.GetPlots())
            {
                Console.WriteLine($"creating {recipe.categoryMajor}.{recipe.categoryMinor}.{recipe.categoryClass}...");
                chef.CreateImage(recipe);
            }

            Console.WriteLine($"creating reports...");
            chef.MakeReports();
            Console.WriteLine($"Cookbook generated in: {chef.outputFolder}");
        }
    }
}
