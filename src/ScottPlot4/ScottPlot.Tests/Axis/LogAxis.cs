using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Axis
{
    internal class LogAxis
    {
        [Test]
        public void Test_LogAxis_Y_A()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 10, 2_000, 50_000, 1_000_000, 1_500_000 };

            var plt = new ScottPlot.Plot(600, 400);

            // Plot the log of the Ys
            double[] logYs = ys.Select(y => Math.Log10(y)).ToArray();
            var scatter = plt.AddScatter(xs, logYs);

            // label each point with a red line
            plt.Palette = ScottPlot.Palette.ColorblindFriendly;
            for (int i = 0; i < scatter.PointCount; i++)
            {
                double x = scatter.Xs[i];
                double y = scatter.Ys[i];
                double actualY = Math.Pow(10, y);
                var color = plt.Palette.GetColor(i);
                plt.AddHorizontalLine(y, color, 1, ScottPlot.LineStyle.Dot);
                plt.AddVerticalLine(x, color, 1, ScottPlot.LineStyle.Dot);
                plt.AddPoint(x, y, color, 10, ScottPlot.MarkerShape.openCircle);
                var txt = plt.AddText(actualY.ToString("N0"), x, y, 12, System.Drawing.Color.Black);
            }

            // Use a custom tick formatter to label tick marks as the antilog of their position
            Func<double, string> tickLabeler = (y) => Math.Pow(10, y).ToString("N0");
            plt.YAxis.TickLabelFormat(tickLabeler);

            // Use log-spaced tick marks and grid lines to make it more convincing
            plt.YAxis.MajorGrid(enable: true);
            plt.YAxis.MinorGrid(enable: true);
            plt.YAxis.MinorLogScale(true);

            // Set the axis limits manually to ensure edges terminate at a nice locations in log space
            plt.SetAxisLimits(0, 6, 0, Math.Log10(10_000_000));

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_LogAxis_Y_B()
        {
            double[] xs = { 1, 2, 3, 4, 5 };
            double[] ys = { 10, 2_000, 50_000, 1_000_000, 1_500_000 };

            var plt = new ScottPlot.Plot(600, 600);

            // Plot the log of the Ys
            double[] logYs = ys.Select(y => Math.Log10(y)).ToArray();
            var scatter = plt.AddScatter(xs, logYs);

            // label each point with a red line
            plt.Palette = ScottPlot.Palette.ColorblindFriendly;
            for (int i = 0; i < scatter.PointCount; i++)
            {
                double x = scatter.Xs[i];
                double y = scatter.Ys[i];
                double actualY = Math.Pow(10, y);
                var color = plt.Palette.GetColor(i);
                plt.AddHorizontalLine(y, color, 1, ScottPlot.LineStyle.Dot);
                plt.AddVerticalLine(x, color, 1, ScottPlot.LineStyle.Dot);
                plt.AddPoint(x, y, color, 10, ScottPlot.MarkerShape.openCircle);
                var txt = plt.AddText(actualY.ToString("N0"), x, y, 12, System.Drawing.Color.Black);
            }

            // Use a custom tick formatter to label tick marks as the antilog of their position
            Func<double, string> tickLabeler = (y) => Math.Pow(10, y).ToString("N0");
            plt.YAxis.TickLabelFormat(tickLabeler);

            // Use log-spaced tick marks and grid lines to make it more convincing
            plt.YAxis.MajorGrid(enable: true);
            plt.YAxis.MinorGrid(enable: true);
            plt.YAxis.MinorLogScale(true);

            // Set the axis limits manually to ensure edges terminate at a nice locations in log space
            plt.SetAxisLimits(0, 6, 0, Math.Log10(10_000_000));

            TestTools.SaveFig(plt);

            // Show the actual Y values used for tick marks
            foreach (ScottPlot.Ticks.Tick tick in plt.YAxis.GetTicks())
                Console.WriteLine(tick);
        }

        [Test]
        public void Test_Log_Unset()
        {
            var plt = new ScottPlot.Plot();
            var bmp1 = plt.Render();
            double[] ticks1 = plt.YAxis.GetTicks().Select(x => x.Position).ToArray();

            plt.YAxis.MinorLogScale(true);
            var bmp2 = plt.Render();
            double[] ticks2 = plt.YAxis.GetTicks().Select(x => x.Position).ToArray();

            plt.YAxis.MinorLogScale(false);
            var bmp3 = plt.Render();
            double[] ticks3 = plt.YAxis.GetTicks().Select(x => x.Position).ToArray();

            Assert.AreNotEqual(ticks1.Length, ticks2.Length);
            Assert.AreEqual(ticks1.Length, ticks3.Length);

            for (int i = 0; i < ticks1.Length; i++)
                Assert.AreEqual(ticks1[i], ticks3[i]);
        }

        [Test]
        public void Test_Log_ManualTickCount()
        {
            var plt = new ScottPlot.Plot();

            double[] ys = ScottPlot.DataGen.Range(100, 10_000, 100, true);
            double[] xs = ScottPlot.DataGen.Consecutive(ys.Length);
            double[] logYs = ys.Select(y => Math.Log10(y)).ToArray();

            var scatter = plt.AddScatter(xs, logYs);

            static string logTickLabels(double y) => Math.Pow(10, y).ToString("N0");
            plt.YAxis.TickLabelFormat(logTickLabels);

            plt.YAxis.MinorLogScale(true, minorTickCount: 20);
            plt.YAxis.MinorGrid(true);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_LogDistributedPoints_Inclusive()
        {
            double[] xs = ScottPlot.Ticks.TickCollection.GetLogDistributedPoints(10, 123, 456, true);

            double[] expected =
            {
                123,
                223.24298855610573,
                281.88137782164756,
                323.48597711221146,
                355.75701144389427,
                382.12436637775335,
                404.4176473247475,
                423.7289656683172,
                440.7627556432952,
                456,
            };

            for (int i = 0; i < xs.Length; i++)
                Assert.AreEqual(expected[i], xs[i]);
        }

        [Test]
        public void Test_LogDistributedPoints_Exclusive()
        {
            double[] xs = ScottPlot.Ticks.TickCollection.GetLogDistributedPoints(10, 123, 456, false);

            double[] expected =
            {
                223.24298855610573,
                281.88137782164756,
                323.48597711221146,
                355.75701144389427,
                382.12436637775335,
                404.4176473247475,
                423.7289656683172,
                440.7627556432952,
            };

            for (int i = 0; i < xs.Length; i++)
                Assert.AreEqual(expected[i], xs[i]);
        }
    }
}
