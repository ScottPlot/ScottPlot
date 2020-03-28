using NUnit.Framework;
using ScottPlot;
using ScottPlot.Config;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Groups
{
    class PlotGroupsTest
    {
        [Test]
        public void Test_PopulationSeries_Render()
        {
            PopulationSeries series = DataGen.GetGlobalLifeExpectancyByYear();

            var plt = new ScottPlot.Plot(400, 300);
            plt.Add(new ScottPlot.PlottableSeries(series));
            plt.XTicks(labels: series.groupLabels);
            plt.Title("Life Expectancy");
            plt.YLabel("Age (years)");
            plt.XLabel("Year");
            plt.Grid(lineStyle: LineStyle.Dot);

            TestTools.LaunchFig(plt);
        }
    }
}
