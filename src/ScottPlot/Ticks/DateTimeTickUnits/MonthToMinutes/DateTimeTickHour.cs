using System;
using System.Globalization;

namespace ScottPlot.Ticks.DateTimeTickUnits
{
    public class DateTimeTickHour : DateTimeTickUnitBase
    {
        public DateTimeTickHour(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
            kind = DateTimeUnit.Hour;
            if (manualSpacing == null)
                deltas = new int[] { 1, 2, 4, 8, 12 };
        }

        protected override DateTime Floor(DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day);
        }

        protected override DateTime Increment(DateTime value, int delta)
        {
            return value.AddHours(delta);
        }

        protected override string GetTickLabel(DateTime value)
        {
            string date = value.ToString("d", culture); // short date
            string time = value.ToString("t", culture); // short time
            return $"{date}\n{time}";
        }
    }
}
