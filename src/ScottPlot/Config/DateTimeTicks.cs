using ScottPlot.Config.DateTimeTickUnits;
using System;
using System.Globalization;

namespace ScottPlot.Config
{
    public static class DateTimeTicks
    {
        /* 
         * This class calculates ideal tick positions and labels for a given time range.
         * This class can be modified to improve how date ticks are calculated and displayed.
         * GetTicks() shall be the only public method of this class.
         * 
         */

        public static (DateTime[], String[]) GetTicks(DateTime dt1, DateTime dt2, int maxTickCount, CultureInfo culture, DateTimeUnit? dtManualUnits, int dtManualSpacing)
        {
            // prevent crashes if equal or inverted date ranges
            if (!(dt1 < dt2))
                dt2 = dt1.AddSeconds(1);

            var unitFactory = new DateTimeUnitFactory();
            IDateTimeUnit tickUnit;
            if (dtManualUnits.HasValue)
            {
                tickUnit = unitFactory.Create(dtManualUnits.Value, culture, maxTickCount, dtManualSpacing);
            }
            else
            {
                tickUnit = unitFactory.CreateBetterUnit(dt1, dt2, culture, maxTickCount);
            }
            return tickUnit.GetTicksAndLabels(dt1, dt2);
        }
    }
}
