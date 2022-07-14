using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class FunctionQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.Function();
        public string ID => "function_quickstart";
        public string Title => "Function";
        public string Description =>
            "Function plots are defined by a function (not X/Y data points) so the curve " +
            "is continuous and can be zoomed in and out infinitely";

        public void ExecuteRecipe(Plot plt)
        {
            // Functions are defined as delegates with an input and output
            var func1 = new Func<double, double?>((x) => Math.Sin(x) * Math.Sin(x / 2));
            var func2 = new Func<double, double?>((x) => Math.Sin(x) * Math.Sin(x / 3));
            var func3 = new Func<double, double?>((x) => Math.Cos(x) * Math.Sin(x / 5));

            // Add functions to the plot
            plt.AddFunction(func1, lineWidth: 2);
            plt.AddFunction(func2, lineWidth: 2, lineStyle: LineStyle.Dot);
            plt.AddFunction(func3, lineWidth: 2, lineStyle: LineStyle.Dash);

            // Manually set axis limits because functions do not have discrete data points
            plt.SetAxisLimits(-10, 10, -1.5, 1.5);
        }
    }
}
