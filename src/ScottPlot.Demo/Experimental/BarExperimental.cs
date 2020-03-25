using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Experimental
{
    class BarExperimental
    {
        public class Bar3Single : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Bar 3 experimental";
            public string description { get; }

            public void Render(Plot plt)
            {
                // define values, labels, and style of each bar series
                var barSetBBR = new DataSet("brainBodyRatio", new double[] { 7, 12, 40, 40, 100, 125, 172, 550, 560, 600, 2496, 2789 });

                // collect BarSets into groups
                var datasets = new DataSet[] { barSetBBR };
                var groupLabels = new string[] { "ant", "bird", "mouse", "human", "cat", "dog", "frog", "lion", "elephant", "horse", "shark", "hippo" };

                // create the experimental plottable
                var plottableThing = new PlottableBarExperimental(datasets, groupLabels);

                // plot the experimental plottable
                plt.Add(plottableThing);
                plt.Title("Body-to-Brain Mass Ratio");
                plt.XTicks(groupLabels);
                plt.Axis(null, null, 0, null);
                plt.Ticks(useMultiplierNotation: false);
                plt.Grid(enableVertical: false);
            }
        }

        public class Bar3 : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Bar 3 experimental";
            public string description { get; }

            public void Render(Plot plt)
            {
                // define values, labels, and style of each bar series
                var barSetMen = new DataSet("Men", new double[] { 15, 22, 45, 17 }, new double[] { 6, 3, 8, 4 });
                var barSetWomen = new DataSet("Women", new double[] { 37, 21, 29, 13 }, new double[] { 7, 5, 6, 3 });

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
                plt.Axis(null, null, 0, null);
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
                var barSetMen = new DataSet("Men", new double[] { 15, 22, 45, 17 });
                var barSetWomen = new DataSet("Women", new double[] { 37, 21, 29, 13 });

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
                plt.Axis(null, null, 0, null);
                plt.Grid(enableVertical: false);
            }
        }
    }
}
