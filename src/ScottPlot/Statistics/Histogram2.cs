using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Statistics
{
    /// <summary>
    /// This class contains experimental histogram methods
    /// </summary>
    public static class Histogram2
    {
        /// <summary>
        /// Compute the histogram of a dataset.
        /// </summary>
        /// <param name="values">Input data</param>
        /// <param name="binCount">Number of equal-width bins</param>
        /// <param name="density">If False, the result will contain the number of samples in each bin. If True, the result is the value of the probability density function at the bin (the sum of all values will be 1 if the bin size is 1).</param>
        /// <param name="min">Lower edge of the first bin (inclusive). If NaN, minimum of input values will be used.</param>
        /// <param name="max">High edge of the largest bin (inclusive). If NaN, maximum of input values will be used.</param>
        /// <returns></returns>
        public static (double[] hist, double[] binEdges) Histogram(double[] values, int binCount, bool density = false, double min = double.NaN, double max = double.NaN)
        {
            // determine min/max based on the data (if not provided)
            if (double.IsNaN(min) || double.IsNaN(max))
            {
                var stats = new BasicStats(values);
                if (double.IsNaN(min))
                    min = stats.Min;
                if (double.IsNaN(max))
                    max = stats.Max;
            }

            // create evenly sized bins
            double binWidth = (max - min) / binCount;
            double[] binEdges = new double[binCount + 1];
            for (int i = 0; i < binEdges.Length; i++)
                binEdges[i] = min + binWidth * i;

            // place values in histogram
            double[] hist = new double[binCount];
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] < min || values[i] > max)
                {
                    continue;
                }

                if (values[i] == max)
                {
                    hist[values.Length - 1] += 1;
                    continue;
                }

                double distanceFromMin = values[i] - min;
                int binsFromMin = (int)(distanceFromMin / binWidth);
                hist[binsFromMin] += 1;
            }

            // optionally normalize the data
            if (density)
            {
                double binScale = hist.Sum() * binWidth;
                for (int i = 0; i < hist.Length; i++)
                    hist[i] /= binScale;
            }

            return (hist, binEdges);
        }
    }
}
