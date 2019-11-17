using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ScottPlot.Config
{
    public static class DateTimeTicks
    {
        /* 
         * This class calculates ideal tick positions and labels for a given time range.
         * GetTicks() shall be the only public method of this class.
         * 
         */

        public static Tuple<double[], String[]>  GetTicks(DateTime dtLow, DateTime dtHigh, int tickCount = 5)
        {
            // TODO: make this class smarter.

            double low = dtLow.ToOADate();
            double high = dtHigh.ToOADate();
            double span = high - low;
            double tickSpacing = span / tickCount;

            DateTime[] ticks = new DateTime[tickCount];
            for (int i = 0; i < tickCount; i++)
                ticks[i] = DateTime.FromOADate(low + i * tickSpacing);

            // create string labels
            string[] tickLabels = new string[ticks.Length];
            for (int i = 0; i < tickLabels.Length; i++)
            {
                string label = ticks[i].ToString();
                label = label.Replace(" ", "\n");
                label = label.Replace("\nAM", " AM");
                label = label.Replace("\nPM", " PM");
                tickLabels[i] = label;
            }

            return Tuple.Create(GetOADates(ticks), tickLabels);
        }

        private static double[] GetOADates(DateTime[] dateTimeArray)
        {
            double[] positions = new double[dateTimeArray.Length];
            for (int i = 0; i < positions.Length; i++)
                positions[i] = dateTimeArray[i].ToOADate();
            return positions;
        }
    }
}