using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class ErrorBarQuickstart : IRecipe
    {
        public string Category => "Plottable: Error Bar";
        public string ID => "errorbar_quickstart";
        public string Title => "Error Bars";
        public string Description => 
            "Error bars can be added to scatter plots (or any plot type) by creating an ErrorBar plottable " + 
            "and rendering it above the original plot type.";

        public void ExecuteRecipe(Plot plt)
        {
            // X values will be an ascending series of numbers
            int pointCount = 20;
            double[] xs = DataGen.Consecutive(pointCount);

            // create scatter plots to display random Y data
            Random rand = new Random(0);
            double[] ys1 = DataGen.RandomNormal(rand, pointCount, mean: 20, stdDev: 2);
            double[] ys2 = DataGen.RandomNormal(rand, pointCount, mean: 10, stdDev: 2);
            double[] ys3 = DataGen.RandomNormal(rand, pointCount, mean: 0, stdDev: 2);
            plt.PlotScatter(xs, ys1, lineStyle: LineStyle.Dot, color: Color.Red);
            plt.PlotScatter(xs, ys2, lineStyle: LineStyle.Dot, color: Color.Green);
            plt.PlotScatter(xs, ys3, lineStyle: LineStyle.Dot, color: Color.Blue);

            // create errorbar plots to display random error sizes at those same points
            double[] randomError1 = DataGen.RandomNormal(rand, pointCount);
            double[] randomError2 = DataGen.RandomNormal(rand, pointCount);
            double[] randomError3 = DataGen.RandomNormal(rand, pointCount);
            double[] randomError4 = DataGen.RandomNormal(rand, pointCount);

            // Add vertical error bars to one of the plots
            plt.AddErrorBarsY(xs, ys1, randomError1, Color.Red);

            // Add horizontmal error bars to one of the plots
            plt.AddErrorBarsX(xs, ys2, randomError2, Color.Green);

            // Add custom error bars (unique values in every direction) to one of the plots
            plt.AddErrorBars(xs, ys3, randomError1, randomError2, randomError3, randomError4, Color.Blue);
        }
    }
}
