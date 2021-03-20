using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlotTests.PlotTypes
{
    public class Bar
    {
        [Test]
        public void Test_Bar_Series()
        {
            double[] xs = { 1, 2, 3, 4, 5, 6, 7 };
            double[] ys = { 10, 15, 12, 6, 8, 4, 12 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddBar(xs, ys);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_PositiveAndNegative()
        {
            var plt = new ScottPlot.Plot(600, 400);

            string[] labels = { "one", "two", "three", "four", "five" };
            int barCount = labels.Length;
            Random rand = new Random(0);
            double[] xs = ScottPlot.DataGen.Consecutive(barCount);
            double[] ys = ScottPlot.DataGen.RandomNormal(rand, barCount, 0, 10);
            double[] yError = ScottPlot.DataGen.RandomNormal(rand, barCount, 5, 2);

            plt.AddBar(xs, ys, yError);

            plt.XTicks(xs, labels);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_PositiveOnly()
        {
            var plt = new ScottPlot.Plot(600, 400);

            string[] labels = { "one", "two", "three", "four", "five" };
            int barCount = labels.Length;
            Random rand = new Random(0);
            double[] xs = ScottPlot.DataGen.Consecutive(barCount);
            double[] ys = ScottPlot.DataGen.RandomNormal(rand, barCount, 50, 10);
            double[] yError = ScottPlot.DataGen.RandomNormal(rand, barCount, 5, 2);

            plt.AddBar(xs, ys, yError);

            plt.XTicks(xs, labels);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_NegativeOnly()
        {
            var plt = new ScottPlot.Plot(600, 400);

            string[] labels = { "one", "two", "three", "four", "five" };
            int barCount = labels.Length;
            Random rand = new Random(0);
            double[] xs = ScottPlot.DataGen.Consecutive(barCount);
            double[] ys = ScottPlot.DataGen.RandomNormal(rand, barCount, -50, 10);
            double[] yError = ScottPlot.DataGen.RandomNormal(rand, barCount, 5, 2);

            plt.AddBar(xs, ys, yError);

            plt.XTicks(xs, labels);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_SeriesWithError()
        {
            double[] xs = { 1, 2, 3, 4, 5, 6, 7 };
            double[] ys = { 10, 15, 12, 6, 8, 4, 12 };
            double[] yErr = { 4, 1, 7, 3, 6, 2, 3 };

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddBar(xs, ys, yErr);
            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_MutiSeries()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys1 = { 10, 15, 12, 6, 8, };
            double[] ys2 = { 6, 8, 8, 9, 5 };

            var plt = new ScottPlot.Plot(400, 300);

            var bar1 = plt.AddBar(xs, ys1);
            bar1.PositionOffset = -.20;
            bar1.BarWidth = .3;
            bar1.ShowValuesAboveBars = true;
            bar1.Orientation = ScottPlot.Orientation.Horizontal;
            bar1.Label = "Series A";

            plt.AddBar(xs, ys2);
            var bar2 = plt.AddBar(xs, ys2);
            bar2.PositionOffset = +.20;
            bar2.BarWidth = 0.3;
            bar2.ShowValuesAboveBars = true;
            bar2.Label = "Series B";
            bar2.Orientation = ScottPlot.Orientation.Horizontal;

            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            plt.XAxis.Grid(false);
            plt.Legend(location: ScottPlot.Alignment.UpperRight);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_SeriesWithErrorHorizontal()
        {
            double[] xs = { 1, 2, 3, 4, 5, 6, 7 };
            double[] ys = { 10, 15, 12, 6, 8, 4, 12 };
            double[] yErr = { 4, 1, 7, 3, 6, 2, 3 };

            var plt = new ScottPlot.Plot(400, 300);
            var bar = plt.AddBar(xs, ys, yErr);
            bar.Orientation = ScottPlot.Orientation.Horizontal;
            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_Stacked()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] valuesA = { 1, 2, 3, 2, 1, };
            double[] valuesB = { 3, 3, 2, 1, 3 };

            // to simulate stacking, shift B up by A
            double[] valuesB2 = new double[valuesB.Length];
            for (int i = 0; i < valuesB.Length; i++)
                valuesB2[i] = valuesA[i] + valuesB[i];

            var plt = new ScottPlot.Plot(400, 300);

            // plot the uppermost bar first
            var bar1 = plt.AddBar(xs, valuesB2);
            bar1.Label = "Series B";

            // plot lower bars last (in front)
            var bar2 = plt.AddBar(xs, valuesA);
            bar2.Label = "Series A";

            plt.Legend(location: ScottPlot.Alignment.UpperRight);
            plt.SetAxisLimits(yMax: 7);
            plt.Title("Stacked Bar Charts");
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_ShowValues()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys1 = { 10, 15, 12, 6, 8, };
            double[] ys2 = { 6, 8, 8, 9, 5 };

            var plt = new ScottPlot.Plot(400, 300);

            var bar1 = plt.AddBar(xs, ys1);
            bar1.PositionOffset = -.20;
            bar1.BarWidth = .3;
            bar1.ShowValuesAboveBars = true;
            bar1.Orientation = ScottPlot.Orientation.Horizontal;
            bar1.Label = "Series A";

            plt.AddBar(xs, ys2);
            var bar2 = plt.AddBar(xs, ys2);
            bar2.PositionOffset = +.20;
            bar2.BarWidth = 0.3;
            bar2.ShowValuesAboveBars = true;
            bar2.Label = "Series B";
            bar2.Orientation = ScottPlot.Orientation.Horizontal;

            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            plt.XAxis.Grid(false);
            plt.Legend(location: ScottPlot.Alignment.UpperRight);
            plt.SetAxisLimits(yMin: 0);
            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Bar_ShowValuesHorizontal()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys1 = { 10, 15, 12, 6, 8, };
            double[] ys2 = { 6, 8, 8, 9, 5 };

            var plt = new ScottPlot.Plot(400, 300);

            var bar1 = plt.AddBar(xs, ys1);
            bar1.PositionOffset = -.20;
            bar1.BarWidth = .3;
            bar1.ShowValuesAboveBars = true;
            bar1.Orientation = ScottPlot.Orientation.Horizontal;
            bar1.Label = "Series A";

            plt.AddBar(xs, ys2);
            var bar2 = plt.AddBar(xs, ys2);
            bar2.PositionOffset = +.20;
            bar2.BarWidth = 0.3;
            bar2.ShowValuesAboveBars = true;
            bar2.Label = "Series B";
            bar2.Orientation = ScottPlot.Orientation.Horizontal;

            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            plt.XAxis.Grid(false);
            plt.Legend(location: ScottPlot.Alignment.UpperRight);
            plt.SetAxisLimits(xMin: 0);
            TestTools.SaveFig(plt);
        }
    }
}
