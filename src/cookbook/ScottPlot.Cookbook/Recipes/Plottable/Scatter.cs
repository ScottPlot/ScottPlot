﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class Quickstart : IRecipe
    {
        public string Category => "Plottable: Scatter Plot";
        public string ID => "scatter_quickstart";
        public string Title => "Scatter Plot Quickstart";
        public string Description =>
            "Scatter plots are best for small numbers of paired X/Y data points. " +
            "For evenly-spaced data points Signal is much faster.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample X/Y data
            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            // add scatter plots
            plt.AddScatter(x, sin);
            plt.AddScatter(x, cos);
        }
    }

    public class CustomizeMarkers : IRecipe
    {
        public string Category => "Plottable: Scatter Plot";
        public string ID => "scatter_markers";
        public string Title => "Custom markers";
        public string Description => "Markers can be customized using optional arguments and public fields.";

        public void ExecuteRecipe(Plot plt)
        {
            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            // add scatter plots and customize markers
            var sp1 = plt.AddScatter(x, sin, markerSize: 15);
            sp1.MarkerShape = MarkerShape.openCircle;

            var sp2 = plt.AddScatter(x, cos, markerSize: 7);
            sp2.MarkerShape = MarkerShape.filledSquare;
        }
    }

    public class AllMarkers : IRecipe
    {
        public string Category => "Plottable: Scatter Plot";
        public string ID => "scatter_markerShape";
        public string Title => "All marker shapes";
        public string Description => "Legend indicates names of all available marker shapes";

        public void ExecuteRecipe(Plot plt)
        {
            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);

            string[] markerShapeNames = Enum.GetNames(typeof(MarkerShape));
            for (int i = 0; i < markerShapeNames.Length; i++)
            {
                Enum.TryParse(markerShapeNames[i], out MarkerShape ms);
                double[] sin = DataGen.Sin(pointCount, 2, -i);
                plt.AddScatter(x, sin, markerSize: 7, markerShape: ms, label: markerShapeNames[i]);
            }

            plt.Grid(enable: false);
            var legend = plt.Legend();
            legend.FontSize = 10;
        }
    }

    public class CustomizeLines : IRecipe
    {
        public string Category => "Plottable: Scatter Plot";
        public string ID => "scatter_lineStyle";
        public string Title => "Custom lines";
        public string Description =>
            "Line color, size, and style can be customized. " +
            "Setting markerSize to 0 prevents markers from being rendered.";

        public void ExecuteRecipe(Plot plt)
        {
            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);
            double[] cos2 = DataGen.Cos(pointCount, mult: -1);

            plt.AddScatter(x, sin, color: Color.Magenta, lineWidth: 0, markerSize: 10);
            plt.AddScatter(x, cos, color: Color.Green, lineWidth: 5, markerSize: 0);
            plt.AddScatter(x, cos2, color: Color.Blue, lineWidth: 3, markerSize: 0, lineStyle: LineStyle.DashDot);

            var legend = plt.Legend();
            legend.FixedLineWidth = false;
        }
    }

    public class RandomXY : IRecipe
    {
        public string Category => "Plottable: Scatter Plot";
        public string ID => "scatter_random";
        public string Title => "Random X/Y Points";
        public string Description =>
            "X data for scatter plots does not have to be evenly spaced, " +
            "making scatter plots are ideal for displaying random data like this.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 51;
            double[] xs1 = DataGen.RandomNormal(rand, pointCount, 1);
            double[] xs2 = DataGen.RandomNormal(rand, pointCount, 3);
            double[] ys1 = DataGen.RandomNormal(rand, pointCount, 5);
            double[] ys2 = DataGen.RandomNormal(rand, pointCount, 7);

            plt.AddScatter(xs1, ys1, markerSize: 0, label: "lines only");
            plt.AddScatter(xs2, ys2, lineWidth: 0, label: "markers only");
            plt.Legend();
        }
    }

    public class ErrorBars : IRecipe
    {
        public string Category => "Plottable: Scatter Plot";
        public string ID => "scatter_errorbar";
        public string Title => "Scatter Plot with Errorbars";
        public string Description =>
            "An array of values can be supplied for error bars " +
            "and redering options can be customized as desired";

        public void ExecuteRecipe(Plot plt)
        {
            int pointCount = 20;
            Random rand = new Random(0);
            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = DataGen.RandomWalk(rand, pointCount);
            double[] xErr = DataGen.RandomNormal(rand, pointCount, .2);
            double[] yErr = DataGen.RandomNormal(rand, pointCount);

            var sp = plt.AddScatter(xs, ys);
            sp.XError = xErr;
            sp.YError = yErr;
            sp.ErrorCapSize = 3;
            sp.ErrorLineWidth = 1;
            sp.LineStyle = LineStyle.Dot;
        }
    }

    public class LinePlot : IRecipe
    {
        public string Category => "Plottable: Scatter Plot";
        public string ID => "scatter_lineplot";
        public string Title => "Lines Only";
        public string Description => "A shortcut method makes it easy " +
            "to create a scatter plot with just lines (no markers)";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(51);
            double[] ys = DataGen.Sin(51);

            plt.AddScatterLines(xs, ys, Color.Red, 3);
        }
    }

    public class PointsPlot : IRecipe
    {
        public string Category => "Plottable: Scatter Plot";
        public string ID => "scatter_pointsplot";
        public string Title => "Markers Only";
        public string Description => "A shortcut method makes it easy to create a scatter plot " +
            "where markers are displayed at every point (without any connecting lines)";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(51);
            double[] ys = DataGen.Sin(51);

            plt.AddScatterPoints(xs, ys, Color.Navy, 10, MarkerShape.filledDiamond);
        }
    }

    public class StepPlot : IRecipe
    {
        public string Category => "Plottable: Scatter Plot";
        public string ID => "scatter_stepplot";
        public string Title => "Step Plot";
        public string Description => "A step plot is a special type of scatter plot where points " +
            "are connected by right angles instead of straight lines.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(51);
            double[] ys = DataGen.Sin(51);

            plt.AddScatterStep(xs, ys);
        }
    }
}
