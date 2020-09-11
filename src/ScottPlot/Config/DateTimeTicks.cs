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

            DateTimeUnit units;
            if (dtManualUnits.HasValue)
            {
                units = dtManualUnits.Value;
            }
            else
            {
                // determine the best DateTime unit to use for the given DateTime range
                double daysApart = dt2.ToOADate() - dt1.ToOADate();
                double hoursApart = daysApart * 24;
                double minutesApart = hoursApart * 60;
                double secondsApart = minutesApart * 60;
                double decisecondsApart = secondsApart * 10;
                double centisecondsApart = decisecondsApart * 10;
                double millisecondsApart = centisecondsApart * 10;
                if (daysApart > 365 * 2)
                    units = DateTimeUnit.Year;
                else if (daysApart > 30 * 2)
                    units = DateTimeUnit.Month;
                else if (hoursApart > 24 * 2)
                    units = DateTimeUnit.Day;
                else if (minutesApart > 60 * 2)
                    units = DateTimeUnit.Hour;
                else if (secondsApart > 60 * 2)
                    units = DateTimeUnit.Minute;
                else if (decisecondsApart > 10 * 2)
                    units = DateTimeUnit.Second;
                else if (centisecondsApart > 10 * 2)
                    units = DateTimeUnit.Decisecond;
                else if (millisecondsApart > 10 * 2)
                    units = DateTimeUnit.Centisecond;
                else
                    units = DateTimeUnit.Millisecond;
            }
            var TickUnit = new DateTimeUnitFactory().Create(units, culture, maxTickCount, dtManualUnits == null ? null : (int?)dtManualSpacing);
            return TickUnit.GetTicksAndLabels(dt1, dt2);
        }
    }
}
