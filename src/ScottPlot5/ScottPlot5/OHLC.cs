using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public readonly struct OHLC
    {
        public double Open { get; }
        public double High { get; }
        public double Low { get; }
        public double Close { get; }

        public OHLC(double open, double high, double low, double close)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }
    }
}
