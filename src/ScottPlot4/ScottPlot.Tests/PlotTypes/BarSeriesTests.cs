using NUnit.Framework;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests.PlotTypes
{
    internal class BarSeriesTests
    {
        [Test]
        public void Test_BarSeries_Quickstart()
        {
            // Create a bunch of Bar objects
            Random rand = new(0);
            List<ScottPlot.Plottable.Bar> bars = new();
            for (int i = 0; i < 10; i++)
            {
                int value = rand.Next(25, 100);
                ScottPlot.Plottable.Bar bar = new()
                {
                    // Each bar can be extensively customized
                    Value = value,
                    Position = i,
                    FillColor = ScottPlot.Palette.Category10.GetColor(i),
                    Label = value.ToString(),
                };
                bars.Add(bar);
            };

            // Add the BarSeries to the plot
            ScottPlot.Plot plt = new(600, 400);
            plt.AddBarSeries(bars);
            plt.SetAxisLimitsY(0, 120);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_BarSeries_Horizontal()
        {
            // Create a bunch of Bar objects
            Random rand = new(0);
            List<ScottPlot.Plottable.Bar> bars = new();
            for (int i = 0; i < 10; i++)
            {
                int value = rand.Next(25, 100);
                ScottPlot.Plottable.Bar bar = new()
                {
                    // Each bar can be extensively customized
                    Value = value,
                    Position = i,
                    FillColor = ScottPlot.Palette.Category10.GetColor(i),
                    Label = value.ToString(),
                    IsVertical = false,
                };
                bars.Add(bar);
            };

            // Add the BarSeries to the plot
            ScottPlot.Plot plt = new(600, 400);
            plt.AddBarSeries(bars);
            plt.SetAxisLimitsX(0, 120);

            TestTools.SaveFig(plt);
        }
    }
}
