using System;
using System.Collections.Generic;
using System.Globalization;

namespace ScottPlot.Config.DateTimeTickUnits
{
    public class DateTimeTickYear : DateTimeTickUnitBase
    {
        public DateTimeTickYear(CultureInfo culture, int maxTickCount, int? manualSpacing) : base(culture, maxTickCount, manualSpacing)
        {
            kind = DateTimeUnitKind.Year;
            if (manualSpacing != null)
                throw new NotImplementedException("can't display years with fixed spacing (use numeric axis instead)");

        }

        // don't need to override Increment and Floor, because we override whole GetTicks

        protected override string GetTickLabel(DateTime value)
        {
            var dt = new DateTime(value.Year, 1, 1);
            string localizedLabel = dt.ToString("yyyy", culture); // year only
            return localizedLabel;
        }

        protected override DateTime[] GetTicks(DateTime from, DateTime to, int delta)
        {
            // determine ideal tick spacing (multiples of 1, 5, and 10)
            int span = to.Year - from.Year;
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
            int firstYear = from.Year - (to.Year % tickSpacing);

            // create a list of dates (only the valid ones)
            var dates = new List<DateTime>();
            DateTime dt = new DateTime(firstYear, 1, 1);
            while (dt <= to)
            {
                if (dt >= from)
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
    }
}
