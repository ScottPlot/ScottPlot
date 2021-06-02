using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlotTests.PlotTypes
{
    public class Fill
    {
        [Test]
        public void Test_Fill_OneCurve()
        {
            double[] xs = ScottPlot.DataGen.Range(0, 10, .1, true);
            double[] ys = ScottPlot.DataGen.Sin(xs);

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddFill(xs, ys);
            plt.AddHorizontalLine(0, color: Color.Black);
            plt.AxisAuto(0);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Fill_OneCurveWithBaseline()
        {
            double[] xs = ScottPlot.DataGen.Range(0, 10, .1, true);
            double[] ys = ScottPlot.DataGen.Sin(xs);

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddFill(xs, ys, baseline: .5);
            plt.AddHorizontalLine(0.5, color: Color.Black);
            plt.AxisAuto(0);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Fill_Between()
        {
            // the sine wave will have ~100 points
            double[] xs1 = ScottPlot.DataGen.Range(1, 10, .1, true);
            double[] ys1 = ScottPlot.DataGen.Sin(xs1);

            // the sine wave will have ~20 points
            double[] xs2 = ScottPlot.DataGen.Range(1, 10, .5, true);
            double[] ys2 = ScottPlot.DataGen.Cos(xs2);

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddFill(xs1, ys1, xs2, ys2);
            plt.AddScatter(xs1, ys1, Color.Black, 2);
            plt.AddScatter(xs2, ys2, Color.Black, 2);
            plt.AxisAuto(0);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Fill_SeveralCurvesWithLegend()
        {
            double[] blueLineXs = { 0, 3, 5.5, 8, 10, 14, 15 };
            double[] blueLineYs = { 10, 13, 10, 13, 11, 15, 15 };

            double[] greenLineXs = { 0, 5, 6.5, 8, 11, 15 };
            double[] greenLineYs = { 6, 2, 3.5, 2, 5, 1 };

            double[] redLineXs = { 0, 5, 8, 15 };
            double[] redLineYs = { 2, 7, 4, 11 };

            var plt = new ScottPlot.Plot(400, 300);

            plt.AddFill(blueLineXs, blueLineYs, color: Color.Blue);
            plt.AddFill(redLineXs, redLineYs, color: Color.Red);
            plt.AddFill(greenLineXs, greenLineYs, color: Color.Green);

            plt.AxisAuto(0, 0);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Fill_AboveBelow()
        {
            Random rand = new Random(0);
            var ys = ScottPlot.DataGen.RandomWalk(rand, 1000, offset: -10);
            var xs = ScottPlot.DataGen.Consecutive(ys.Length, spacing: 0.025);

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddFillAboveAndBelow(xs, ys);
            plt.Legend(location: ScottPlot.Alignment.LowerLeft);
            plt.AxisAuto(0);

            TestTools.SaveFig(plt);
        }
    }
}
