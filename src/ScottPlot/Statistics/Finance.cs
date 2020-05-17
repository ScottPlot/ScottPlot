using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Statistics
{
    public static class Finance
    {
        public static double[] SMA(double[] values, int period)
        {
            if (period < 2)
                throw new ArgumentException("period must be 2 or greater");

            if (period > values.Length)
                throw new ArgumentException("period cannot be longer than number of values");

            double[] sma = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (i < period)
                {
                    sma[i] = double.NaN;
                    continue;
                }
                else
                {
                    double sum = 0;
                    for (int j = 0; j < period; j++)
                        sum += values[i - j];
                    sma[i] = sum / period;
                    Console.WriteLine(sma[i]);
                }
            }
            return sma;
        }

        public static double[] SMA(OHLC[] ohlcs, int period)
        {
            double[] closingPrices = new double[ohlcs.Length];
            for (int i = 0; i < ohlcs.Length; i++)
                closingPrices[i] = ohlcs[i].close;
            return SMA(closingPrices, period);
        }
    }
}
