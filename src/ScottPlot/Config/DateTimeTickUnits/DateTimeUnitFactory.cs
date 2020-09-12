using System;
using System.Globalization;

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

        public IDateTimeUnit CreateBetterUnit(DateTime from, DateTime to, CultureInfo culture, int maxTickCount)
        {
            double daysApart = to.ToOADate() - from.ToOADate();
            double hoursApart = daysApart * 24;
            double minutesApart = hoursApart * 60;
            double secondsApart = minutesApart * 60;
            double decisecondsApart = secondsApart * 10;
            double centisecondsApart = decisecondsApart * 10;
            double millisecondsApart = centisecondsApart * 10;
            DateTimeUnitKind units;
            if (daysApart > 365 * 1000 * 2)
                units = DateTimeUnitKind.ThousandYear;
            else if (daysApart > 365.0 * 100 * 2)
                units = DateTimeUnitKind.HundredYear;
            else if (daysApart > 365.0 * 10 * 2)
                units = DateTimeUnitKind.TenYear;
            else if (daysApart > 365 * 2)
                units = DateTimeUnitKind.Year;
            else if (daysApart > 30 * 2)
                units = DateTimeUnitKind.Month;
            else if (hoursApart > 24 * 2)
                units = DateTimeUnitKind.Day;
            else if (minutesApart > 60 * 2)
                units = DateTimeUnitKind.Hour;
            else if (secondsApart > 60 * 2)
                units = DateTimeUnitKind.Minute;
            else if (decisecondsApart > 10 * 2)
                units = DateTimeUnitKind.Second;
            else if (centisecondsApart > 10 * 2)
                units = DateTimeUnitKind.Decisecond;
            else if (millisecondsApart > 10 * 2)
                units = DateTimeUnitKind.Centisecond;
            else
                units = DateTimeUnitKind.Millisecond;
            return Create(units, culture, maxTickCount, null);
        }
    }
}
