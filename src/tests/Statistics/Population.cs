using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using ScottPlot;

namespace ScottPlotTests.Statistics
{
    class Population
    {
        // TODO: add tests to check mean, stdev, stderr, etc. of a known population

        [Test]
        public void Test_Population_Curve()
        {
            Random rand = new Random(0);
            var ages = new ScottPlot.Statistics.Population(rand, 44, 78, 2);

            double[] curveXs = DataGen.Range(ages.minus3stDev, ages.plus3stDev, .1);
            double[] curveYs = ages.GetDistribution(curveXs, false);

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddScatterPoints(ages.values, DataGen.Random(rand, ages.values.Length),
                markerSize: 10, markerShape: MarkerShape.openCircle);
            plt.AddScatterLines(curveXs, curveYs);
            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_Population_CurveSideways()
        {
            Random rand = new Random(0);
            var ages = new ScottPlot.Statistics.Population(rand, 44, 78, 2);

            double[] curveXs = DataGen.Range(ages.minus3stDev, ages.plus3stDev, .1);
            double[] curveYs = ages.GetDistribution(curveXs, false);

            var plt = new ScottPlot.Plot(400, 300);
            plt.AddScatterPoints(DataGen.Random(rand, ages.values.Length), ages.values,
                markerSize: 10, markerShape: MarkerShape.openCircle);
            plt.AddScatterLines(curveYs, curveXs);
            plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);

            TestTools.SaveFig(plt);
        }
    }
}
