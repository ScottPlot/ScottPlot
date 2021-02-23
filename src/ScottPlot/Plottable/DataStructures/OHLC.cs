using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    /// <summary>
    /// This class holds open/high/low/close (OHLC) price data over a time range.
    /// </summary>
    public class OHLC : IComparable
    {
        public double Open;
        public double High;
        public double Low;
        public double Close;
        public DateTime DateTime;
        public TimeSpan TimeSpan;

        private bool IsNanOrInfinity(double val) => double.IsInfinity(val) || double.IsNaN(val);

        public bool IsValid
        {
            get
            {
                if (IsNanOrInfinity(Open)) return false;
                if (IsNanOrInfinity(High)) return false;
                if (IsNanOrInfinity(Low)) return false;
                if (IsNanOrInfinity(Close)) return false;
                return true;
            }
        }

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
        /// OHLC price over a specific period of time
        /// </summary>
        /// <param name="open">opening price</param>
        /// <param name="high">maximum price</param>
        /// <param name="low">minimum price</param>
        /// <param name="close">closing price</param>
        /// <param name="timeStart">open time (DateTime.ToOADate() units)</param>
        /// <param name="timeSpan">width of the OHLC in days</param>
        public OHLC(double open, double high, double low, double close, double timeStart, double timeSpan = 1)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
            DateTime = DateTime.FromOADate(timeStart);
            TimeSpan = TimeSpan.FromDays(timeSpan);
        }

        public int CompareTo(object obj)
        {
            if (obj is OHLC other)
            {
                if (this.DateTime < other.DateTime)
                    return 1;
                else if (this.DateTime > other.DateTime)
                    return -1;
                else
                    return 0;
            }
            else
            {
                throw new InvalidOperationException("OHLCs may only be compared to other OHLCs");
            }
        }
    }
}
