using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Quickstart
{
    public class Scatter : IRecipe
    {
        public string Category => "Quickstart";
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
        }
    }

    public class Signal : IRecipe
    {
        public string Category => "Quickstart";
        public string ID => "quickstart_signal";
        public string Title => "Signal Plot";
        public string Description => "Signal plots have evenly spaced Y points. " +
            "Signal plots are very fast and can interactively display millions of data points. " +
            "There are many different types of plottable objects, each serving a different purpose.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] values = DataGen.RandomWalk(1_000_000);
            plt.AddSignal(values, sampleRate: 48_000);
            plt.XAxis2.Label = "One Million Points";
        }
    }

    public class Axis : IRecipe
    {
        public string Category => "Quickstart";
        public string ID => "quickstart_axis";
        public string Title => "Axis Labels and Limits";
        public string Description => "Axis labels and limits can be customized";

        public void ExecuteRecipe(Plot plt)
        {
            double[] time = DataGen.Consecutive(51);
            double[] voltage = DataGen.Sin(51);
            plt.AddScatter(time, voltage);

            // Axes can be customized
            plt.XAxis.Label = "Time (milliseconds)";
            plt.YAxis.Label = "Voltage (mV)";
            plt.XAxis2.Label = "Important Experiment";

            // Set axis limits to control the view
            plt.SetAxisLimits(-20, 80, -2, 2);
        }
    }
}
