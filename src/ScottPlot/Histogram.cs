using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public class Histogram
    {
        public double[] values;
        public double[] bins;
        public double[] counts;
        public double[] countsFrac;
        public double[] cumulativeFrac;
        public double binSpacing { get { return bins[1] - bins[0]; } }

        public Histogram(double[] values)
        {
            this.values = values;
            BinByCount(50);
            //BinBySize(5);
        }

        private void UpdateCountsFromBins()
        {
            counts = GetHistogram(values, bins);

            countsFrac = new double[counts.Length];
            for (int i = 0; i < countsFrac.Length; i++)
                countsFrac[i] = counts[i] / counts.Sum();

            cumulativeFrac = new double[counts.Length];
            cumulativeFrac[0] = counts[0];
            for (int i = 1; i < cumulativeFrac.Length; i++)
                cumulativeFrac[i] = cumulativeFrac[i - 1] + counts[i];
        }

        public void BinBySize(double binSize, double? min = null, double? max = null)
        {
            min = (min == null) ? values.Min() : (double)min;
            max = (max == null) ? values.Max() : (double)max;
            double span = (double)max - (double)min;
            int binCount = (int)(span / binSize) + 1;
            bins = new double[binCount];
            for (int i = 0; i < bins.Length; i++)
                bins[i] = i * (double)binSize + (double)min;
            UpdateCountsFromBins();
        }

        public void BinByCount(int binCount, double? min = null, double? max = null)
        {
            min = (min == null) ? values.Min() : (double)min;
            max = (max == null) ? values.Max() : (double)max;
            double span = (double)max - (double)min;
            double binSize = span / binCount;
            bins = new double[binCount];
            for (int i = 0; i < bins.Length; i++)
                bins[i] = i * (double)binSize + (double)min;
            UpdateCountsFromBins();
        }

        private static double[] GetHistogram(double[] values, double[] bins)
        {
            double binSize = bins[1] - bins[0];
            double[] counts = new double[bins.Length];
            for (int i = 0; i < values.Length; i++)
            {
                int index = (int)((values[i] - bins[0]) / binSize);
                if (index < 0)
                    counts[0] += 1;
                else if (index >= counts.Length)
                    counts[counts.Length - 1] += 1;
                else
                    counts[index] += 1;
            }
            return counts;
        }
    }
}
