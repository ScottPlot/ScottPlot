﻿using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class BarSeriesQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.BarSeries();
        public string ID => "barseries_quickstart";
        public string Title => "BarSeries Quickstart";
        public string Description =>
            "A BarSeries plot allows each Bar to be created and customized individually.";

        public void ExecuteRecipe(Plot plt)
        {
            // Create a collection of Bar objects
            Random rand = new(0);
            List<ScottPlot.Plottable.Bar> bars = new();
            for (int i = 0; i < 10; i++)
            {
                int value = rand.Next(25, 100);
                ScottPlot.Plottable.Bar bar = new()
                {
                    // Each bar can be extensively customized
                    Value = value,
                    Position = i,
                    FillColor = ScottPlot.Palette.Category10.GetColor(i),
                    Label = value.ToString(),
                    LineWidth = 2,
                };
                bars.Add(bar);
            };

            // Add the BarSeries to the plot
            plt.AddBarSeries(bars);
            plt.SetAxisLimitsY(0, 120);
        }
    }

    public class BarSeriesHorizontal : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.BarSeries();
        public string ID => "barseries_horizontal";
        public string Title => "BarSeries Horizontal";
        public string Description =>
            "Horizontal orientation can be achieved by customizing the IsVertical property of each Bar.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new(0);
            List<ScottPlot.Plottable.Bar> bars = new();
            for (int i = 0; i < 10; i++)
            {
                int value = rand.Next(25, 100);
                ScottPlot.Plottable.Bar bar = new()
                {
                    Value = value,
                    Position = i,
                    FillColor = ScottPlot.Palette.Category10.GetColor(i),
                    Label = value.ToString(),
                    IsVertical = false, // ENABLE HORIZONTAL ORIENTATION
                };
                bars.Add(bar);
            };

            plt.AddBarSeries(bars);
            plt.SetAxisLimitsX(0, 120);
        }
    }
    public class BarSeriesWithError : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.BarSeries();
        public string ID => "barseries_error";
        public string Title => "BarSeries with Error Bars";
        public string Description =>
            "Combine a BarSeries plot with an ErrorBar plot to achieve this effect.";

        public void ExecuteRecipe(Plot plt)
        {
            // Create and add Bar objects to the plot
            Random rand = new(0);
            List<ScottPlot.Plottable.Bar> bars = new();
            for (int i = 0; i < 10; i++)
            {
                int value = rand.Next(25, 100);
                ScottPlot.Plottable.Bar bar = new()
                {
                    Value = value,
                    Position = i,
                    FillColor = ScottPlot.Palette.Category10.GetColor(i),
                    LineWidth = 2,
                };
                bars.Add(bar);
            };
            plt.AddBarSeries(bars);

            // Add error bars on top of the BarSeries plot
            double[] xs = bars.Select(x => x.Position).ToArray();
            double[] xErrs = bars.Select(x => (double)0).ToArray();
            double[] ys = bars.Select(x => x.Value).ToArray();
            double[] yErrs = bars.Select(x => (double)rand.Next(2, 20)).ToArray();
            var err = plt.AddErrorBars(xs, ys, xErrs, yErrs);
            err.LineWidth = 2;
            err.CapSize = 5;
            err.LineColor = System.Drawing.Color.Black;

            plt.SetAxisLimitsY(0, 120);
        }
    }
}
