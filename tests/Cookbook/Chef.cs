using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Cookbook
{
    class Chef
    {
        [Test]
        public void Test_Cookbook_MakeCookbook()
        {
            // don't run test on MacOS
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                return; // TODO: figure out how to get this working in MacOS

            ScottPlot.Demo.Cookbook.Chef.MakeCookbook("../../../../src/ScottPlot.Demo/");
        }

        [Test]
        public void Test_Cookbook_ReadRecipes()
        {
            // don't run test on MacOS
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                return; // TODO: figure out how to get this working in MacOS

            ScottPlot.Demo.IPlotDemo[] recipes = ScottPlot.Demo.Reflection.GetPlots();

            foreach(var recipe in recipes)
                Console.WriteLine(recipe.id);
        }
    }
}
