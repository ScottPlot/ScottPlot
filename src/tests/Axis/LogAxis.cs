using NUnit.Framework;
using System;
using System.Collections.Generic;
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
    }
}
