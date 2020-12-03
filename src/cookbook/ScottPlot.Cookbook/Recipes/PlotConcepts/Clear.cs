using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.PlotConcepts
{
    class Clear : IRecipe
    {
        public string Category => "Add and Remove Plottables";
        public string ID => "plot_clear";
        public string Title => "Clear plottables from the plot";
        public string Description =>
            "Call Clear() to remove all plottables from the plot. " +
            "Overloads of Clear() allow you to remote one type of plottable, or a specific plottable.";

        public void ExecuteRecipe(Plot plt)
        {
            double[] xs = DataGen.Consecutive(51);
            double[] sin = DataGen.Sin(51);
            double[] cos = DataGen.Sin(51);

            plt.PlotScatter(xs, sin, color: Color.Red);
            plt.Clear();
            plt.PlotScatter(xs, cos, color: Color.Blue);
        }
    }
}
