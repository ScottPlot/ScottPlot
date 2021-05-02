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
            double[] xs = ScottPlot.DataGen.Consecutive(31);
            double[] sin = ScottPlot.DataGen.Sin(31);
            double[] cos = ScottPlot.DataGen.Cos(31);

            var plt = new ScottPlot.Plot(600, 400);
            plt.AddBubblePlot(xs, sin);
            plt.AddBubblePlot(xs, cos);
            plt.Title("Simple Bubble Plot");
            plt.AxisAuto(.2, .25);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_BubblePlot_Advanced()
        {
            double[] xs = ScottPlot.DataGen.Consecutive(31);
            double[] ys = ScottPlot.DataGen.Sin(31);
            var cmap = ScottPlot.Drawing.Colormap.Viridis;

            var plt = new ScottPlot.Plot(600, 400);
            var myBubblePlot = plt.AddBubblePlot();
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

            plt.Title("Advanced Bubble Plot");
            plt.AxisAuto(.2, .25);
            TestTools.SaveFig(plt);
        }
    }
}
