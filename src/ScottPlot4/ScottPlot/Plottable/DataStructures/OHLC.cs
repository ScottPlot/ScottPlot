using System;

namespace ScottPlot
{
    /// <summary>
    /// This class holds open/high/low/close (OHLC) price data over a time range.
    /// </summary>
    public class OHLC : IOHLC
    {
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public DateTime DateTime { get; set; }
        public TimeSpan TimeSpan { get; set; }

        [Obsolete("The `Volume` property of OHLCs has been deprecated.")]
        public double Volume { get; set; }

        public override string ToString() =>
            $"OHLC: open={Open}, high={High}, low={Low}, close={Close}, start={DateTime}, span={TimeSpan}";

        /// <summary>
        /// OHLC price over a specific period of time
        /// </summary>
        /// <param name="open">opening price</param>
        /// <param name="high">maximum price</param>
        /// <param name="low">minimum price</param>
        /// <param name="close">closing price</param>
        /// <param name="timeStart">open time</param>
        /// <param name="timeSpan">width of the OHLC</param>
        public OHLC(double open, double high, double low, double close, DateTime timeStart, TimeSpan timeSpan)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            DateTime = timeStart;
            TimeSpan = timeSpan;
        }

        /// <summary>
        /// OHLC price over a specific period of time using the OADate format
        /// </summary>
        /// <param name="open">opening price</param>
        /// <param name="high">maximum price</param>
        /// <param name="low">minimum price</param>
        /// <param name="close">closing price</param>
        /// <param name="oaDate">start time (days since 1900)</param>
        /// <param name="oaDateSpan">width of the OHLC (days)</param>
        public OHLC(double open, double high, double low, double close, double oaDate, double oaDateSpan = 1)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            DateTime = DateTime.FromOADate(oaDate);
            TimeSpan = TimeSpan.FromDays(oaDateSpan);
        }

        [Obsolete("This constructor has been deprecated.")]
        public OHLC(double open, double high, double low, double close, double timeStart, double timeSpan = 1, double volume = 0)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            DateTime = DateTime.FromOADate(timeStart);
            TimeSpan = TimeSpan.FromDays(timeSpan);
            Volume = volume;
        }
    }
}
