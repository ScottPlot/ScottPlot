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

        public double highestOpenClose;
        public double lowestOpenClose;
        public bool closedHigher;

        public OHLC(double open, double high, double low, double close, DateTime dateTime)
        {
            this.open = open;
            this.high = high;
            this.low = low;
            this.close = close;
            time = dateTime.ToOADate();

            highestOpenClose = Math.Max(open, close);
            lowestOpenClose = Math.Min(open, close);
            closedHigher = (close > open) ? true : false;
        }

        public OHLC(double open, double high, double low, double close, double time)
        {
            this.open = open;
            this.high = high;
            this.low = low;
            this.close = close;
            this.time = time;

            highestOpenClose = Math.Max(open, close);
            lowestOpenClose = Math.Min(open, close);
            closedHigher = (close > open) ? true : false;
        }

        public override string ToString()
        {
            return $"OHLC: open={open}, high={high}, low={low}, close={close}, timestamp={time}";
        }
    }
}
