using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlotTests.Misc
{
    public class MiscPlots
    {
        [Test]
        public void Test_EmptyPlot_DrawsGridAndTicks()
        {
            var plt = new ScottPlot.Plot();
            TestTools.SaveFig(plt);
        }
    }
}
