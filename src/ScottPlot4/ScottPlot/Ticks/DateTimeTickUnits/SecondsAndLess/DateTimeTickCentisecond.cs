using System;
using System.Globalization;

namespace ScottPlot.Ticks.DateTimeTickUnits
{
    public class DateTimeTickCentisecond : DateTimeTickUnitBase
    {
        public DateTimeTickCentisecond(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
            kind = DateTimeUnit.Centisecond;
            if (manualSpacing == null)
                deltas = new int[] { 1, 2, 5 };
        }

        protected override DateTime Floor(DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
        }

        protected override DateTime Increment(DateTime value, int delta)
        {
            return value.AddMilliseconds(delta * 10);
        }

        protected override string GetTickLabel(DateTime value)
        {
            string date = value.ToString("d", culture); // short date

            string hourSpecifier = Tools.Uses24HourClock(culture) ? "HH" : "hh";
            string time = value.ToString($"{hourSpecifier}:mm:ss.ff", culture); // long time
            return $"{date}\n{time}";
        }
    }
}
