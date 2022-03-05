using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ScottPlot.Ticks.DateTimeTickUnits
{
    public class DateTimeUnitFactory
    {
        public IDateTimeUnit Create(DateTimeUnit kind, CultureInfo culture, int maxTickCount, int? manualSpacing)
        {
            switch (kind)
            {
                case DateTimeUnit.ThousandYear:
                    return new DateTimeTickThousandYear(culture, maxTickCount, manualSpacing);
                case DateTimeUnit.HundredYear:
                    return new DateTimeTickHundredYear(culture, maxTickCount, manualSpacing);
                case DateTimeUnit.TenYear:
                    return new DateTimeTickTenYear(culture, maxTickCount, manualSpacing);
                case DateTimeUnit.Year:
                    return new DateTimeTickYear(culture, maxTickCount, manualSpacing);
                case DateTimeUnit.Month:
                    return new DateTimeTickMonth(culture, maxTickCount, manualSpacing);
                case DateTimeUnit.Day:
                    return new DateTimeTickDay(culture, maxTickCount, manualSpacing);
                case DateTimeUnit.Hour:
                    return new DateTimeTickHour(culture, maxTickCount, manualSpacing);
                case DateTimeUnit.Minute:
                    return new DateTimeTickMinute(culture, maxTickCount, manualSpacing);
                case DateTimeUnit.Second:
                    return new DateTimeTickSecond(culture, maxTickCount, manualSpacing);
                case DateTimeUnit.Decisecond:
                    return new DateTimeTickDecisecond(culture, maxTickCount, manualSpacing);
                case DateTimeUnit.Centisecond:
                    return new DateTimeTickCentisecond(culture, maxTickCount, manualSpacing);
                case DateTimeUnit.Millisecond:
                    return new DateTimeTickMillisecond(culture, maxTickCount, manualSpacing);
                default:
                    throw new NotImplementedException("unrecognized TickUnit");
            }
        }

        public IDateTimeUnit CreateBestUnit(DateTime from, DateTime to, CultureInfo culture, int maxTickCount)
        {
            double daysApart = to.ToOADate() - from.ToOADate();

            int halfDensity = maxTickCount / 2;

            // tick unit borders in days
            var tickUnitBorders = new List<(DateTimeUnit kind, double border)?>
            {
                (DateTimeUnit.ThousandYear, 365 * 1_000 * halfDensity),
                (DateTimeUnit.HundredYear, 365 * 100 * halfDensity),
                (DateTimeUnit.TenYear, 365 * 10 * halfDensity),
                (DateTimeUnit.Year, 365 * halfDensity),
                (DateTimeUnit.Month, 30 * halfDensity),
                (DateTimeUnit.Day, 1 * halfDensity),
                (DateTimeUnit.Hour, 1.0 / 24 * halfDensity),
                (DateTimeUnit.Minute, 1.0 / 24 / 60 * halfDensity),
                (DateTimeUnit.Second, 1.0 / 24 / 3600 * halfDensity),
                (DateTimeUnit.Decisecond, 1.0 / 24 / 3600 / 10 * halfDensity),
                (DateTimeUnit.Centisecond, 1.0 / 24 / 3600 / 100 * halfDensity),
                (DateTimeUnit.Millisecond, 1.0 / 24 / 3600 / 1000 * halfDensity),
            };

            var bestTickUnitKind = tickUnitBorders.FirstOrDefault(tr => daysApart > tr.Value.border);
            bestTickUnitKind = bestTickUnitKind ?? tickUnitBorders.Last(); // last tickUnit if not found best

            return Create(bestTickUnitKind.Value.kind, culture, maxTickCount, null);
        }

        public IDateTimeUnit CreateUnit(DateTime from, DateTime to, CultureInfo culture, int maxTickCount, DateTimeUnit? manualUnits, int? manualSpacing)
        {
            if (manualUnits == null)
            {
                return CreateBestUnit(from, to, culture, maxTickCount);
            }
            else
            {
                return Create(manualUnits.Value, culture, maxTickCount, manualSpacing);
            }
        }
    }
}
