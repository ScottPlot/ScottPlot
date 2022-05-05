using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Quickstart
{
    public class Scatter : IRecipe
    {
        public ICategory Category => new Categories.Quickstart();
        public string ID => "quickstart_scatter";
        public string Title => "Scatter Plot";
        public string Description => "Scatter plots have paired X/Y points.";

        public void ExecuteRecipe(Plot plt)
        {
            // sample data
            double[] xs = DataGen.Consecutive(51);
            double[] sin = DataGen.Sin(51);
            double[] cos = DataGen.Cos(51);

            // plot the data
            plt.AddScatter(xs, sin);
            plt.AddScatter(xs, cos);

            // customize the axis labels
            plt.Title("ScottPlot Quickstart");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");
        }
    }

    public class Signal : IRecipe
    {
        public ICategory Category => new Categories.Quickstart();
        public string ID => "quickstart_signal";
        public string Title => "Signal Plot";
        public string Description => "Signal plots have evenly spaced Y points. " +
            "Signal plots are very fast and can interactively display millions of data points. " +
            "There are many different types of plottable objects, each serving a different purpose.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = DataGen.RandomWalk(1_000_000);
            plt.AddSignal(values, sampleRate: 48_000);
            plt.Title("One Million Points");
        }
    }

    public class Axis : IRecipe
    {
        public ICategory Category => new Categories.Quickstart();
        public string ID => "quickstart_axis";
        public string Title => "Axis Labels and Limits";
        public string Description => "Axis labels and limits can be customized";

        public void ExecuteRecipe(Plot plt)
        {
            double[] time = DataGen.Consecutive(51);
            double[] voltage = DataGen.Sin(51);
            plt.AddScatter(time, voltage);

            // Axes can be customized
            plt.XAxis.Label("Time (milliseconds)");
            plt.YAxis.Label("Voltage (mV)");
            plt.XAxis2.Label("Important Experiment");

            // Set axis limits to control the view
            plt.SetAxisLimits(-20, 80, -2, 2);
        }
    }

    class Add : IRecipe
    {
        public ICategory Category => new Categories.Quickstart();
        public string ID => "quickstart_add";
        public string Title => "Manually add a Plottable";
        public string Description =>
            "You can create a plot manually, then add it to the plot with Add(). " +
            "This allows you to create custom plot types and add them to the plot.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(51);
            double[] sin = DataGen.Sin(51);

            // instantiate a plottable
            var splt = new ScottPlot.Plottable.ScatterPlot(xs, sin);

            // customize its style or change its data as desired
            splt.Color = Color.Navy;
            splt.MarkerSize = 10;
            splt.MarkerShape = MarkerShape.filledDiamond;

            // add it to the plot
            plt.Add(splt);
        }
    }

    class Remove : IRecipe
    {
        public ICategory Category => new Categories.Quickstart();
        public string ID => "quickstart_remove";
        public string Title => "Remove a Plottable";
        public string Description =>
            "Call Remove() to remove a specific plottable.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(51);
            double[] sin = DataGen.Sin(51);
            double[] cos = DataGen.Cos(51);

            var sinPlot = plt.AddScatter(xs, sin, color: Color.Red);
            var cosPlot = plt.AddScatter(xs, cos, color: Color.Blue);

            plt.Remove(sinPlot);
        }
    }

    class Clear : IRecipe
    {
        public ICategory Category => new Categories.Quickstart();
        public string ID => "quickstart_clear";
        public string Title => "Clear Plottables";
        public string Description =>
            "Call Clear() to remove all plottables from the plot. " +
            "Overloads of Clear() allow you to remote one type of plottable, or a specific plottable.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(51);
            double[] sin = DataGen.Sin(51);
            double[] cos = DataGen.Cos(51);

            plt.AddScatter(xs, sin, color: Color.Red);
            plt.Clear();
            plt.AddScatter(xs, cos, color: Color.Blue);
        }
    }

    class Legend : IRecipe
    {
        public ICategory Category => new Categories.Quickstart();
        public string ID => "quickstart_legend";
        public string Title => "Legend";
        public string Description =>
            "Most plottable objects have a Label which defines how they appear in the legend";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(51);
            double[] sin = DataGen.Sin(51);
            double[] cos = DataGen.Cos(51);

            plt.AddScatter(xs, sin, label: "sin");
            plt.AddScatter(xs, cos, label: "cos");
            plt.Legend();
        }
    }
}
