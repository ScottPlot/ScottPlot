using NUnit.Framework;
using ScottPlot.Cookbook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Cookbook
{
    class Category
    {
        [Test]
        public void Test_Categories_ListPlotTypes()
        {
            var recipes = Reflection.GetRecipes();
            var plottableRecipes = recipes.Where(x => x is IPlottableRecipe).Select(x => (IPlottableRecipe)x).ToArray();

            string[] plotTypes = plottableRecipes.Select(x => x.PlotType).Distinct().ToArray();
            Assert.IsNotEmpty(plotTypes);

            foreach (string plotType in plotTypes)
                Console.WriteLine(plotType);
        }
    }
}
