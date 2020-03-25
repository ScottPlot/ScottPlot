using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Experimental
{
    class BarExperimental
    {
        public class Bar3 : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Bar 3 experimental";
            public string description { get; }

            public void Render(Plot plt)
            {
                // define values, labels, and style of each bar series
                var barSetMen = new DataSet(new double[] { 15, 22, 45, 17 }, "Men");
                var barSetWomen = new DataSet(new double[] { 37, 21, 29, 13 }, "Women");

                // collect BarSets into groups
                var datasets = new DataSet[] { barSetMen, barSetWomen };
                var groupLabels = new string[] { "always", "regularly", "sometimes", "never" };

                // create the experimental plottable
                var plottableThing = new PlottableBarExperimental(datasets, groupLabels);

                // plot the experimental plottable
                plt.Add(plottableThing);
                plt.Legend(location: legendLocation.upperRight);
                plt.Title("How often do you read reviews?");
                plt.YLabel("Respondents (%)");
                plt.XTicks(groupLabels);
                plt.Grid(enableVertical: false);
            }
        }

        public class Bar3Stacked : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Bar 3 experimental";
            public string description { get; }

            public void Render(Plot plt)
            {
                // define values, labels, and style of each bar series
                var barSetMen = new DataSet(new double[] { 15, 22, 45, 17 }, "Men");
                var barSetWomen = new DataSet(new double[] { 37, 21, 29, 13 }, "Women");

                // collect BarSets into groups
                var datasets = new DataSet[] { barSetMen, barSetWomen };
                var groupLabels = new string[] { "always", "regularly", "sometimes", "never" };

                // create the experimental plottable
                var plottableThing = new PlottableBarExperimental(datasets, groupLabels, stacked: true);

                // plot the experimental plottable
                plt.Add(plottableThing);
                plt.Legend(location: legendLocation.upperRight);
                plt.Title("How often do you read reviews?");
                plt.YLabel("Respondents (%)");
                plt.XTicks(groupLabels);
                plt.Grid(enableVertical: false);
            }
        }
    }
}
