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
            ScottPlot.Demo.Cookbook.Chef.MakeCookbook("../../../../src/ScottPlot.Demo/");
        }

        [Test]
        public void Test_Cookbook_ReadRecipes()
        {
            ScottPlot.Demo.IPlotDemo[] recipes = ScottPlot.Demo.Reflection.GetPlots();

            foreach(var recipe in recipes)
                Console.WriteLine(recipe.id);
        }
    }
}
