using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{

    public class SignalXYQuickstart : IRecipe
    {
        public string Category => "Plottable: SignalXY";
        public string ID => "signalxy_quickstart";
        public string Title => "SignalXY Quickstart";
        public string Description =>
            "SignalXY is a speed-optimized plot for displaying vaues (Ys) with unevenly-spaced positions (Xs) " +
            "that are in ascending order. If your data is evenly-spaced, Signal and SignalConst is faster.";

        public void ExecuteRecipe(Plot plt)
        {
            // generate random, unevenly-spaced data
            Random rand = new Random(0);
            int pointCount = 100_000;
            double[] ys = new double[pointCount];
            double[] xs = new double[pointCount];
            for (int i = 1; i < ys.Length; i++)
            {
                ys[i] = ys[i - 1] + rand.NextDouble() - .5;
                xs[i] = xs[i - 1] + rand.NextDouble();
            }

            plt.AddSignalXY(xs, ys);
        }
    }

    public class SignalXYOffset : IRecipe
    {
        public string Category => "Plottable: SignalXY";
        public string ID => "signalxy_offset";
        public string Title => "SignalXY Offset";
        public string Description =>
            "SignalXY plots can have X and Y offsets that shift all data by a defined amount.";

        public void ExecuteRecipe(Plot plt)
        {
            // generate random, unevenly-spaced data
            var rand = new Random(2);
            int pointCount = 100_000;
            double[] ys = new double[pointCount];
            double[] xs = new double[pointCount];
            for (int i = 1; i < ys.Length; i++)
            {
                ys[i] = ys[i - 1] + rand.NextDouble() - .5;
                xs[i] = xs[i - 1] + rand.NextDouble();
            }

            var sig = plt.AddSignalXY(xs, ys);
            sig.OffsetX = 10_000;
            sig.OffsetY = 100;
        }
    }

    public class HasXGaps : IRecipe
    {
        public string Category => "Plottable: SignalXY";
        public string ID => "signalxy_gaps";
        public string Title => "Signal Data with Gaps";
        public string Description => "Signal with defined Xs that contain gaps";

        public void ExecuteRecipe(Plot plt)
        {
            var rand = new Random(0);
            int pointCount = 1_000_000;
            double[] sine = DataGen.Sin(pointCount, 3);
            double[] noise = DataGen.RandomNormal(rand, pointCount, 0, 0.5);
            double[] ys = sine.Zip(noise, (s, n) => s + n).ToArray();
            double[] xs = Enumerable.Range(0, pointCount)
                .Select(x => (double)x)
                .Select(x => x > 500_000 ? x + 1_000_000 : x)
                .Select(x => x > 200_000 ? x + 100_000 : x)
                .ToArray();

            plt.AddSignalXY(xs, ys);
        }
    }

    public class SignalWithDifferentDensity : IRecipe
    {
        public string Category => "Plottable: SignalXY";
        public string ID => "signalxy_density";
        public string Title => "Different Densities";
        public string Description => "Signal with mised low and high density data";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 1_000_000;
            double[] sine = DataGen.Sin(pointCount, 3);
            double[] noise = DataGen.RandomNormal(rand, pointCount, 0, 0.5);
            double[] ys = sine.Zip(noise, (s, n) => s + n).ToArray();
            double[] xs = new double[pointCount];

            double currentX = 0;
            for (int i = 0; i < pointCount; i++)
            {
                if ((i % 100000) < 10)
                    currentX += 10;
                else
                    currentX += 0.0001;
                xs[i] = currentX;
            }

            plt.AddSignalXY(xs, ys);
        }
    }
}
