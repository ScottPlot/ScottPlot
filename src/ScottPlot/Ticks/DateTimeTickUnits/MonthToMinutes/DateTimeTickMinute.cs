using System;
using System.Globalization;

namespace ScottPlot.Ticks.DateTimeTickUnits
{
    public class DateTimeTickMinute : DateTimeTickUnitBase
    {
        public DateTimeTickMinute(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
            kind = DateTimeUnit.Minute;
            if (manualSpacing == null)
                deltas = new int[] { 1, 2, 5, 10, 15, 30 };
        }

        protected override DateTime Floor(DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, 0, 0);
        }

        protected override DateTime Increment(DateTime value, int delta)
        {
            return value.AddMinutes(delta);
        }

        protected override string GetTickLabel(DateTime value)
        {
            string date = value.ToString("d", culture); // short date
            string time = value.ToString("t", culture); // short time
            return $"{date}\n{time}";
        }
    }
}
