using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ScottPlot.Config.DateTimeTickUnits
{
    public class DateTimeUnitFactory
    {
        public IDateTimeUnit Create(DateTimeUnitKind kind, CultureInfo culture, int maxTickCount, int? manualSpacing)
        {
            switch (kind)
            {
                case DateTimeUnitKind.ThousandYear:
                    return new DateTimeTickThousandYear(culture, maxTickCount, manualSpacing);
                case DateTimeUnitKind.HundredYear:
                    return new DateTimeTickHundredYear(culture, maxTickCount, manualSpacing);
                case DateTimeUnitKind.TenYear:
                    return new DateTimeTickTenYear(culture, maxTickCount, manualSpacing);
                case DateTimeUnitKind.Year:
                    return new DateTimeTickYear(culture, maxTickCount, manualSpacing);
                case DateTimeUnitKind.Month:
                    return new DateTimeTickMonth(culture, maxTickCount, manualSpacing);
                case DateTimeUnitKind.Day:
                    return new DateTimeTickDay(culture, maxTickCount, manualSpacing);
                case DateTimeUnitKind.Hour:
                    return new DateTimeTickHour(culture, maxTickCount, manualSpacing);
                case DateTimeUnitKind.Minute:
                    return new DateTimeTickMinute(culture, maxTickCount, manualSpacing);
                case DateTimeUnitKind.Second:
                    return new DateTimeTickSecond(culture, maxTickCount, manualSpacing);
                case DateTimeUnitKind.Decisecond:
                    return new DateTimeTickDecisecond(culture, maxTickCount, manualSpacing);
                case DateTimeUnitKind.Centisecond:
                    return new DateTimeTickCentisecond(culture, maxTickCount, manualSpacing);
                case DateTimeUnitKind.Millisecond:
                    return new DateTimeTickMillisecond(culture, maxTickCount, manualSpacing);
                default:
                    throw new NotImplementedException("unrecognized TickUnit");
            }
        }

        public IDateTimeUnit CreateBestUnit(DateTime from, DateTime to, CultureInfo culture, int maxTickCount)
        {
            double daysApart = to.ToOADate() - from.ToOADate();

            // tick unit borders in days
            var tickUnitBorders = new List<(DateTimeUnitKind kind, double border)?>
            {
                (DateTimeUnitKind.ThousandYear, 365 * 1_000 * 2),
                (DateTimeUnitKind.HundredYear, 365 * 100 * 2),
                (DateTimeUnitKind.TenYear, 365 * 10 * 2),
                (DateTimeUnitKind.Year, 365 * 2),
                (DateTimeUnitKind.Month, 30 * 2),
                (DateTimeUnitKind.Day, 1 * 2),
                (DateTimeUnitKind.Hour, 1.0 / 24 * 2),
                (DateTimeUnitKind.Minute, 1.0 / 24 / 60 * 2),
                (DateTimeUnitKind.Second, 1.0 / 24 / 3600 * 2),
                (DateTimeUnitKind.Decisecond, 1.0 / 24 / 3600 / 10 * 2),
                (DateTimeUnitKind.Centisecond, 1.0 / 24 / 3600 / 100 * 2),
                (DateTimeUnitKind.Millisecond, 1.0 / 24 / 3600 / 1000 * 2),
            };

            var bestTickUnitKind = tickUnitBorders.FirstOrDefault(tr => daysApart > tr.Value.border);
            bestTickUnitKind = bestTickUnitKind ?? tickUnitBorders.Last(); // last tickUnit if not found best

            return Create(bestTickUnitKind.Value.kind, culture, maxTickCount, null);
        }

        public IDateTimeUnit CreateUnit(DateTime from, DateTime to, CultureInfo culture, int maxTickCount, DateTimeUnitKind? manualUnits, int? manualSpacing)
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
