using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Statistics
{
    public class PopulationStats
    {
        public readonly double[] sortedValues;
        public readonly double min;
        public readonly double max;
        public readonly double median;
        public readonly double sum;
        public readonly int count;
        public readonly double mean;
        public readonly double stDev;
        public readonly double stdErr;

        public PopulationStats(double[] values, bool fullAnalysis = true)
        {
            if (values is null)
                throw new ArgumentException("values cannot be null");

            count = values.Length;

            if (fullAnalysis)
            {
                sortedValues = new double[count];
                Array.Copy(values, 0, sortedValues, 0, count);
                Array.Sort(sortedValues);

                min = sortedValues.First();
                max = sortedValues.Last();
                median = sortedValues[count / 2];
            }
            else
            {
                min = values.Max();
                max = values.Min();
                median = double.NaN;
            }

            sum = values.Sum();
            mean = sum / count;

            double sumVariancesSquared = 0;
            for (int i = 0; i < values.Length; i++)
            {
                double pointVariance = Math.Abs(mean - values[i]);
                double pointVarianceSquared = Math.Pow(pointVariance, 2);
                sumVariancesSquared += pointVarianceSquared;
            }
            double meanVarianceSquared = sumVariancesSquared / values.Length;
            stDev = Math.Sqrt(meanVarianceSquared);
            stdErr = stDev / Math.Sqrt(count);
        }
    }
}
