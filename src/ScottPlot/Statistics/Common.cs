using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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

        public static double NthOrderStatistic(double[] values, int n)
        {
            double[] copied_values = new double[values.Length];
            Array.Copy(values, copied_values, values.Length); // QuickSelect mutates the array

            return QuickSelect(copied_values, 0, values.Length - 1, n - 1);
        }

        private static double QuickSelect(double[] values, int begin, int end, int i)
        {
            // QuickSelect (aka Hoare's Algorithm) is a selection algorithm (i.e. given an integer i it returns the ith smallest element in a sequence) with O(n) expected time.
            // In the worst case it is O(n^2), i.e. when the chosen pivot is always the max or min at each call. It is very similar to QuickSort and developed by the same man.
            // The use of a random pivot virtually assures linear time performance.
            // There is a guaranteed linear time algorithm which uses median of medians to choose a pivot but the overhead is often not worth it.

            if (begin == end)
            {
                return values[begin];
            }

            if(i == 0)
			{
                double min = values[begin];
                for(int j = begin; j <= end; j++)
				{
                    if(values[j] < min)
					{
                        min = values[j];
					}
				}

                return min;
			}

            if(i == end - begin)
			{
                double max = values[begin];
                for (int j = begin; j <= end; j++)
                {
                    if (values[j] > max)
                    {
                        max = values[j];
                    }
                }

                return max;
            }

            int pivot_index = Partition(values, begin, end);
            int k = pivot_index - begin;

            if (i == k)
            {
                return values[pivot_index];
            }
            else if (i < k)
            {
                return QuickSelect(values, begin, pivot_index - 1, i);
            }
            else
            {
                return QuickSelect(values, pivot_index + 1, end, i - k - 1);
            }
        }

        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider(); // In principle QuickSelect could have its performance degraded through timing attacks, hence the CSPRNG
        private static int Partition(double[] values, int begin, int end)
        {
            byte[] random_bytes = new byte[sizeof(int)];
            rand.GetBytes(random_bytes);
            int pivot_index = Math.Abs(BitConverter.ToInt32(random_bytes, 0) % (end - begin)) + begin; // Modulo can return negative numbers in C# if the dividend is negative


            // Moving the pivot to the end is far easier than handling it where it is
            // This also allows you to turn this into the non-randomized Partition
            double swap = values[pivot_index];
            values[pivot_index] = values[end];
            values[end] = swap;

            double pivot = values[end];

            int i = begin - 1;
            for (int j = begin; j < end; j++)
            {
                if (values[j] <= pivot)
                {
                    i++;
                    double tmp1 = values[j];
                    values[j] = values[i];
                    values[i] = tmp1;
                }
            }

            i++;
            double tmp2 = values[end];
            values[end] = values[i];
            values[i] = tmp2;

            return i;
        }
    }
}
