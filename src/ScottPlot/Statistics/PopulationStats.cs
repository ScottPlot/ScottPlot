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
        public readonly double Q1;
        public readonly double Q3;
        public readonly double IQR;
        public readonly double[] lowOutliers;
        public readonly double[] highOutliers;
        public readonly double maxNonOutlier;
        public readonly double minNonOutlier;

        public PopulationStats(double[] values, bool fullAnalysis = true)
        {
            if (values is null)
                throw new ArgumentException("values cannot be null");

            count = values.Length;

            if (fullAnalysis)
            {
                int QSize = (int)Math.Floor(count / 4.0);

                sortedValues = new double[count];
                Array.Copy(values, 0, sortedValues, 0, count);
                Array.Sort(sortedValues);

                min = sortedValues.First();
                max = sortedValues.Last();
                median = sortedValues[count / 2];

                Q1 = sortedValues[QSize];
                Q3 = sortedValues[sortedValues.Length - QSize - 1];
                IQR = Q3 - Q1;

                double lowerBoundary = Q1 - 1.5 * IQR;
                double upperBoundary = Q3 + 1.5 * IQR;

                //These could be made faster knowing that we have already sorted the list above 
                lowOutliers = sortedValues.Where(y => y < lowerBoundary).ToArray();
                highOutliers = sortedValues.Where(y => y > upperBoundary).ToArray();
                minNonOutlier = sortedValues.Where(y => y > lowerBoundary).FirstOrDefault();
                maxNonOutlier = sortedValues.Where(y => y < upperBoundary).LastOrDefault();
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
