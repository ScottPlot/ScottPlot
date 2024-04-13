using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        [Test]
        public void Test_SmallPlot_WithAxisLabels()
        {
            var plt = new ScottPlot.Plot(250, 175);
            plt.Style(figureBackground: Color.WhiteSmoke);
            plt.AddSignal(ScottPlot.DataGen.Sin(100));
            plt.Title("Small Plot Title");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_PanCenter()
        {
            ScottPlot.Plot plt = new();

            plt.AddMarker(-1, -1, size: 20);
            plt.AddMarker(-1, 1, size: 20);
            plt.AddMarker(1, 1, size: 20);
            plt.AddMarker(1, -1, size: 20);

            plt.SetAxisLimits(-3, 3, -3, 3);

            TestTools.SaveFig(plt, "before");

            plt.AxisPanCenter(1, 1);

            TestTools.SaveFig(plt, "after");
        }
    }
}
