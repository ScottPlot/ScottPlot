using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ScottPlot.Statistics
{
    public static class Common
    {
        private readonly static RNGCryptoServiceProvider Rand = new RNGCryptoServiceProvider();

        /// <summary>
        /// Return the standard deviation of the given values
        /// </summary>
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

        /// <summary>
        /// Return the standard error of the given values
        /// </summary>
        public static double StdErr(double[] values) => StDev(values) / Math.Sqrt(values.Length);

        /// <summary>
        /// Return the mean of the given values
        /// </summary>
        public static double Mean(double[] values)
        {
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
                sum += values[i];
            double mean = sum / values.Length;
            return mean;
        }

        /// <summary>
        /// Return the Nth smallest value in the given array.
        /// </summary>
        public static double NthOrderStatistic(double[] values, int n)
        {
            if (n < 1 || n > values.Length)
                throw new ArgumentException("n must be a number from 1 to the length of the array");

            double[] valuesCopy = new double[values.Length];
            Array.Copy(values, valuesCopy, values.Length);
            return QuickSelect(valuesCopy, 0, values.Length - 1, n - 1);
        }

        /// <summary>
        /// Return the value of the Nth quantile.
        /// </summary>
        public static double Quantile(double[] values, int n, int quantileCount)
        {
            if (n == 0)
                return values.Min();
            else if (n == quantileCount)
                return values.Max();
            else
                return NthOrderStatistic(values, n * values.Length / quantileCount);
        }

        /// <summary>
        /// Return the value of the Nth quartile
        /// </summary>
        /// <param name="values"></param>
        /// <param name="quartile">quartile 1, 2, or 3</param>
        public static double Quartile(double[] values, int quartile) => Quantile(values, quartile, 4);

        /// <summary>
        /// Return the percentile of the given array
        /// </summary>
        /// <param name="values"></param>
        /// <param name="percentile">number from 0 to 100</param>
        /// <returns></returns>
        public static double Percentile(double[] values, int percentile) => Quantile(values, percentile, 100);

        /// <summary>
        /// Return the percentile of the given array
        /// </summary>
        /// <param name="values"></param>
        /// <param name="percentile">number from 0 to 100</param>
        public static double Percentile(double[] values, double percentile)
        {
            if (percentile == 0)
                return values.Min();
            else if (percentile == 100)
                return values.Max();
            int percentileIndex = (int)(percentile / 100.0 * (values.Length - 1));

            double[] copiedValues = new double[values.Length];
            Array.Copy(values, copiedValues, values.Length);
            return NthOrderStatistic(values, percentileIndex + 1);
        }

        /// <summary>
        /// Return the median of the given array.
        /// If the length of the array is even, this value is the mean of the upper and lower medians.
        /// </summary>
        public static double Median(double[] values)
        {
            if (values.Length % 2 == 1)
            {
                return NthOrderStatistic(values, values.Length / 2 + 1);
            }
            else
            {
                double lowerMedian = NthOrderStatistic(values, values.Length / 2);
                double upperMedian = NthOrderStatistic(values, values.Length / 2 + 1);
                return (lowerMedian + upperMedian) / 2;
            }
        }

        /// <summary>
        /// Return the kth smallest value from a range of the given array.
        /// WARNING: values will be mutated.
        /// </summary>
        /// <param name="values"></param>
        /// <param name="leftIndex">inclusive lower bound</param>
        /// <param name="rightIndex">inclusive upper bound</param>
        /// <param name="k">number starting at 0</param>
        /// <returns></returns>
        private static double QuickSelect(double[] values, int leftIndex, int rightIndex, int k)
        {
            /*
             * QuickSelect (aka Hoare's Algorithm) is a selection algorithm 
             *  - Given an integer k it returns the kth smallest element in a sequence) with O(n) expected time.
             *  - In the worst case it is O(n^2), i.e. when the chosen pivot is always the max or min at each call.
             *  - The use of a random pivot virtually assures linear time performance.
             *  - https://en.wikipedia.org/wiki/Quickselect
             */

            if (leftIndex == rightIndex)
                return values[leftIndex];

            if (k == 0)
            {
                double min = values[leftIndex];
                for (int j = leftIndex; j <= rightIndex; j++)
                {
                    if (values[j] < min)
                    {
                        min = values[j];
                    }
                }

                return min;
            }

            if (k == rightIndex - leftIndex)
            {
                double max = values[leftIndex];
                for (int j = leftIndex; j <= rightIndex; j++)
                {
                    if (values[j] > max)
                    {
                        max = values[j];
                    }
                }

                return max;
            }

            int partitionIndex = Partition(values, leftIndex, rightIndex);
            int pivotIndex = partitionIndex - leftIndex;

            if (k == pivotIndex)
                return values[partitionIndex];
            else if (k < pivotIndex)
                return QuickSelect(values, leftIndex, partitionIndex - 1, k);
            else
                return QuickSelect(values, partitionIndex + 1, rightIndex, k - pivotIndex - 1);
        }

        /// <summary>
        /// Return a random integer from within the given range
        /// </summary>
        /// <param name="min">inclusive lower bound</param>
        /// <param name="max">exclusive upper bound</param>
        /// <returns></returns>
        public static int GetRandomInt(int min, int max)
        {
            byte[] randomBytes = new byte[sizeof(int)];
            Rand.GetBytes(randomBytes);
            int randomInt = BitConverter.ToInt32(randomBytes, 0);
            return Math.Abs(randomInt % (max - min + 1)) + min;
        }

        /// <summary>
        /// Partition the array between the defined bounds according to elements above and below a randomly chosen pivot value
        /// </summary>
        /// <param name="values"></param>
        /// <param name="leftIndex"></param>
        /// <param name="rightIndex"></param>
        /// <returns>index of the pivot used</returns>
        private static int Partition(double[] values, int leftIndex, int rightIndex)
        {
            // Moving the pivot to the end is far easier than handling it where it is
            // This also allows you to turn this into the non-randomized Partition
            int initialPivotIndex = GetRandomInt(leftIndex, rightIndex);
            double swap = values[initialPivotIndex];
            values[initialPivotIndex] = values[rightIndex];
            values[rightIndex] = swap;

            double pivotValue = values[rightIndex];

            int pivotIndex = leftIndex - 1;
            for (int j = leftIndex; j < rightIndex; j++)
            {
                if (values[j] <= pivotValue)
                {
                    pivotIndex++;
                    double tmp1 = values[j];
                    values[j] = values[pivotIndex];
                    values[pivotIndex] = tmp1;
                }
            }

            pivotIndex++;
            double tmp2 = values[rightIndex];
            values[rightIndex] = values[pivotIndex];
            values[pivotIndex] = tmp2;

            return pivotIndex;
        }
    }
}
