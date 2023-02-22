using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Statistics;

public static class Descriptive
{
    public static double Mean(double[] values)
    {
        return values.Sum() / values.Length;
    }

    public static double StDev(double[] values)
    {
        return StDev(values, Mean(values));
    }

    public static double StDev(double[] values, double mean)
    {
        double sumVariancesSquared = 0;
        for (int i = 0; i < values.Length; i++)
        {
            double pointVariance = Math.Abs(mean - values[i]);
            double pointVarianceSquared = Math.Pow(pointVariance, 2);
            sumVariancesSquared += pointVarianceSquared;
        }
        double meanVarianceSquared = sumVariancesSquared / values.Length;
        double stDev = Math.Sqrt(meanVarianceSquared);
        return stDev;
    }
}
