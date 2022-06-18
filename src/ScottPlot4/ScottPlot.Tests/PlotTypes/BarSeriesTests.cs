using NUnit.Framework;
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
                ScottPlot.Plottable.Bar bar = new()
                {
                    // Each bar can be extensively customized
                    Value = rand.Next(25, 100),
                    Position = i,
                    FillColor = ScottPlot.Palette.Category10.GetColor(i),
                };
                bars.Add(bar);
            };

            // Add the BarSeries to the plot
            ScottPlot.Plot plt = new(600, 400);
            plt.AddBarSeries(bars);
            plt.SetAxisLimitsY(0, 120);

            TestTools.SaveFig(plt);
        }
    }
}
