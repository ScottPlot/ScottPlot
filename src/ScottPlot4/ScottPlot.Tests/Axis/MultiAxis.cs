using FluentAssertions;
using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.Axis
{
    internal class MultiAxis
    {
        [Test]
        public void Test_RemoveAxis_Works()
        {
            var plt = new ScottPlot.Plot(600, 400);
            plt.AddSignal(ScottPlot.DataGen.Sin(51));
            plt.AddSignal(ScottPlot.DataGen.Cos(51));

            var extraAxis = plt.AddAxis(ScottPlot.Renderable.Edge.Left, axisIndex: 2);
            TestTools.SaveFig(plt, "a");

            plt.RemoveAxis(extraAxis);
            TestTools.SaveFig(plt, "b");
        }

        [Test]
        public void Test_MultiAxis_LeftCornerNotation()
        {
            ScottPlot.Plot plt = new(600, 400);

            List<ScottPlot.Renderable.Axis> axes = new();
            axes.Add(plt.YAxis);
            axes.Add(plt.AddAxis(ScottPlot.Renderable.Edge.Left, 2));
            axes.Add(plt.AddAxis(ScottPlot.Renderable.Edge.Left, 3));

            for (int i = 0; i < axes.Count; i++)
            {
                // add sample data for this axis
                double[] data = ScottPlot.DataGen.Sin(
                    pointCount: 51,
                    phase: .15 * i / axes.Count,
                    mult: Math.Pow(10, i + 3));

                var sig = plt.AddSignal(data);
                sig.YAxisIndex = axes[i].AxisIndex;

                // enable conner notation
                axes[i].TickLabelNotation(multiplier: true);

                // style this axis
                axes[i].Color(plt.Palette.GetColor(i));
                axes[i].Label($"Axis #{i + 1}");
            }

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_MultiAxis_RightCornerNotation()
        {
            ScottPlot.Plot plt = new(600, 400);
            plt.YAxis2.Ticks(true);

            List<ScottPlot.Renderable.Axis> axes = new();
            axes.Add(plt.YAxis2);
            axes.Add(plt.AddAxis(ScottPlot.Renderable.Edge.Right, 2));
            axes.Add(plt.AddAxis(ScottPlot.Renderable.Edge.Right, 3));

            for (int i = 0; i < axes.Count; i++)
            {
                // add sample data for this axis
                double[] data = ScottPlot.DataGen.Sin(
                    pointCount: 51,
                    phase: .15 * i / axes.Count,
                    mult: Math.Pow(10, i + 3));

                var sig = plt.AddSignal(data);
                sig.YAxisIndex = axes[i].AxisIndex;

                // enable conner notation
                axes[i].TickLabelNotation(multiplier: true);

                // style this axis
                axes[i].Color(plt.Palette.GetColor(i));
                axes[i].Label($"Axis #{i + 1}");
            }

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_MultiAxis_BottomCornerNotation()
        {
            ScottPlot.Plot plt = new(600, 400);

            List<ScottPlot.Renderable.Axis> axes = new();
            axes.Add(plt.XAxis);
            axes.Add(plt.AddAxis(ScottPlot.Renderable.Edge.Bottom, 2));
            axes.Add(plt.AddAxis(ScottPlot.Renderable.Edge.Bottom, 3));

            for (int i = 0; i < axes.Count; i++)
            {
                // add sample data for this axis
                double[] xs = ScottPlot.DataGen.Sin(
                    pointCount: 51,
                    phase: .15 * i / axes.Count,
                    mult: Math.Pow(10, i + 3));

                double[] ys = ScottPlot.DataGen.Consecutive(xs.Length);

                var scatter = plt.AddScatter(xs, ys);
                scatter.XAxisIndex = axes[i].AxisIndex;

                // enable conner notation
                axes[i].TickLabelNotation(multiplier: true);

                // style this axis
                axes[i].Color(plt.Palette.GetColor(i));
                axes[i].Label($"Axis #{i + 1}");
            }

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_MultiAxis_TopCornerNotation()
        {
            ScottPlot.Plot plt = new(600, 400);
            plt.XAxis2.Ticks(true);

            List<ScottPlot.Renderable.Axis> axes = new();
            axes.Add(plt.XAxis2);
            axes.Add(plt.AddAxis(ScottPlot.Renderable.Edge.Top, 2));
            axes.Add(plt.AddAxis(ScottPlot.Renderable.Edge.Top, 3));

            for (int i = 0; i < axes.Count; i++)
            {
                // add sample data for this axis
                double[] xs = ScottPlot.DataGen.Sin(
                    pointCount: 51,
                    phase: .15 * i / axes.Count,
                    mult: Math.Pow(10, i + 3));

                double[] ys = ScottPlot.DataGen.Consecutive(xs.Length);

                var scatter = plt.AddScatter(xs, ys);
                scatter.XAxisIndex = axes[i].AxisIndex;

                // enable conner notation
                axes[i].TickLabelNotation(multiplier: true);

                // style this axis
                axes[i].Color(plt.Palette.GetColor(i));
                axes[i].Label($"Axis #{i + 1}");
            }

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_MultiAxis_MatchLimits()
        {
            ScottPlot.Plot plt1 = new();
            plt1.SetAxisLimits(-1, 1, -2, 2);
            plt1.SetAxisLimits(-11, 11, -22, 22, xAxisIndex: 1, yAxisIndex: 1);

            ScottPlot.Plot plt2 = new();
            plt2.MatchAxis(plt1);
            plt2.MatchAxis(plt1, xAxisIndex: 1, yAxisIndex: 1);

            plt1.GetAxisLimits().Should().Be(plt2.GetAxisLimits());
            plt1.GetAxisLimits(1, 1).Should().Be(plt2.GetAxisLimits(1, 1));
        }
    }
}
