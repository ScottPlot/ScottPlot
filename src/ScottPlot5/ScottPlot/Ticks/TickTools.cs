using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Ticks
{
    // common methods which may be useful for different tick strategies
    public static class TickTools
    {
        public static double GetIdealTickSpacing(double low, double high, int maxTickCount)
        {
            double range = high - low;
            int exponent = (int)Math.Log10(range);
            List<double> tickSpacings = new List<double>() { Math.Pow(10, exponent) };
            tickSpacings.Add(tickSpacings.Last());
            tickSpacings.Add(tickSpacings.Last());

            int divisions = 0;
            double[] divBy = new double[] { 2, 2, 2.5 }; // dividing from 10 yields 5, 2.5, and 1.

            while (true)
            {
                tickSpacings.Add(tickSpacings.Last() / divBy[divisions++ % divBy.Length]);
                int tickCount = (int)(range / tickSpacings.Last());
                if ((tickCount > maxTickCount) || (tickSpacings.Count > 1000))
                    break;
            }

            return tickSpacings[tickSpacings.Count - 3];
        }
    }
}
