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

    public class ErrorBarSymmetric: IRecipe
    {
        public string Category => "Plottable: Error Bar";
        public string ID => "errorBar_symmetric";
        public string Title => "Symmetric Error Bars";
        public string Description => "There's a shorthand method for error bars where the positive and negative error is the same.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 20;

            double[] dataX = DataGen.Consecutive(pointCount);
            double[] dataY = DataGen.RandomNormal(rand, pointCount, mean: 20, stdDev: 2);

            double[] errorX= DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();
            double[] errorY= DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();

            plt.AddScatter(dataX, dataY);
            plt.AddErrorBars(dataX, dataY, errorX, errorY);
        }
    }

    public class ErrorBarOneDimension: IRecipe
    {
        public string Category => "Plottable: Error Bar";
        public string ID => "errorBar_oneDimension";
        public string Title => "Error Bars in One Dimension";
        public string Description => "If you only have error data for one dimension you can simply pass in null for the other dimension.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 20;

            double[] dataX = DataGen.Consecutive(pointCount);
            double[] dataY = DataGen.RandomNormal(rand, pointCount, mean: 20, stdDev: 2);

            double[] error = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();

            plt.AddScatter(dataX, dataY);
            plt.AddErrorBars(dataX, dataY, null, error);
        }
    }

    public class ErrorBarCustomization: IRecipe
    {
        public string Category => "Plottable: Error Bar";
        public string ID => "errorBar_customization";
        public string Title => "Customization";
        public string Description => "You can customize the colour, cap size, and line width of the error bars.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 20;

            double[] dataX = DataGen.Consecutive(pointCount);
            double[] dataY = DataGen.RandomNormal(rand, pointCount, mean: 20, stdDev: 2);

            double[] error = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();

            plt.AddScatter(dataX, dataY);
            var errorBars = plt.AddErrorBars(dataX, dataY, null, error);
            errorBars.CapSize = 6;
            errorBars.Color = System.Drawing.Color.LightBlue;
            errorBars.LineWidth = 2;
        }
    }
}
