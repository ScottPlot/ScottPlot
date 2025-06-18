using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Statistics;

public enum KdeKernel
{
    Epanechnikov,
    Gaussian,
    Uniform,
    Triangular
}

public enum KdeBandWidthRule
{
    Scotts
}

public static class KernelDensity
{
    public static double Estimate(double x, IReadOnlyList<double> values, Func<double, double> kernel, double bandwidth)
    {
        if (values.Count == 0) return 0;
        if (bandwidth == 0) return 0;

        return values.Select(v => kernel((x - v) / bandwidth)).Sum() / (values.Count * bandwidth);
    }

    public static IEnumerable<double> Estimate(IEnumerable<double> xs, IReadOnlyList<double> values, Func<double, double> kernel, double bandwidth)
    {
        return xs.Select(x => Estimate(x, values, kernel, bandwidth));
    }

    public static double Estimate(double x, IReadOnlyList<double> values, KdeKernel kernel = KdeKernel.Epanechnikov, KdeBandWidthRule bandwidthRule = KdeBandWidthRule.Scotts)
    {
        var kernelFn = GetKernel(kernel);
        var bandwidth = GetBandWidth(bandwidthRule, values);

        return Estimate(x, values, kernelFn, bandwidth);
    }

    public static IEnumerable<double> Estimate(IEnumerable<double> xs, IReadOnlyList<double> values, KdeKernel kernel = KdeKernel.Epanechnikov, KdeBandWidthRule bandwidthRule = KdeBandWidthRule.Scotts)
    {
        var kernelFn = GetKernel(kernel);
        var bandwidth = GetBandWidth(bandwidthRule, values);

        return Estimate(xs, values, kernelFn, bandwidth);
    }

    private static Func<double, double> GetKernel(KdeKernel kernel)
    {
        return kernel switch
        {
            KdeKernel.Epanechnikov => EpanechnikovKernel,
            KdeKernel.Gaussian => GaussianKernel,
            KdeKernel.Uniform => UniformKernel,
            KdeKernel.Triangular => TriangularKernel,
            _ => throw new NotImplementedException(kernel.ToString()),
        };
    }

    private static double GetBandWidth(KdeBandWidthRule bandwidthRule, IReadOnlyList<double> values)
    {
        return bandwidthRule switch
        {
            KdeBandWidthRule.Scotts => ScottsRule(values),
            _ => throw new NotImplementedException(bandwidthRule.ToString()),
        };
    }

    // See https://en.wikipedia.org/wiki/Epanechnikov_distribution
    // This is an unusual distribution, but it has optimal mean squared error for KDE
    private static double EpanechnikovKernel(double x)
    {
        return Math.Abs(x) <= 1
            ? 0.75 * (1 - x * x)
            : 0;
    }

    // I'm sure we have a NormPDF function around here somewhere, and likely one that takes mu and sigma as parameters
    private static double GaussianKernel(double x)
    {
        return Math.Exp(-0.5 * x * x) / Math.Sqrt(2 * Math.PI);
    }

    // Note that this is the uniform distribution on [-1, 1], not the standard uniform distribution on [0, 1]
    private static double UniformKernel(double x)
    {
        return Math.Abs(x) <= 1
            ? 0.5
            : 0;
    }

    // On [-1, 1] with mode at 0
    // This is another unusual kernel, but it's common in KDE
    private static double TriangularKernel(double x)
    {
        if (x <= -1 || x >= 1)
        {
            return 0;
        }

        if (x <= 0)
        {
            return (2 * (x + 1)) / 2;
        }

        if (x == 0)
        {
            return 1;
        }

        if (x >= 0)
        {
            return (2 * (1 - x)) / 2;
        }

        throw new ApplicationException("This should be unreachable.");
    }

    // A common rule for bandwidth estimation
    // See https://en.wikipedia.org/wiki/Scott%27s_rule
    private static double ScottsRule(IReadOnlyList<double> values)
    {
        if (values.Count == 0)
        {
            return 0;
        }
        var stdDev = Statistics.Descriptive.StandardDeviation(values);
        var n = values.Count;

        return Math.Pow(24 * Math.Sqrt(Math.PI), 1 / 3d) * stdDev * Math.Pow(n, -1 / 3d);
    }
}
