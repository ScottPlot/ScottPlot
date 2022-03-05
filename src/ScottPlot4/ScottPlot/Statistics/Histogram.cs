using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    [Obsolete("ScottPlot.Histogram is now ScottPlot.Statistics.Histogram", true)]
    public class Histogram
    {
        public Histogram(double[] values, double? min = null, double? max = null, double? binSize = null, double? binCount = null, bool ignoreOutOfBounds = true)
        {
            throw new NotImplementedException("ScottPlot.Histogram is now ScottPlot.Statistics.Histogram");
        }
    };
}

namespace ScottPlot.Statistics
{
    [Obsolete("This module is obsolete. Use ScottPlot.Statistics.Common methods instead.", error: false)]
    public class Histogram
    {
        /// <summary>
        /// Lower edges of bins used to create the histogram
        /// </summary>
        public readonly double[] bins;

        /// <summary>
        /// Total number of values in each bin.
        /// </summary>
        public readonly double[] counts;

        /// <summary>
        /// Fractional number of values in each bin.
        /// The total of all values in this array is 1.0.
        /// </summary>
        public readonly double[] countsFrac;

        /// <summary>
        /// Cumulative total number of values in each bin.
        /// The returned array will start near 0.0 and end near 1.0.
        /// </summary>
        public readonly double[] cumulativeCounts;

        /// <summary>
        /// Probability density (fraction) for each bin based on the mean and standard deviation of the population.
        /// The sum of all these values is 1.0
        /// </summary>
        public readonly double[] probability;

        /// <summary>
        /// This is the probability density curve normalized to its peak, so its maximum value is 1.0
        /// </summary>
        public readonly double[] countsFracCurve;

        /// <summary>
        /// Cumulative probability density fraction for each bin
        /// </summary>
        public readonly double[] cumulativeFrac;

        /// <summary>
        /// Distance between each bin
        /// </summary>
        public readonly double binSize;

        /// <summary>
        /// Population mean
        /// </summary>
        public readonly double mean;

        /// <summary>
        /// Population standard deviation
        /// </summary>
        public readonly double stdev;

        /// <summary>
        /// Compute the histogram of a set of data.
        /// Bins are identically sized and evenly spaced.
        /// </summary>
        /// <param name="values">input data</param>
        /// <param name="min">manually-defined lower edge of first bin</param>
        /// <param name="max">manually-defined upper edge of last bin</param>
        /// <param name="binSize">manually-defined width of each bin</param>
        /// <param name="binCount">resize bins as needed so this number of bins is achieved</param>
        /// <param name="ignoreOutOfBounds">if True, values below min or above max will be ignored</param>
        public Histogram(double[] values, double? min = null, double? max = null, double? binSize = null, double? binCount = null, bool ignoreOutOfBounds = true)
        {
            var population = new Population(values);
            mean = population.mean;
            stdev = population.stDev;

            min = (min is null) ? population.minus3stDev : min.Value;
            max = (max is null) ? population.plus3stDev : max.Value;

            if (min >= max)
                throw new ArgumentException($"max ({max}) cannot be greater than min ({min})");

            if ((binCount != null) && (binSize != null))
                throw new ArgumentException("binCount and binSize cannot both be given");

            double defaultBinCount = 100;
            double span = max.Value - min.Value;
            if (binSize == null)
            {
                if (binCount == null)
                    binSize = span / defaultBinCount;
                else
                    binSize = span / binCount;
            }

            if (ignoreOutOfBounds == false)
            {
                // add an extra bin on each side of the histogram
                min -= binSize;
                max += binSize;
            }

            bins = BinBySize((double)binSize, (double)min, (double)max);
            this.binSize = bins[1] - bins[0];
            counts = GetHistogram(values, bins, ignoreOutOfBounds);
            cumulativeCounts = GetCumulative(counts);
            countsFrac = GetNormalized(counts);
            cumulativeFrac = GetCumulative(countsFrac);
            countsFracCurve = population.GetDistribution(bins, false);
            probability = population.GetDistribution(bins, true);
        }

        private static double[] GetNormalized(double[] values)
        {
            double[] countsFrac = new double[values.Length];
            for (int i = 0; i < countsFrac.Length; i++)
                countsFrac[i] = values[i] / values.Sum();
            return countsFrac;
        }
        private static double[] GetCumulative(double[] values)
        {
            double[] cumulaltive = new double[values.Length];
            cumulaltive[0] = values[0];
            for (int i = 1; i < cumulaltive.Length; i++)
                cumulaltive[i] = cumulaltive[i - 1] + values[i];
            return cumulaltive;
        }

        public static double[] BinBySize(double binSize, double min, double max)
        {
            double span = (double)max - (double)min;
            int binCount = (int)(span / binSize);
            double[] bins = new double[binCount];
            for (int i = 0; i < bins.Length; i++)
                bins[i] = i * (double)binSize + (double)min;
            return bins;
        }

        private static double[] GetHistogram(double[] values, double[] bins, bool ignoreOutOfBounds = true)
        {
            double binSize = bins[1] - bins[0];
            double[] counts = new double[bins.Length];
            for (int i = 0; i < values.Length; i++)
            {
                int index = (int)((values[i] - bins[0]) / binSize);
                if (index < 0)
                {
                    if (!ignoreOutOfBounds)
                        counts[0] += 1;
                }
                else if (index >= counts.Length)
                {
                    if (!ignoreOutOfBounds)
                        counts[counts.Length - 1] += 1;
                }
                else
                {
                    counts[index] += 1;
                }
            }
            return counts;
        }
    }
}
