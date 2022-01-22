using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class ErrorBarQuickstart : IRecipe
    {
        public string Category => "Plottable: Error Bar";
        public string ID => "errorBar_quickstart";
        public string Title => "Error Bar Quickstart";
        public string Description => "Error Bars allow more fine-grained control over how your error bars are shown.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 20;

            double[] dataX = DataGen.Consecutive(pointCount);
            double[] dataY = DataGen.RandomNormal(rand, pointCount, mean: 20, stdDev: 2);

            double[] errorXPositive = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();
            double[] errorXNegative = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();
            double[] errorYPositive = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();
            double[] errorYNegative = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();

            plt.AddScatter(dataX, dataY);
            plt.AddErrorBars(dataX, dataY, errorXPositive, errorXNegative, errorYPositive, errorYNegative);
        }
    }
}
