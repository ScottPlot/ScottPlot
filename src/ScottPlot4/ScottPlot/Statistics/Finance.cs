using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Statistics
{
    public static class Finance
    {
        /// <summary>
        /// Simple moving average
        /// </summary>
        /// <param name="values">number of values to use for each calculation</param>
        /// <param name="period">number of values to use for each calculation</param>
        /// <param name="trimNan">only return data where values are present for the whole period</param>
        public static double[] SMA(double[] values, int period, bool trimNan = true)
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
                }
                else
                {
                    // TODO: could optimize this for perforance by not copying
                    // to do this create a Common.Mean overload
                    var periodValues = new double[period];
                    Array.Copy(values, i - period + 1, periodValues, 0, period);
                    sma[i] = Common.Mean(periodValues);
                }
            }

            return trimNan ? sma.Skip(period).ToArray() : sma;
        }

        /// <summary>
        /// Simple moving standard deviation
        /// </summary>
        /// <param name="values"></param>
        /// <param name="period">number of values to use for each calculation</param>
        public static double[] SMStDev(double[] values, int period)
        {
            if (period < 2)
                throw new ArgumentException("period must be 2 or greater");

            if (period > values.Length)
                throw new ArgumentException("period cannot be longer than number of values");

            double[] stDev = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                if (i < period)
                {
                    stDev[i] = double.NaN;
                    continue;
                }
                else
                {
                    var periodValues = new double[period];
                    Array.Copy(values, i - period + 1, periodValues, 0, period);
                    stDev[i] = Common.StDev(periodValues);
                }
            }
            return stDev;
        }

        /// <summary>
        /// Return the simple moving average (SMA) of the OHLC closing prices.
        /// The returned data will be shorter than the input data by N points.
        /// </summary>
        /// <param name="ohlcs">price data to analyze</param>
        /// <param name="N">each returned price represents the average of N prices</param>
        public static double[] SMA(IOHLC[] ohlcs, int N)
        {
            double[] closingPrices = new double[ohlcs.Length];
            for (int i = 0; i < ohlcs.Length; i++)
                closingPrices[i] = ohlcs[i].Close;
            return SMA(closingPrices, N);
        }

        /// <summary>
        /// Return the SMA and upper/lower Bollinger bands for the given price data.
        /// The returned data will NOT be shorter than the input data. It will contain NaN values at the front.
        /// </summary>
        /// <param name="prices">price data to use for analysis</param>
        /// <param name="N">each returned price represents the average of N prices</param>
        /// <param name="sdCoeff">number of standard deviations from the mean to use for the Bollinger bands</param>
        public static (double[] sma, double[] lower, double[] upper) Bollinger(double[] prices, int N, double sdCoeff = 2)
        {
            double[] sma = SMA(prices, N, trimNan: false);
            double[] smstd = SMStDev(prices, N);

            double[] bolU = new double[prices.Length];
            double[] bolL = new double[prices.Length];
            for (int i = 0; i < prices.Length; i++)
            {
                bolL[i] = sma[i] - sdCoeff * smstd[i];
                bolU[i] = sma[i] + sdCoeff * smstd[i];
            }

            return (sma, bolL, bolU);
        }

        /// <summary>
        /// Return the SMA and upper/lower Bollinger bands for the closing price of the given OHLCs.
        /// The returned data will be shorter than the input data by N values.
        /// </summary>
        /// <param name="ohlcs">price data to use for analysis</param>
        /// <param name="N">each returned price represents the average of N prices</param>
        /// <param name="sdCoeff">number of standard deviations from the mean to use for the Bollinger bands</param>
        public static (double[] sma, double[] lower, double[] upper) Bollinger(IOHLC[] ohlcs, int N, double sdCoeff = 2)
        {
            double[] closingPrices = new double[ohlcs.Length];
            for (int i = 0; i < ohlcs.Length; i++)
                closingPrices[i] = ohlcs[i].Close;
            var (sma, lower, upper) = Bollinger(closingPrices, N, sdCoeff);

            // skip the first points which all contain NaN
            sma = sma.Skip(N).ToArray();
            lower = lower.Skip(N).ToArray();
            upper = upper.Skip(N).ToArray();

            return (sma, lower, upper);
        }
    }
}
