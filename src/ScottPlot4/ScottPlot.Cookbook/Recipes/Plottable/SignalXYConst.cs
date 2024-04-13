using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Recipes.Plottable
{

    public class SignalXYConstQuickstart : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalXY();
        public string ID => "signalxyconst_quickstart";
        public string Title => "SignalConst with X and Y data";
        public string Description =>
            "SignalXYConst is a speed-optimized plot for displaying values (Ys) with unevenly-spaced positions (Xs) " +
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

            plt.AddSignalXYConst(xs, ys);
        }
    }

    public class MixedDataTypes : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalXY();
        public string ID => "signalxyconst_types";
        public string Title => "Different data types for xs and ys";
        public string Description => "SignalXYConst with (int)Xs and (float)Ys arrays";

        public void ExecuteRecipe(Plot plt)
        {
            Random rand = new Random(0);
            int pointCount = 1_000_000;
            double[] sine = DataGen.Sin(pointCount, 3);
            double[] noise = DataGen.RandomNormal(rand, pointCount, 0, 0.5);
            float[] ys = sine.Zip(noise, (s, n) => s + n).Select(x => (float)x).ToArray();
            int[] xs = Enumerable.Range(0, pointCount)
                .Select(x => (int)x)
                .Select(x => x > 500_000 ? x + 1_000_000 : x)
                .Select(x => x > 200_000 ? x + 100_000 : x)
                .ToArray();

            plt.AddSignalXYConst(xs, ys);
        }
    }

    public class SignalXYConstStep : IRecipe
    {
        public ICategory Category => new Categories.PlotTypes.SignalXY();
        public string ID => "signalxyconst_step";
        public string Title => "SignalConst Step Mode";
        public string Description =>
            "Data points can be connected with steps (instead of straight lines).";

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

            var sigxyconst = plt.AddSignalXYConst(xs, ys);
            sigxyconst.StepDisplay = true;
            plt.SetAxisLimits(18700, 18730, -49.25, -46.75);
        }
    }
}
