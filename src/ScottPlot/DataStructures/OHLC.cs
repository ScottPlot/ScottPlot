using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public class OHLC
    {
        public double open;
        public double high;
        public double low;
        public double close;
        public double time;
        public double timeSpan = 1;

        public double highestOpenClose;
        public double lowestOpenClose;
        public bool closedHigher;

        private bool IsNanOrInf(double val) => double.IsInfinity(val) || double.IsNaN(val);

        public bool IsValid => !IsNanOrInf(open) && !IsNanOrInf(close) &&
                               !IsNanOrInf(low) && !IsNanOrInf(high) &&
                               !IsNanOrInf(time) && !IsNanOrInf(timeSpan) &&
                               !IsNanOrInf(highestOpenClose) && !IsNanOrInf(lowestOpenClose);

        public OHLC(double open, double high, double low, double close, DateTime dateTime, double timeSpan = 1)
        {
            this.open = open;
            this.high = high;
            this.low = low;
            this.close = close;
            time = dateTime.ToOADate();
            this.timeSpan = timeSpan;

            highestOpenClose = Math.Max(open, close);
            lowestOpenClose = Math.Min(open, close);
            closedHigher = (close > open);
        }

        public OHLC(double open, double high, double low, double close, double time, double timeSpan = 1)
        {
            this.open = open;
            this.high = high;
            this.low = low;
            this.close = close;
            this.time = time;
            this.timeSpan = timeSpan;

            highestOpenClose = Math.Max(open, close);
            lowestOpenClose = Math.Min(open, close);
            closedHigher = (close > open);
        }

        public override string ToString()
        {
            return $"OHLC: open={open}, high={high}, low={low}, close={close}, timestamp={time}";
        }
    }
}
