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

        private ScottPlot.Plottable.BubblePlot CreateTestBubblePlot(bool plotToo = false)
        {
            int bubbleCount = 10;
            double[] xs = ScottPlot.DataGen.Consecutive(bubbleCount);
            double[] ys = ScottPlot.DataGen.Sin(bubbleCount);
            System.Drawing.Color fillColor = System.Drawing.Color.Green;
            System.Drawing.Color edgeColor = System.Drawing.Color.Magenta;
            float edgeWidth = 2;
            float radius = 5;

            ScottPlot.Plottable.BubblePlot bp = new();
            bp.Add(xs, ys, radius, fillColor, edgeWidth, edgeColor);

            if (plotToo)
            {
                ScottPlot.Plot plt = new();
                plt.Add(bp);
                TestTools.SaveFig(plt);
            }

            return bp;
        }

        [Test]
        public void Test_BubblePlot_GetPointNearestX()
        {
            ScottPlot.Plottable.BubblePlot bp = CreateTestBubblePlot();

            (double x, double y, int index) = bp.GetPointNearestX(5.1);
            Assert.AreEqual(5, x);
            Assert.AreEqual(y, -.3, .1);
            Assert.AreEqual(index, 5);
        }

        [Test]
        public void Test_BubblePlot_GetPointNearestY()
        {
            ScottPlot.Plottable.BubblePlot bp = CreateTestBubblePlot();

            (double x, double y, int index) = bp.GetPointNearestY(-0.25);
            Assert.AreEqual(5, x);
            Assert.AreEqual(y, -.3, .1);
            Assert.AreEqual(index, 5);
        }

        [Test]
        public void Test_BubblePlot_GetPointNearest()
        {
            ScottPlot.Plottable.BubblePlot bp = CreateTestBubblePlot();

            (double x, double y, int index) = bp.GetPointNearest(5.1, -0.25);
            Assert.AreEqual(5, x);
            Assert.AreEqual(y, -.3, .1);
            Assert.AreEqual(index, 5);
        }
    }
}
