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
        public void Test_PopulationGroupedSeries_Render()
        {
            PopulationGroupedSeries groupedSeries = DataGen.GetLifeExpectancyByLocationByYear();

            var plt = new ScottPlot.Plot();
            plt.Add(new ScottPlot.PlottableGroupedSeries(groupedSeries));
            plt.XTicks(labels: groupedSeries.groupLabels);
            plt.Title("Life Expectancy");
            plt.YLabel("Age (years)");
            plt.XLabel("Year");
            plt.Legend(location: legendLocation.lowerRight);
            plt.Grid(lineStyle: LineStyle.Dot, enableVertical: false);

            TestTools.SaveFig(plt);
        }
    }
}
