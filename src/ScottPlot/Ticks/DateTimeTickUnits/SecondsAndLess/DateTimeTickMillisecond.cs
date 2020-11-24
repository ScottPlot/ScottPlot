﻿using System;
using System.Globalization;

namespace ScottPlot.Ticks.DateTimeTickUnits
{
    public class DateTimeTickMillisecond : DateTimeTickUnitBase
    {
        public DateTimeTickMillisecond(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
            kind = DateTimeUnit.Millisecond;
            if (manualSpacing == null)
                deltas = new int[] { 1, 2, 5 };
        }

        protected override DateTime Floor(DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
        }

        protected override DateTime Increment(DateTime value, int delta)
        {
            return value.AddMilliseconds(delta);
        }

        protected override string GetTickLabel(DateTime value)
        {
            string date = value.ToString("d", culture); // short date
            string time = value.ToString("hh:mm:ss.fff", culture); // long time
            return $"{date}\n{time}";
        }
    }
}
