using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotCookbook.Recipes.Miscellaneous;

public class KernelDensityEstimation : ICategory
{
    public Chapter Chapter => Chapter.General;
    public string CategoryName => "Kernel Density Estimation";
    public string CategoryDescription => "Kernel Density Estimation (KDE) can be used to estimate a PDF for a histogram, allowing the creation of density plots";
    public class KdeQuickstart : RecipeBase
    {
        public override string Name => "Density Plot";
        public override string Description => "Density Plots use KDE to estimate a PDF.";

        [Test]
        public override void Execute()
        {
            var ys = SampleData.Faithful;

            var hist = Histogram.WithBinCount(80, ys);

            var histPlot = myPlot.Add.Histogram(hist);
            histPlot.BarWidthFraction = 0.8;

            var densityEstimate = hist.Bins.Select((x, i) => KernelDensity.Estimate(x, ys)).ToArray();
            double scale = ys.Length;

            var rescaledDensityEstimate = densityEstimate.Select(x => x * scale).ToArray();

            var scat = myPlot.Add.Scatter(hist.Bins, rescaledDensityEstimate, Colors.Red);
            scat.MarkerSize = 0;
        }
    }

    public class KdeKernelOptions : RecipeBase
    {
        public override string Name => "Density Plot Kernels";
        public override string Description => "Several choices of kernels are available.";

        [Test]
        public override void Execute()
        {
            var ys = SampleData.Faithful;

            var hist = Histogram.WithBinCount(80, ys);

            var histPlot = myPlot.Add.Histogram(hist);
            histPlot.BarWidthFraction = 0.8;
            foreach (var bar in histPlot.Bars)
            {
                bar.FillColor = Colors.LightBlue;
            }

            foreach (var kernel in Enum.GetValues<KdeKernel>())
            {
                var densityEstimate = hist.Bins.Select((x, i) => KernelDensity.Estimate(x, ys, kernel)).ToArray();
                double scale = ys.Length;

                var rescaledDensityEstimate = densityEstimate.Select(x => x * scale).ToArray();

                var scat = myPlot.Add.Scatter(hist.Bins, rescaledDensityEstimate);
                scat.MarkerSize = 0;
                scat.LegendText = kernel.ToString();
            }
        }
    }
}
