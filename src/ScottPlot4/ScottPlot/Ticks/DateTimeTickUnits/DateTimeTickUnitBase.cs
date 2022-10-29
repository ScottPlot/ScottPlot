using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ScottPlot.Ticks.DateTimeTickUnits
{
    public class DateTimeTickUnitBase : IDateTimeUnit
    {
        // base class implements Seconds Unit
        protected DateTimeUnit kind = DateTimeUnit.Second;
        protected CultureInfo culture;
        protected int[] deltas = new int[] { 1, 2, 5, 10, 15, 30 };
        protected int maxTickCount;

        public DateTimeTickUnitBase(CultureInfo culture, int maxTickCount, int? manualSpacing)
        {
            this.culture = culture ?? CultureInfo.CurrentCulture;
            this.maxTickCount = maxTickCount;
            if (manualSpacing.HasValue)
                deltas = new int[] { manualSpacing.Value };
        }

        protected virtual DateTime Floor(DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0);
        }

        protected virtual DateTime Increment(DateTime value, int delta)
        {
            return value.AddSeconds(delta);
        }

        protected virtual string GetTickLabel(DateTime value)
        {
            string date = value.ToString("d", culture); // short date
            string time = value.ToString("T", culture); // long time
            return $"{date}\n{time}";
        }

        public (double[] Ticks, string[] Labels) GetTicksAndLabels(DateTime from, DateTime to, string format)
        {
            DateTime[] ticks = GetTicks(from, to, deltas, maxTickCount);
            string[] labels = (format is null) ?
                ticks.Select(t => GetTickLabel(t)).ToArray() :
                ticks.Select(t => t.ToString(format, culture)).ToArray();
            return (ticks.Select(t => t.ToOADate()).ToArray(), labels);
        }

        protected DateTime[] GetTicks(DateTime from, DateTime to, int[] deltas, int maxTickCount)
        {
            DateTime[] result = new DateTime[] { };
            foreach (var delta in deltas)
            {
                result = GetTicks(from, to, delta);
                if (result.Length <= maxTickCount)
                    return result;
            }
            return result;
        }

        protected virtual DateTime[] GetTicks(DateTime from, DateTime to, int delta)
        {
            var dates = new List<DateTime>();
            DateTime dt = Floor(from);
            while (dt <= to)
            {
                if (dt >= from)
                    dates.Add(dt);
                try
                {
                    dt = Increment(dt, delta);
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
