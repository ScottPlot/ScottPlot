using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class FillQuickstart : IRecipe
    {
        public string Category => "Plottable: Fill";
        public string ID => "fill_curve";
        public string Title => "Fill Under Curve";
        public string Description =>
            "Fill methods help to create semitransparent polygons to fill the area under a curve. " +
            "This can be used to give the appearance of shading under a scatter plot, even though the " +
            "plottable created here is a polygon with optional edge color and fill color.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample data
            double[] xs = DataGen.Range(0, 10, .1, true);
            double[] ys1 = DataGen.Sin(xs);
            double[] ys2 = DataGen.Cos(xs);

            // add filled polygons
            plt.AddFill(xs, ys1);
            plt.AddFill(xs, ys2, baseline: -.25);

            // tighten the axis limits so we don't see lines on the edges
            plt.SetAxisLimits(xMin: 0, xMax: 10);
        }
    }

    public class FillAboveBelow : IRecipe
    {
        public string Category => "Plottable: Fill";
        public string ID => "fill_aboveBelow";
        public string Title => "Fill Above and Below";
        public string Description =>
            "Sometimes you want to share the area under a curve, but change its color depending " +
            "on which side of the baseline value it is. There's a helper method to make this easier.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample data
            Random rand = new Random(3);
            double[] xs = DataGen.Consecutive(201);
            double[] ys = DataGen.RandomWalk(rand, xs.Length);

            // add filled polygons
            plt.AddFillAboveAndBelow(xs, ys);

            // tighten the axis limits so we don't see lines on the edges
            plt.SetAxisLimits(xMin: 0, xMax: 200);
        }
    }

    public class FillBetween : IRecipe
    {
        public string Category => "Plottable: Fill";
        public string ID => "fill_between";
        public string Title => "Fill Between Curves";
        public string Description =>
            "Given two curves, a polygon can be created to give the appearance of shading between them. " +
            "Here we will display two scatter plots, then create a polygon to fill the region between them.";

        public void ExecuteRecipe(Plot plt)
        {
            // create sample data
            double[] xs = DataGen.Range(0, 10, .1, true);
            double[] ys1 = DataGen.Sin(xs);
            double[] ys2 = DataGen.Cos(xs);

            // add a polygon to fill the region between the two curves
            plt.AddFill(xs, ys1, xs, ys2);

            // add two scatter plots the traditional way
            plt.AddScatter(xs, ys1, color: Color.Black);
            plt.AddScatter(xs, ys2, color: Color.Black);

            // tighten the axis limits so we don't see lines on the edges
            plt.SetAxisLimits(xMin: 0, xMax: 10);
        }
    }
}
