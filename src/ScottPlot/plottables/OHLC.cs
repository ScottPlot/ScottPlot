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
        public double epochSeconds;

        public double highestOpenClose;
        public double lowestOpenClose;
        public bool closedHigher;

        public OHLC(double open, double high, double low, double close, double epochSeconds)
        {
            this.open = open;
            this.high = high;
            this.low = low;
            this.close = close;
            this.epochSeconds = epochSeconds;

            highestOpenClose = Math.Max(open, close);
            lowestOpenClose = Math.Min(open, close);
            closedHigher = (close > open) ? true : false;
        }

        public override string ToString()
        {
            return $"OHLC: open={open}, high={high}, low={low}, close={close}, timestamp={epochSeconds}";
        }
    }
}
