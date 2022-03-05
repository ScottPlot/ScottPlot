using System;
using System.Globalization;

namespace ScottPlot.Ticks.DateTimeTickUnits
{
    public class DateTimeTickDay : DateTimeTickUnitBase
    {
        public DateTimeTickDay(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
            kind = DateTimeUnit.Day;
            if (manualSpacing == null)
                deltas = new int[] { 1, 2, 5, 10, 20 };
        }

        protected override DateTime Floor(DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        protected override DateTime Increment(DateTime value, int delta)
        {
            return value.AddDays(delta);
        }

        protected override string GetTickLabel(DateTime value)
        {
            var dt = new DateTime(value.Year, value.Month, value.Day);
            string localizedLabel = dt.ToString("d", culture); // short date pattern
            return localizedLabel.Replace("T", "\n") + "\n ";
        }
    }
}
