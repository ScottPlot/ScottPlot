using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    class BubblePlot
    {
        [Test]
        public void Test_BubblePlot_Simple()
        {
            int pointCount = 31;
            double[] xs = ScottPlot.DataGen.Consecutive(pointCount);
            double[] ys = ScottPlot.DataGen.Sin(pointCount);
            var cmap = ScottPlot.Drawing.Colormap.Viridis;

            var myBubblePlot = new ScottPlot.Plottable.BubblePlot();
            for (int i = 0; i < xs.Length; i++)
            {
                double fraction = (double)i / xs.Length;
                myBubblePlot.Add(
                    x: xs[i],
                    y: ys[i],
                    radius: 10 + i,
                    fillColor: cmap.GetColor(fraction, alpha: .8),
                    edgeColor: System.Drawing.Color.Black,
                    edgeWidth: 2
                );
            }

            var plt = new ScottPlot.Plot(600, 400);
            plt.Add(myBubblePlot);
            plt.Title("Bubble Plot");
            plt.AxisAuto(.2, .25);
            TestTools.SaveFig(plt);
        }
    }
}
