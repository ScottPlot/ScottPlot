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
        // sampe data inspired by https://cmdlinetips.com/2019/02/how-to-make-grouped-boxplots-with-ggplot2/
        // actual data is at http://gapminder.org/data/


        [Test]
        public void Test_PlottablePopulations_Population()
        {
            // This example will display a single population: mean age of every country in europe in 2007
            // This example has 1 series that contains 1 population.

            // for this example we will simulate countries by creating random data
            Random rand = new Random(0);
            var ages = new Population(rand, 44, 78, 2);

            var customPlottable = new PlottablePopulations(ages);

            // plot the multi-series
            var plt = new ScottPlot.Plot(400, 300);
            plt.Add(customPlottable);
            plt.Ticks(displayTicksX: false);

            // additional plot styling
            plt.Title("Life Expectancy in European Countries in 2007");
            plt.YLabel("Age (years)");
            plt.Legend(location: legendLocation.lowerRight);
            plt.Grid(lineStyle: LineStyle.Dot, enableVertical: false);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_PlottablePopulations_Series()
        {
            // This example will display age, grouped by location.
            // This example has 1 series that contains 5 population objects.

            // for this example we will simulate countries by creating random data
            Random rand = new Random(0);

            var ages = new Population[]
            {
                new Population(rand, 54, 53, 5), // africa
                new Population(rand, 35, 72, 3), // americas
                new Population(rand, 48, 72, 4), // asia
                new Population(rand, 44, 78, 2), // europe
                new Population(rand, 14, 81, 1), // oceania
            };
            var customPlottable = new PlottablePopulations(ages, color: System.Drawing.Color.CornflowerBlue);

            // plot the multi-series
            var plt = new ScottPlot.Plot();
            plt.Add(customPlottable);
            plt.XTicks(labels: new string[] { "Africas", "Americas", "Asia", "Europe", "Oceania" });
            plt.Ticks(fontSize: 18);

            // additional plot styling
            plt.Title("Life Expectancy in 2007", fontSize: 26);
            plt.YLabel("Age (years)", fontSize: 18);
            plt.Legend(location: legendLocation.lowerRight);
            plt.Grid(lineStyle: LineStyle.Dot, enableVertical: false);

            TestTools.SaveFig(plt);
        }

        [Test]
        public void Test_PlottablePopulations_MultiSeries()
        {
            // This example will display age, grouped by location, and by year.
            // This example has 3 series (years), each of which has 5 population objects (locations).

            // for this example we will simulate countries by creating random data
            Random rand = new Random(0);

            // start by collecting series data as Population[] arrays
            var ages1957 = new Population[]
            {
                new Population(rand, 54, 42, 4), // africa
                new Population(rand, 35, 56, 6), // americas
                new Population(rand, 48, 47, 5), // asia
                new Population(rand, 44, 66, 2), // europe
                new Population(rand, 14, 70, 1), // oceania
            };

            var ages1987 = new Population[]
            {
                new Population(rand, 54, 52, 8), // africa
                new Population(rand, 35, 70, 3), // americas
                new Population(rand, 48, 66, 3), // asia
                new Population(rand, 44, 75, 2), // europe
                new Population(rand, 14, 75, 1), // oceania
            };

            var ages2007 = new Population[]
            {
                new Population(rand, 54, 53, 5), // africa
                new Population(rand, 35, 72, 3), // americas
                new Population(rand, 48, 72, 4), // asia
                new Population(rand, 44, 78, 2), // europe
                new Population(rand, 14, 81, 1), // oceania
            };

            // now create a PopulationSeries object for each series
            string[] groupLabels = new string[] { "Africa", "Americas", "Asia", "Europe", "Oceania" };
            var series1957 = new PopulationSeries(ages1957, "1957", color: System.Drawing.Color.Red);
            var series1987 = new PopulationSeries(ages1987, "1987", color: System.Drawing.Color.Green);
            var series2007 = new PopulationSeries(ages2007, "2007", color: System.Drawing.Color.Blue);

            // now collect all the series into a MultiSeries
            var multiSeries = new PopulationSeries[] { series1957, series1987, series2007 };
            var plottableMultiSeries = new PopulationMultiSeries(multiSeries);
            var customPlottable = new PlottablePopulations(plottableMultiSeries);

            // plot the multi-series
            var plt = new ScottPlot.Plot();
            plt.Add(customPlottable);
            plt.XTicks(labels: groupLabels);
            plt.Ticks(fontSize: 18);

            // additional plot styling
            plt.Title("Life Expectancy", fontSize: 26);
            plt.YLabel("Age (years)", fontSize: 18);
            plt.Legend(location: legendLocation.lowerRight);
            plt.Grid(lineStyle: LineStyle.Dot, enableVertical: false);

            TestTools.SaveFig(plt);
        }
    }
}
