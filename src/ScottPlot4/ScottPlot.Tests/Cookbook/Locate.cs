using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS0618 // ignore obsolete warnings

namespace ScottPlotTests.Cookbook
{
    internal class Locate
    {
        [Test]
        public void Test_LocateRecipes_OriginalSameAsNewMethod()
        {
            var recipes1 = ScottPlot.Cookbook.Locate.LocateRecipes();
            var recipes2 = ScottPlot.Cookbook.Locate.TryLocateRecipes();

            Assert.IsNotNull(recipes1);
            Assert.IsNotNull(recipes2);

            Assert.IsNotEmpty(recipes1);
            Assert.IsNotEmpty(recipes2);

            Assert.AreEqual(recipes1.Length, recipes2.Length);

            string[] recipeIds1 = recipes1.Select(x => x.ID).ToArray();
            string[] recipeIds2 = recipes2.Select(x => x.ID).ToArray();
            Assert.AreEqual(recipeIds1, recipeIds2);
        }
    }
}
