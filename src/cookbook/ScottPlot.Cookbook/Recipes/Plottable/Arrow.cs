using System;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class Arrow : IRecipe, IPlottableRecipe
    {
        public string ID => "plottable_arrow_quickstart";
        public string PlotType => "Arrow";
        public string Title => "Drawing Arrows";
        public string Description => "Arrows point to specific locations on the plot. " +
            "Arrows are actually just scatter plots with two points and an arrowhead.";

        public void ExecuteRecipe(Plot plt)
        {
            int pointCount = 51;
            double[] x = DataGen.Consecutive(pointCount);
            double[] sin = DataGen.Sin(pointCount);
            double[] cos = DataGen.Cos(pointCount);

            plt.PlotScatter(x, sin);
            plt.PlotScatter(x, cos);

            plt.PlotArrow(25, 0, 27, .2, label: "default");
            plt.PlotArrow(27, -.25, 23, -.5, label: "big", lineWidth: 10);
            plt.PlotArrow(12, 1, 12, 0, label: "skinny", arrowheadLength: 10);
            plt.PlotArrow(20, .6, 20, 1, label: "fat", arrowheadWidth: 10);
            plt.Legend(fixedLineWidth: false);
        }
    }
}
