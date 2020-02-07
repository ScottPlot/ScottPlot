using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

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

        private static readonly string[] monthShort = { null,
            "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        private enum TickUnits { years, months, days, hours, minutes, seconds };

        public static (DateTime[], String[]) GetTicks(DateTime dt1, DateTime dt2, int maxTickCount, CultureInfo culture)
        {
            if (!(dt1 < dt2))
                dt2 = dt1.AddSeconds(1);

            // determine the best time units to use for tick marks
            TickUnits units;
            double daysApart = dt2.ToOADate() - dt1.ToOADate();
            double hoursApart = daysApart * 24;
            double minutesApart = hoursApart * 60;
            double secondsApart = minutesApart * 60;
            if (daysApart > 365 * 2)
                units = TickUnits.years;
            else if (daysApart > 30 * 2)
                units = TickUnits.months;
            else if (hoursApart > 24 * 2)
                units = TickUnits.days;
            else if (minutesApart > 60 * 2)
                units = TickUnits.hours;
            else if (secondsApart > 60 * 2)
                units = TickUnits.minutes;
            else
                units = TickUnits.seconds;

            // create arrays of DateTimes with spacings customized for each tick unit
            if (units == TickUnits.years)
            {
                DateTime[] ticks = GetYearTicks(dt1, dt2, maxTickCount);

                string[] labels = new string[ticks.Length];
                for (int i = 0; i < labels.Length; i++)
                {
                    var dt = new DateTime(ticks[i].Year, 1, 1);
                    string localizedLabel = dt.ToString("yyyy", culture); // year only
                    labels[i] = localizedLabel;
                }

                return (ticks, labels);
            }
            else if (units == TickUnits.months)
            {
                DateTime[] ticks = GetMonthTicks(dt1, dt2, 1);
                if (ticks.Length > maxTickCount)
                    ticks = GetMonthTicks(dt1, dt2, 2);
                if (ticks.Length > maxTickCount)
                    ticks = GetMonthTicks(dt1, dt2, 3);
                if (ticks.Length > maxTickCount)
                    ticks = GetMonthTicks(dt1, dt2, 6);

                string[] labels = new string[ticks.Length];
                for (int i = 0; i < labels.Length; i++)
                {
                    var dt = new DateTime(ticks[i].Year, ticks[i].Month, 1);
                    string localizedLabel = dt.ToString("Y", culture); // year and month pattern
                    labels[i] = localizedLabel.Replace(" ", "\n");
                }

                return (ticks, labels);
            }
            else if (units == TickUnits.days)
            {
                DateTime[] ticks = GetDayTicks(dt1, dt2, 1);
                if (ticks.Length > maxTickCount)
                    ticks = GetDayTicks(dt1, dt2, 2);
                if (ticks.Length > maxTickCount)
                    ticks = GetDayTicks(dt1, dt2, 5);
                if (ticks.Length > maxTickCount)
                    ticks = GetDayTicks(dt1, dt2, 10);

                string[] labels = new string[ticks.Length];
                for (int i = 0; i < labels.Length; i++)
                {
                    var dt = new DateTime(ticks[i].Year, ticks[i].Month, ticks[i].Day);
                    string localizedLabel = dt.ToString("d", culture); // short date pattern
                    labels[i] = localizedLabel.Replace("T", "\n");
                }

                return (ticks, labels);
            }
            else if (units == TickUnits.hours)
            {
                DateTime[] ticks = GetHourTicks(dt1, dt2, 1);
                if (ticks.Length > maxTickCount)
                    ticks = GetHourTicks(dt1, dt2, 2);
                if (ticks.Length > maxTickCount)
                    ticks = GetHourTicks(dt1, dt2, 4);
                if (ticks.Length > maxTickCount)
                    ticks = GetHourTicks(dt1, dt2, 8);
                if (ticks.Length > maxTickCount)
                    ticks = GetHourTicks(dt1, dt2, 12);

                string[] labels = new string[ticks.Length];
                for (int i = 0; i < labels.Length; i++)
                {
                    string date = ticks[i].ToString("d", culture); // short date
                    string time = ticks[i].ToString("t", culture); // short time
                    labels[i] = $"{date}\n{time}";
                }

                return (ticks, labels);
            }
            else if (units == TickUnits.minutes)
            {
                DateTime[] ticks = GetMinuteTicks(dt1, dt2, 1);
                if (ticks.Length > maxTickCount)
                    ticks = GetMinuteTicks(dt1, dt2, 2);
                if (ticks.Length > maxTickCount)
                    ticks = GetMinuteTicks(dt1, dt2, 5);
                if (ticks.Length > maxTickCount)
                    ticks = GetMinuteTicks(dt1, dt2, 10);
                if (ticks.Length > maxTickCount)
                    ticks = GetMinuteTicks(dt1, dt2, 15);
                if (ticks.Length > maxTickCount)
                    ticks = GetMinuteTicks(dt1, dt2, 30);

                string[] labels = new string[ticks.Length];
                for (int i = 0; i < labels.Length; i++)
                {
                    string date = ticks[i].ToString("d", culture); // short date
                    string time = ticks[i].ToString("t", culture); // short time
                    labels[i] = $"{date}, {time}";
                }

                return (ticks, labels);
            }
            else if (units == TickUnits.seconds)
            {
                DateTime[] ticks = GetSecondTicks(dt1, dt2, 1);
                if (ticks.Length > maxTickCount)
                    ticks = GetSecondTicks(dt1, dt2, 2);
                if (ticks.Length > maxTickCount)
                    ticks = GetSecondTicks(dt1, dt2, 5);
                if (ticks.Length > maxTickCount)
                    ticks = GetSecondTicks(dt1, dt2, 10);
                if (ticks.Length > maxTickCount)
                    ticks = GetSecondTicks(dt1, dt2, 15);
                if (ticks.Length > maxTickCount)
                    ticks = GetSecondTicks(dt1, dt2, 30);

                string[] labels = new string[ticks.Length];
                for (int i = 0; i < labels.Length; i++)
                {
                    string date = ticks[i].ToString("d", culture); // short date
                    string time = ticks[i].ToString("T", culture); // long time
                    labels[i] = $"{date}, {time}";
                }

                return (ticks, labels);
            }
            else
            {
                throw new NotImplementedException("unrecognized TickUnit");
            }
        }

        private static DateTime[] GetYearTicks(DateTime dt1, DateTime dt2, int maxTickCount)
        {
            // determine ideal tick spacing (multiples of 1, 5, and 10)
            int span = dt2.Year - dt1.Year;
            int[] interval = { 2, 5 };
            int tickSpacing = 1000;
            for (int i = 0; i < 100; i++)
            {
                int divisor = interval[i % interval.Length];
                if (tickSpacing > 1)
                {
                    tickSpacing /= divisor;
                    double tickCountNow = span / tickSpacing;
                    if (tickCountNow > maxTickCount)
                    {
                        tickSpacing *= divisor;
                        break;
                    }
                }
                else
                {
                    tickSpacing = 1;
                    break;
                }
            }

            // offset the first year to make it a multiple of the tick spacing
            int firstYear = dt1.Year - (dt1.Year % tickSpacing);

            // create a list of dates (only the valid ones)
            var dates = new List<DateTime>();
            DateTime dt = new DateTime(firstYear, 1, 1);
            while (dt <= dt2)
            {
                if (dt >= dt1)
                    dates.Add(dt);
                try
                {
                    dt = dt.AddYears((int)tickSpacing);
                }
                catch
                {
                    break; // our date is larger than possible
                }
            }
            return dates.ToArray();

        }

        private static DateTime[] GetMonthTicks(DateTime dt1, DateTime dt2, int delta)
        {
            var dates = new List<DateTime>();
            DateTime dt = new DateTime(dt1.Year, 1, 1);
            while (dt <= dt2)
            {
                if (dt >= dt1)
                    dates.Add(dt);
                dt = dt.AddMonths(delta);
            }
            return dates.ToArray();
        }

        private static DateTime[] GetDayTicks(DateTime dt1, DateTime dt2, int delta)
        {
            var dates = new List<DateTime>();
            DateTime dt = new DateTime(dt1.Year, dt1.Month, 1);
            while (dt <= dt2)
            {
                if (dt >= dt1)
                    dates.Add(dt);
                dt = dt.AddDays(delta);
            }
            return dates.ToArray();
        }

        private static DateTime[] GetHourTicks(DateTime dt1, DateTime dt2, int delta)
        {
            var dates = new List<DateTime>();
            DateTime dt = new DateTime(dt1.Year, dt1.Month, dt1.Day);
            while (dt <= dt2)
            {
                if (dt >= dt1)
                    dates.Add(dt);
                dt = dt.AddHours(delta);
            }
            return dates.ToArray();
        }

        private static DateTime[] GetMinuteTicks(DateTime dt1, DateTime dt2, int delta)
        {
            var dates = new List<DateTime>();
            DateTime dt = new DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, 0, 0);
            while (dt <= dt2)
            {
                if (dt >= dt1)
                    dates.Add(dt);
                dt = dt.AddMinutes(delta);
            }
            return dates.ToArray();
        }

        private static DateTime[] GetSecondTicks(DateTime dt1, DateTime dt2, int delta)
        {
            var dates = new List<DateTime>();
            DateTime dt = new DateTime(dt1.Year, dt1.Month, dt1.Day, dt1.Hour, dt1.Minute, 0);
            while (dt <= dt2)
            {
                if (dt >= dt1)
                    dates.Add(dt);
                dt = dt.AddSeconds(delta);
            }
            return dates.ToArray();
        }

    }
}