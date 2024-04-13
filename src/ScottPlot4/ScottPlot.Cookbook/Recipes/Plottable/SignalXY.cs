using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{

    public class SignalXYQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalXY();
        public string ID => "signalxy_quickstart";
        public string Title => "SignalXY Quickstart";
        public string Description =>
            "SignalXY is a speed-optimized plot for displaying values (Ys) with unevenly-spaced positions (Xs) " +
            "that are in ascending order. If your data is evenly-spaced, Signal and SignalConst is faster.";

        public void ExecuteRecipe(Plot plt)
        {
            (double[] xs, double[] ys) = DataGen.RandomWalk2D(new Random(0), 5_000);

            plt.AddSignalXY(xs, ys);
        }
    }

    public class SignalXYOffset : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalXY();
        public string ID => "signalxy_offset";
        public string Title => "SignalXY Offset";
        public string Description =>
            "SignalXY plots can have X and Y offsets that shift all data by a defined amount.";

        public void ExecuteRecipe(Plot plt)
        {
            (double[] xs, double[] ys) = DataGen.RandomWalk2D(new Random(0), 5_000);

            var sig = plt.AddSignalXY(xs, ys);
            sig.OffsetX = 10_000;
            sig.OffsetY = 100;
        }
    }

    public class SignalScale : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalXY();
        public string ID => "signalxy_scale";
        public string Title => "SignalXY Scale";
        public string Description =>
            "SignalXY plots can have a Y scale that multiply all data by a defined amount. " +
            "ScaleY is applied before OffsetX and OffsetY.";

        public void ExecuteRecipe(Plot plt)
        {
            // display 100,000 values between -1 and +1
            double[] values = DataGen.Sin(100_000, oscillations: 10);
            double[] xs = ScottPlot.Generate.Consecutive(values.Length);
            var sigxy = plt.AddSignalXY(xs, values);

            // scale Y by 500 so values span -500 to +500
            sigxy.ScaleY = 500;
        }
    }

    public class HasXGaps : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalXY();
        public string ID => "signalxy_gaps";
        public string Title => "Signal Data with Gaps";
        public string Description => "Signal with defined Xs that contain gaps";

        public void ExecuteRecipe(Plot plt)
        {
            var rand = new Random(0);
            int pointCount = 10_000;
            double[] sine = DataGen.Sin(pointCount, 3);
            double[] noise = DataGen.RandomNormal(rand, pointCount, 0, 0.5);
            double[] ys = sine.Zip(noise, (s, n) => s + n).ToArray();
            double[] xs = Enumerable.Range(0, pointCount)
                .Select(x => (double)x)
                .Select(x => x > 3_000 ? x + 10_000 : x)
                .Select(x => x > 7_000 ? x + 20_000 : x)
                .ToArray();

            plt.AddSignalXY(xs, ys);
        }
    }

    public class SignalWithDifferentDensity : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalXY();
        public string ID => "signalxy_density";
        public string Title => "Different Densities";
        public string Description => "Signal with mised low and high density data";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new(0);
            int pointCount = 5_000;
            double[] sine = DataGen.Sin(pointCount, 3);
            double[] noise = DataGen.RandomNormal(rand, pointCount, 0, 0.5);
            double[] ys = sine.Zip(noise, (s, n) => s + n).ToArray();
            double[] xs = new double[pointCount];

            double x = 0;
            for (int i = 0; i < pointCount; i++)
            {
                bool lowDensityPoint = (i % 1_000) < 10;
                x += lowDensityPoint ? 10 : .05;
                xs[i] = x;
            }

            plt.AddSignalXY(xs, ys);
        }
    }

    public class SignalXYStep : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalXY();
        public string ID => "signalxy_step";
        public string Title => "SignalXY Step Mode";
        public string Description =>
            "Data points can be connected with steps (instead of straight lines).";

        public void ExecuteRecipe(Plot plt)
        {
            (double[] xs, double[] ys) = DataGen.RandomWalk2D(new Random(0), 5_000);

            var sigxy = plt.AddSignalXY(xs, ys);
            sigxy.StepDisplay = true;
            sigxy.MarkerSize = 0;

            plt.SetAxisLimits(110, 140, 17.5, 27.5);
        }
    }

    public class SignalXYFillBelow : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalXY();
        public string ID => "signalxy_fillBelow";
        public string Title => "SignalXY with Fill";
        public string Description =>
            "Various options allow shading above/below the signal data.";

        public void ExecuteRecipe(Plot plt)
        {
            (double[] xs, double[] ys) = DataGen.RandomWalk2D(new Random(0), 5_000);

            var sigxy = plt.AddSignalXY(xs, ys);
            sigxy.FillBelow();

            plt.Margins(x: 0);
        }
    }

    public class SignalCustomMarkers : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalXY();
        public string ID => "signalxy_markers";
        public string Title => "Customize Markers";
        public string Description => "SignalXY plots have markers which only appear when they are zoomed in.";

        public void ExecuteRecipe(Plot plt)
        {
            var rand = new Random(0);
            double[] ys = DataGen.RandomWalk(rand, 200);
            double[] xs = DataGen.Consecutive(200);

            var sig = plt.AddSignalXY(xs, ys);
            sig.MarkerShape = MarkerShape.filledTriangleUp;
            sig.MarkerSize = 10;

            plt.SetAxisLimits(100, 120, 10, 15);
        }
    }
}
