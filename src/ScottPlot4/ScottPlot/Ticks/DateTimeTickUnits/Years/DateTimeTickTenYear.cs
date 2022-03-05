using System;
using System.Globalization;

namespace ScottPlot.Ticks.DateTimeTickUnits
{
    public class DateTimeTickTenYear : DateTimeTickUnitBase
    {
        public DateTimeTickTenYear(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
            kind = DateTimeUnit.TenYear;
            if (manualSpacing == null)
                deltas = new int[] { 1, 2, 5 };
        }

        protected override DateTime Floor(DateTime value)
        {
            return new DateTime(value.Year - (value.Year % 10), 1, 1);
        }

        protected override DateTime Increment(DateTime value, int delta)
        {
            return value.AddYears(delta * 10);
        }

        protected override string GetTickLabel(DateTime value)
        {
            var dt = new DateTime(value.Year, 1, 1);
            string localizedLabel = dt.ToString("yyyy", culture); // year only
            return localizedLabel + "\n ";
        }
    }
}
