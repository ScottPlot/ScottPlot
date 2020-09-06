using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Statistics
{
    public static class Common
    {
        public static double StDev(double[] values)
        {
            double mean = Mean(values);
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

        public static double Mean(double[] values)
        {
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
                sum += values[i];
            double mean = sum / values.Length;
            return mean;
        }
    }
}
