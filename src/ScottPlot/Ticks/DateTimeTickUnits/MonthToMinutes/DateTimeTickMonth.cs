using System;
using System.Globalization;

namespace ScottPlot.Ticks.DateTimeTickUnits
{
    public class DateTimeTickMonth : DateTimeTickUnitBase
    {
        public DateTimeTickMonth(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
            kind = DateTimeUnit.Month;
            if (manualSpacing == null)
                deltas = new int[] { 1, 2, 3, 6 };
        }

        protected override DateTime Floor(DateTime value)
        {
            return new DateTime(value.Year, 1, 1);
        }

        protected override DateTime Increment(DateTime value, int delta)
        {
            return value.AddMonths(delta);
        }

        protected override string GetTickLabel(DateTime value)
        {
            var dt = new DateTime(value.Year, value.Month, 1);
            string localizedLabel = dt.ToString("Y", culture); // year and month pattern
            // replace only first space " "
            int pos = localizedLabel.IndexOf(" ");
            if (pos < 0)
                return localizedLabel + "\n";
            else
                return localizedLabel.Substring(0, pos) + "\n" + localizedLabel.Substring(pos + 1, localizedLabel.Length - pos - 1);
        }
    }
}
