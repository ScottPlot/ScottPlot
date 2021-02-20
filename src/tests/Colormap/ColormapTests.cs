using NUnit.Framework;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.ColormapTests
{
    class ColormapTests
    {
        [Test]
        public void Test_Colormap_MultipleRequests()
        {
            var colormaps = ScottPlot.Drawing.Colormap.GetColormaps();
            Assert.IsNotNull(colormaps);
            Assert.IsNotEmpty(colormaps);
        }

        [Test]
        public void Test_Colormap_GetColormapNames()
        {
            string[] names = ScottPlot.Drawing.Colormap.GetColormapNames();
            Assert.IsNotNull(names);
            Assert.IsNotEmpty(names);
            Assert.Contains("Blues", names);
            Assert.Contains("Viridis", names);
        }

        [Test]
        public void Test_Colormap_GetColormapByName()
        {
            ScottPlot.Drawing.Colormap cmap = ScottPlot.Drawing.Colormap.GetColormapByName("Inferno");
            Assert.IsNotNull(cmap);
            Assert.AreEqual("Inferno", cmap.Name);
        }

        [Test]
        public void Test_Colormaps_HaveUniqueNames()
        {
            var fac = new ColormapFactory();
            IEnumerable<string> names = fac.GetAvailableNames();
            Assert.AreEqual(names.Distinct().Count(), names.Count());
        }
    }
}
