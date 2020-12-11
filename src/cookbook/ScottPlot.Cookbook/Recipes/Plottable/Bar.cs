using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class BarQuickstart : IRecipe
    {
        public string Category => "Plottable: Bar Graph";
        public string ID => "bar_quickstart";
        public string Title => "Bar Graph";
        public string Description =>
            "A simple bar graph can be created from a series of values. " +
            "By default values are palced at X positions 0, 1, 2, etc.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample data
            double[] values = { 26, 20, 23, 7, 16 };

            // add a bar graph to the plot
            plt.AddBar(values);

            // adjust axis limits so there is no padding below the bar graph
            plt.SetAxisLimits(yMin: 0);
        }
    }

    public class BarPositions : IRecipe
    {
        public string Category => "Plottable: Bar Graph";
        public string ID => "bar_positions";
        public string Title => "Bar Graph with Defined Positions";
        public string Description =>
            "Horizontal positions for each bar can be defined manually. " +
            "If you define bar positions, you will probably want to define the bar width as well.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample data
            double[] values = { 26, 20, 23, 7, 16 };
            double[] positions = { 10, 20, 30, 40, 50 };

            // add a bar graph to the plot
            var bar = plt.AddBar(values, positions);

            // customize the width of bars (80% of the inter-position distance looks good)
            bar.BarWidth = (positions[1] - positions[0]) * .8;

            // adjust axis limits so there is no padding below the bar graph
            plt.SetAxisLimits(yMin: 0);
        }
    }

    public class BarError : IRecipe
    {
        public string Category => "Plottable: Bar Graph";
        public string ID => "bar_error";
        public string Title => "Bar Graph with Error Bars";
        public string Description =>
            "Errorbars can be added to any bar graph.";

        public void ExecuteRecipe(Plot plt)
        {
            // add a bar graph to the plot
            double[] values = { 26, 20, 23, 7, 16 };
            var bar = plt.AddBar(values);

            // add errorbars to the bar graph and customize styling as desired
            double[] errors = { 3, 2, 5, 1, 3 };
            bar.YErrors = errors;
            bar.ErrorCapSize = .1;

            // adjust axis limits so there is no padding below the bar graph
            plt.SetAxisLimits(yMin: 0);
        }
    }

    public class BarStacked : IRecipe
    {
        public string Category => "Plottable: Bar Graph";
        public string ID => "bar_stacked";
        public string Title => "Stacked Bar Graphs";
        public string Description => "Bars can be overlapped to give the appearance of stacking.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample data
            double[] valuesA = { 1, 2, 3, 2, 1, 2, 1 };
            double[] valuesB = { 3, 3, 2, 1, 3, 2, 1 };

            // to simulate stacking B on A, shift B up by A
            double[] valuesB2 = new double[valuesB.Length];
            for (int i = 0; i < valuesB.Length; i++)
                valuesB2[i] = valuesA[i] + valuesB[i];

            // plot the bar charts in reverse order (highest first)
            plt.AddBar(valuesB2);
            plt.AddBar(valuesA);

            // adjust axis limits so there is no padding below the bar graph
            plt.SetAxisLimits(yMin: 0);
        }
    }

    public class BarShowValue : IRecipe
    {
        public string Category => "Plottable: Bar Graph";
        public string ID => "bar_values";
        public string Title => "Values Above Bars";
        public string Description => "The value of each bar can be displayed above it.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample data
            double[] values = { 27.3, 23.1, 21.2, 16.1, 6.4, 19.2, 18.7, 17.3, 20.3, 13.1 };

            // add a bar graph to the plot and enable values to be displayed above each bar
            var bar = plt.AddBar(values);
            bar.ShowValuesAboveBars = true;

            // adjust axis limits so there is no padding below the bar graph
            plt.SetAxisLimits(yMin: 0);
        }
    }

    public class BarPattern : IRecipe
    {
        public string Category => "Plottable: Bar Graph";
        public string ID => "bar_pattern";
        public string Title => "Bar Fill Pattern";
        public string Description => "Bar graph fill pattern can be customized.";

        public void ExecuteRecipe(Plot plt)
        {
            var bar1 = plt.AddBar(new double[] { 10, 13, 15 }, new double[] { 1, 5, 9 });
            bar1.HatchStyle = Drawing.HatchStyle.StripedUpwardDiagonal;
            bar1.FillColor = Color.Gray;
            bar1.FillColorHatch = Color.Black;
            bar1.Label = "Series 1";

            var bar2 = plt.AddBar(new double[] { 14, 15, 9 }, new double[] { 2, 6, 10 });
            bar2.HatchStyle = Drawing.HatchStyle.StripedWideDownwardDiagonal;
            bar2.FillColor = Color.DodgerBlue;
            bar2.FillColorHatch = Color.DeepSkyBlue;
            bar2.Label = "Series 2";

            var bar3 = plt.AddBar(new double[] { 13, 6, 14 }, new double[] { 3, 7, 11 });
            bar3.HatchStyle = Drawing.HatchStyle.LargeCheckerBoard;
            bar3.FillColor = Color.SeaGreen;
            bar3.FillColorHatch = Color.DarkSeaGreen;
            bar3.Label = "Series 3";

            // add a legend to display each labeled bar plot
            plt.Legend(location: Alignment.UpperRight);

            // adjust axis limits so there is no padding below the bar graph
            plt.SetAxisLimits(yMin: 0, yMax: 20);
        }
    }

    public class BarHorizontal : IRecipe
    {
        public string Category => "Plottable: Bar Graph";
        public string ID => "bar_horizontal";
        public string Title => "Horizontal Bar Graph";
        public string Description =>
            "Bar graphs are typically displayed as columns, but it's possible to show bars as rows.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample data
            double[] values = { 26, 20, 23, 7, 16 };
            double[] errors = { 3, 2, 5, 1, 3 };
            double[] positions = { 1, 2, 3, 4, 5 };

            // add a bar graph to the plot and customize it to render horizontally
            var bar = plt.AddBar(values, errors, positions);
            bar.VerticalOrientation = false;

            // adjust axis limits so there is no padding to the left of the bar graph
            plt.SetAxisLimits(xMin: 0);
        }
    }

    public class BarGroup : IRecipe
    {
        public string Category => "Plottable: Bar Graph";
        public string ID => "bar_group";
        public string Title => "Grouped Bar Graphs";
        public string Description =>
            "By customizing positions of multiple bar plots you can achieve the appearance of grouped bar graphs. " +
            "The AddBarGroups() method is designed to simplify this process. " +
            "More advanced grouping and bar plot styling is possible using the Population plot type.";

        public void ExecuteRecipe(Plot plt)
        {
            // generate random data to plot
            int groupCount = 5;
            Random rand = new Random(0);
            double[] values1 = DataGen.RandomNormal(rand, groupCount, 20, 5);
            double[] values2 = DataGen.RandomNormal(rand, groupCount, 20, 5);
            double[] values3 = DataGen.RandomNormal(rand, groupCount, 20, 5);
            double[] errors1 = DataGen.RandomNormal(rand, groupCount, 5, 2);
            double[] errors2 = DataGen.RandomNormal(rand, groupCount, 5, 2);
            double[] errors3 = DataGen.RandomNormal(rand, groupCount, 5, 2);

            // group all data together
            string[] groupNames = { "Group A", "Group B", "Group C", "Group D", "Group E" };
            string[] seriesNames = { "Series 1", "Series 2", "Series 3" };
            double[][] valuesBySeries = { values1, values2, values3 };
            double[][] errorsBySeries = { errors1, errors2, errors3 };

            // add the grouped bar plots and show a legend
            plt.AddBarGroups(groupNames, seriesNames, valuesBySeries, errorsBySeries);
            plt.Legend(location: Alignment.UpperRight);

            // adjust axis limits so there is no padding below the bar graph
            plt.SetAxisLimits(yMin: 0);
        }
    }

    public class BarYOffset : IRecipe
    {
        public string Category => "Plottable: Bar Graph";
        public string ID => "bar_yoffset";
        public string Title => "Bars with Y Offsets";
        public string Description =>
            "By default bar graphs start at 0, but this does not have to be the case. " +
            "Y offsets can be defined for each bar. " +
            "When Y offsets are used, values represent the height of the bars (relative to their offsets).";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 23, 17, 19, 24, 22 };
            double[] yOffsets = { -100, -100, -100, -100, -100 };

            var bar = plt.AddBar(values);
            bar.YOffsets = yOffsets;

            // adjust axis limits so there is no padding below the bar graph
            plt.SetAxisLimits(yMin: -100);
        }
    }

    public class BarColorByDirection : IRecipe
    {
        public string Category => "Plottable: Bar Graph";
        public string ID => "bar_yNegColor";
        public string Title => "Negative Bar Colors";
        public string Description => "Bars with negative values can be colored differently than positive ones.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = { 23, -17, 19, -24, 22 };

            var bar = plt.AddBar(values);
            bar.FillColor = Color.Green;
            bar.FillColorNegative = Color.Red;
        }
    }

    public class BarWaterfall : IRecipe
    {
        public string Category => "Plottable: Bar Graph";
        public string ID => "bar_waterfall";
        public string Title => "Waterfall Bar Graph";
        public string Description =>
            "Waterfall bar graphs use bars to represent changes in value from the previous level. " +
            "This style graph can be created by offseting each bar by the sum of all bars preceeding it. " +
            "This effect is similar to financial plots (OHLC and Candlestick) which are described in another section.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = DataGen.RandomNormal(0, 12, 5, 10);
            double[] offsets = Enumerable.Range(0, values.Length).Select(x => values.Take(x).Sum()).ToArray();

            var bar = plt.AddBar(values);
            bar.YOffsets = offsets;
            bar.FillColorNegative = Color.Red;
            bar.FillColor = Color.Green;
        }
    }
}
