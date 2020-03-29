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
    public class Histogram
    {
        public double[] values;
        public Population population;
        public double[] bins;
        public double[] counts;
        public double[] cumulativeCounts;
        public double[] countsFrac;
        public double[] countsFracCurve;
        public double[] cumulativeFrac;
        public double binSize { get { return bins[1] - bins[0]; } }

        public double mean { get { return population.mean; } }
        public double stdev { get { return population.stDev; } }

        public Histogram(double[] values, double? min = null, double? max = null, double? binSize = null, double? binCount = null, bool ignoreOutOfBounds = true)
        {
            this.values = values;
            population = new Population(values);

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
            counts = GetHistogram(values, bins, ignoreOutOfBounds);
            cumulativeCounts = GetCumulative(counts);
            countsFrac = GetNormalized(counts);
            cumulativeFrac = GetCumulative(countsFrac);
            countsFracCurve = population.GetDistribution(bins, false);
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
