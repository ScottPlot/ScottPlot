using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{
    public class ErrorBarQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.ErrorBar();
        public string ID => "errorBar_quickstart";
        public string Title => "Error Bar Quickstart";
        public string Description => "Error Bars allow more fine-grained control over how your error bars are shown.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 20;

            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = DataGen.RandomNormal(rand, pointCount, mean: 20, stdDev: 2);

            double[] xErrPos = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();
            double[] xErrNeg = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();
            double[] yErrPos = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();
            double[] yErrNeg = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();

            plt.AddScatter(xs, ys, System.Drawing.Color.Blue, lineStyle: LineStyle.Dot);
            plt.AddErrorBars(xs, ys, xErrPos, xErrNeg, yErrPos, yErrNeg, System.Drawing.Color.Blue);
        }
    }

    public class ErrorBarSymmetric : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.ErrorBar();
        public string ID => "errorBar_symmetric";
        public string Title => "Symmetric Error Bars";
        public string Description => "There's a shorthand method for error bars where the positive and negative error is the same.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 20;

            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = DataGen.RandomNormal(rand, pointCount, mean: 20, stdDev: 2);

            double[] xErr = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();
            double[] yErr = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();

            plt.AddScatter(xs, ys, System.Drawing.Color.Blue, lineStyle: LineStyle.Dot);
            plt.AddErrorBars(xs, ys, xErr, yErr, System.Drawing.Color.Blue);
        }
    }

    public class ErrorBarOneDimension : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.ErrorBar();
        public string ID => "errorBar_oneDimension";
        public string Title => "Error Bars in One Dimension";
        public string Description => "If you only have error data for one dimension you can simply pass in null for the other dimension.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 20;

            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = DataGen.RandomNormal(rand, pointCount, mean: 20, stdDev: 2);

            double[] yErr = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();

            plt.AddScatter(xs, ys, System.Drawing.Color.Blue, lineStyle: LineStyle.Dot);
            plt.AddErrorBars(xs, ys, null, yErr, System.Drawing.Color.Blue);
        }
    }

    public class ErrorBarCustomization : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.ErrorBar();
        public string ID => "errorBar_customization";
        public string Title => "Customization";
        public string Description => "You can customize the colour, cap size, and line width of the error bars.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 20;

            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = DataGen.RandomNormal(rand, pointCount, mean: 20, stdDev: 2);

            double[] yErr = DataGen.RandomNormal(rand, pointCount).Select(e => Math.Abs(e)).ToArray();

            plt.AddScatter(xs, ys, System.Drawing.Color.Blue, lineStyle: LineStyle.Dot);

            var errorBars = plt.AddErrorBars(xs, ys, null, yErr);
            errorBars.CapSize = 8;
            errorBars.Color = System.Drawing.Color.Green;
            errorBars.LineWidth = 3;
        }
    }

    public class ErrorBarMarker : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.ErrorBar();
        public string ID => "errorBar_marker";
        public string Title => "Error Bar Marker";
        public string Description => "An optional marker can be drawn at the center X/Y position for each error bar.";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 50;
            double[] xs = DataGen.Consecutive(pointCount);
            double[] ys = DataGen.NoisyBellCurve(rand, pointCount);
            double[] yErr = DataGen.Random(rand, pointCount, multiplier: .2, offset: .05);

            plt.AddErrorBars(xs, ys, null, yErr, markerSize: 5);
        }
    }
}
