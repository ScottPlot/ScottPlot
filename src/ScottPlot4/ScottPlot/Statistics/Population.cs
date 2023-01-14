using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Statistics
{
    /// <summary>
    /// This module holds an array of values and provides popultation statistics (mean, median, standard deviation, etc)
    /// </summary>
    public class Population
    {
        public double[] values { get; private set; }
        public double[] sortedValues { get; private set; }
        public double min { get; private set; }
        public double max { get; private set; }
        public double median { get; private set; }
        public double sum { get; private set; }
        public int count { get; private set; }
        public double mean { get; private set; }
        public double stDev { get; private set; }
        public double plus3stDev { get; private set; }
        public double minus3stDev { get; private set; }
        public double plus2stDev { get; private set; }
        public double minus2stDev { get; private set; }
        public double stdErr { get; private set; }
        public double Q1 { get; private set; }
        public double Q3 { get; private set; }
        public double IQR { get; private set; }
        public double[] lowOutliers { get; private set; }
        public double[] highOutliers { get; private set; }
        public double maxNonOutlier { get; private set; }
        public double minNonOutlier { get; private set; }
        public int n { get { return values.Length; } }
        public double span { get { return sortedValues.Last() - sortedValues.First(); } }

        /// <summary>
        /// Generate random values with a normal distribution
        /// </summary>
        public Population(Random rand, int pointCount, double mean = .5, double stdDev = .5)
        {
            values = DataGen.RandomNormal(rand, pointCount, mean, stdDev);
            Recalculate();
        }

        /// <summary>
        /// Calculate population stats from the given array of values
        /// </summary>
        public Population(double[] values)
        {
            if (values is null)
                throw new ArgumentException("values cannot be null");

            this.values = values;
            Recalculate();
        }

        [Obsolete("This constructor overload is deprecated. Please remove the fullAnalysis argument.")]
        public Population(double[] values, bool fullAnalysis = true)
        {
            if (values is null)
                throw new ArgumentException("values cannot be null");

            this.values = values;
            if (fullAnalysis)
                Recalculate();
        }

        public void Recalculate()
        {
            count = values.Length;

            int QSize = (int)Math.Floor(count / 4.0);

            sortedValues = new double[count];
            Array.Copy(values, 0, sortedValues, 0, count);
            Array.Sort(sortedValues);

            min = sortedValues.First();
            max = sortedValues.Last();

            // median is average of the two values in the middle if value count is even
            median = count % 2 == 0 ? ((sortedValues[(count / 2) - 1] + sortedValues[count / 2]) / 2d) : sortedValues[count / 2];

            Q1 = sortedValues[QSize];
            Q3 = sortedValues[sortedValues.Length - QSize - 1];
            IQR = Q3 - Q1;

            double lowerBoundary = Q1 - 1.5 * IQR;
            double upperBoundary = Q3 + 1.5 * IQR;

            int minNonOutlierIndex = 0;
            int maxNonOutlierIndex = 0;

            for (int i = 0; i < sortedValues.Length; i++)
            {
                if (sortedValues[i] < lowerBoundary)
                {

                }
                else
                {
                    minNonOutlierIndex = i;
                    break;
                }
            }

            for (int i = sortedValues.Length - 1; i >= 0; i--)
            {
                if (sortedValues[i] > upperBoundary)
                {

                }
                else
                {
                    maxNonOutlierIndex = i;
                    break;
                }
            }

            lowOutliers = new double[minNonOutlierIndex];
            highOutliers = new double[sortedValues.Length - maxNonOutlierIndex - 1];
            Array.Copy(sortedValues, 0, lowOutliers, 0, lowOutliers.Length);
            Array.Copy(sortedValues, maxNonOutlierIndex + 1, highOutliers, 0, highOutliers.Length);
            minNonOutlier = sortedValues[minNonOutlierIndex];
            maxNonOutlier = sortedValues[maxNonOutlierIndex];

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
            plus2stDev = mean + stDev * 2;
            minus2stDev = mean - stDev * 2;
            plus3stDev = mean + stDev * 3;
            minus3stDev = mean - stDev * 3;
            stdErr = stDev / Math.Sqrt(count);
        }

        public double[] GetDistribution(double[] xs, bool normalize)
        {
            double[] ys = new double[xs.Length];
            for (int i = 0; i < xs.Length; i++)
                ys[i] = Math.Exp(-.5 * Math.Pow((xs[i] - mean) / stDev, 2));

            if (normalize)
            {
                double sum = ys.Sum();
                for (int i = 0; i < ys.Length; i++)
                    ys[i] /= sum;
            }

            return ys;
        }
    }
}
