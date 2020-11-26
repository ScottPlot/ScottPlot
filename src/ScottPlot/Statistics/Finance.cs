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
        /// <param name="period">number of values to use for each calculation</param>
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
                    Array.Copy(values, i - period, periodValues, 0, period);
                    sma[i] = Common.Mean(periodValues);
                }
            }

            return trimNan ? sma.Skip(period).ToArray() : sma;
        }

        /// <summary>
        /// Simple moving standard deviation
        /// </summary>
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
                    Array.Copy(values, i - period, periodValues, 0, period);
                    stDev[i] = Common.StDev(periodValues);
                }
            }
            return stDev;
        }

        /// <summary>
        /// Simple moving average
        /// </summary>
        /// <param name="period">number of OHLCs to use for each calculation</param>
        public static double[] SMA(OHLC[] ohlcs, int period)
        {
            double[] closingPrices = new double[ohlcs.Length];
            for (int i = 0; i < ohlcs.Length; i++)
                closingPrices[i] = ohlcs[i].close;
            return SMA(closingPrices, period);
        }

        /// <summary>
        /// Bollinger Bands
        /// </summary>
        /// <param name="period">number of OHLCs to use for each calculation</param>
        /// <param name="multiplier">number of standard deviations from the mean</param>
        public static (double[] sma, double[] lower, double[] upper) Bollinger(double[] values, int period, double multiplier = 2)
        {
            double[] sma = SMA(values, period, trimNan: false);
            double[] smstd = SMStDev(values, period);

            double[] bolU = new double[values.Length];
            double[] bolL = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                bolL[i] = sma[i] - multiplier * smstd[i];
                bolU[i] = sma[i] + multiplier * smstd[i];
            }

            return (sma, bolL, bolU);
        }

        /// <summary>
        /// Bollinger Bands
        /// </summary>
        /// <param name="period">number of OHLCs to use for each calculation</param>
        /// <param name="multiplier">number of standard deviations from the mean</param>
        public static (double[] sma, double[] lower, double[] upper) Bollinger(OHLC[] ohlcs, int period, double multiplier = 2)
        {
            double[] closingPrices = new double[ohlcs.Length];
            for (int i = 0; i < ohlcs.Length; i++)
                closingPrices[i] = ohlcs[i].close;
            var (sma, lower, upper) = Bollinger(closingPrices, period, multiplier);

            // skip the first points which all contain NaN
            sma = sma.Skip(period).ToArray();
            lower = lower.Skip(period).ToArray();
            upper = upper.Skip(period).ToArray();

            return (sma, lower, upper);
        }
    }
}
