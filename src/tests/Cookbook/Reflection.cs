using NUnit.Framework;
using ScottPlot.Cookbook;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Cookbook
{
    class Reflection
    {
        [Test]
        public void Test_Cookbook_ListRecipes()
        {
            Recipe[] recipes = ScottPlot.Cookbook.Reflection.GetRecipes();
            Assert.IsNotEmpty(recipes);

            foreach (Recipe recipe in recipes)
                Console.WriteLine(recipe);
        }
    }
}
