using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Bar
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Simple Bar Graph";
            public string description { get; } = "To plot a single series of data as a bar graph, send the data directly to the PlotBar() function.";

            public void Render(Plot plt)
            {
                var votes = new double[] { 33706, 36813, 12496 };
                var groups = new string[] { "Debian", "SuSE", "Red Hat" };

                // create the bar graph
                plt.PlotBar(votes, groups);
                plt.XTicks(labels: groups);

                // further improve the style of the plot
                plt.Title("Favorite Linux Distribution");
                plt.YLabel("Respondants");
                plt.Grid(enableVertical: false, lineStyle: LineStyle.Dot);
                plt.Axis(y1: 0);
                plt.Ticks(useMultiplierNotation: false);
            }
        }

        public class BarAngled : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Bar Graph with Angled Labels";
            public string description { get; } = "When using a large number of groups, labels can be rotated improve space utilization.";

            public void Render(Plot plt)
            {
                var values = new double[] { 7, 12, 40, 40,
                    100, 125, 172, 550, 560, 600, 2496, 2789 };

                var errors = new double[] { 4, 5, 15, 17,
                    43, 62, 86, 258, 297, 312, 345, 543 };

                var groups = new string[] { "ant", "bird", "mouse",
                    "human", "cat", "dog", "frog", "lion", "elephant",
                    "horse", "shark", "hippo" };

                // create the bar graph
                plt.PlotBar(values, groups, errors, outlineWidth: 1);
                plt.XTicks(labels: groups);
                plt.Ticks(xTickRotation: 45);
                plt.Layout(xScaleHeight: 50); // extra padding below the graph

                // further improve the style of the plot
                plt.Title("Body/Brain Mass Ratio");
                plt.Grid(enableVertical: false, lineStyle: LineStyle.Dot);
                plt.Axis(y1: 0);
                plt.Ticks(useMultiplierNotation: false);
            }
        }

        public class BarMulti : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Multi-Series Bar Graphs";
            public string description { get; } = "Bar graphs with multiple bar series can be displayed, organized into groups. Building a DataSet array and send it to the PlotBar() method.";

            public void Render(Plot plt)
            {
                // define series-level data
                var barSetMen = new DataSet(label: "Men",
                    values: new double[] { 15, 22, 45, 17 },
                    errors: new double[] { 6, 3, 8, 4 });

                var barSetWomen = new DataSet(label: "Women",
                    values: new double[] { 37, 21, 29, 13 },
                    errors: new double[] { 7, 5, 6, 3 });

                // define group-level data
                var dataSets = new DataSet[] { barSetMen, barSetWomen };
                var groupLabels = new string[] {
                    "Always", "Regularly", "Sometimes", "Never" };

                // create the bar graph
                plt.PlotBar(dataSets, groupLabels, outlineWidth: 1);
                plt.XTicks(groupLabels);

                // plot the experimental plottable
                plt.Title("How often do you read reviews?");
                plt.YLabel("Respondents");
                plt.Axis(y1: 0);
                plt.Grid(enableVertical: false, lineStyle: LineStyle.Dot);
                plt.Legend(location: legendLocation.upperRight);
            }
        }

        public class BarStacked : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Stacked Bar Graphs";
            public string description { get; } = "Use the 'stacked' argument to display stacked bar graphs.";

            public void Render(Plot plt)
            {
                // define series-level data
                var barSetMen = new DataSet(label: "Men",
                    values: new double[] { 15, 22, 45, 17 },
                    errors: new double[] { 6, 3, 8, 4 });

                var barSetWomen = new DataSet(label: "Women",
                    values: new double[] { 37, 21, 29, 13 },
                    errors: new double[] { 7, 5, 6, 3 });

                // define group-level data
                var dataSets = new DataSet[] { barSetMen, barSetWomen };
                var groupLabels = new string[] {
                    "Always", "Regularly", "Sometimes", "Never" };

                // create the bar graph
                plt.PlotBar(dataSets, groupLabels, stacked: true, outlineWidth: 1);
                plt.XTicks(groupLabels);

                // plot the experimental plottable
                plt.Title("How often do you read reviews?");
                plt.YLabel("Respondents");
                plt.Axis(y1: 0);
                plt.Grid(enableVertical: false, lineStyle: LineStyle.Dot);
                plt.Legend(location: legendLocation.upperRight);
            }
        }

        public class BarHorizontal : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Horizontal Bar Graph";
            public string description { get; } = "Use the 'horizontal' argumen to display bar graphs horizontally.";

            public void Render(Plot plt)
            {
                // define series-level data
                var barSetMen = new DataSet(label: "Men",
                    values: new double[] { 15, 22, 45, 17 },
                    errors: new double[] { 6, 3, 8, 4 });

                var barSetWomen = new DataSet(label: "Women",
                    values: new double[] { 37, 21, 29, 13 },
                    errors: new double[] { 7, 5, 6, 3 });

                // define group-level data
                var dataSets = new DataSet[] { barSetMen, barSetWomen };
                var groupLabels = new string[] {
                    "Always", "Regularly", "Sometimes", "Never" };

                // create the bar graph
                plt.PlotBar(dataSets, groupLabels, horizontal: true, outlineWidth: 1);
                plt.YTicks(groupLabels);

                // plot the experimental plottable
                plt.Title("How often do you read reviews?");
                plt.YLabel("Respondents");
                plt.Axis(y1: 0);
                plt.Grid(enableHorizontal: false, lineStyle: LineStyle.Dot);
                plt.Legend(location: legendLocation.upperRight);
            }
        }
    }
}
