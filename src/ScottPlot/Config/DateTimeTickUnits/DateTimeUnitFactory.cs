using System;
using System.Globalization;

namespace ScottPlot.Config.DateTimeTickUnits
{
    public class DateTimeUnitFactory
    {
        public IDateTimeUnit Create(DateTimeUnit kind, CultureInfo culture, int maxTickCount, int? manualSpacing)
        {
            switch (kind)
            {
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

        public IDateTimeUnit CreateBetterUnit(DateTime from, DateTime to, CultureInfo culture, int maxTickCount)
        {
            double daysApart = to.ToOADate() - from.ToOADate();
            double hoursApart = daysApart * 24;
            double minutesApart = hoursApart * 60;
            double secondsApart = minutesApart * 60;
            double decisecondsApart = secondsApart * 10;
            double centisecondsApart = decisecondsApart * 10;
            double millisecondsApart = centisecondsApart * 10;
            DateTimeUnit units;
            if (daysApart > 365 * 2)
                units = DateTimeUnit.Year;
            else if (daysApart > 30 * 2)
                units = DateTimeUnit.Month;
            else if (hoursApart > 24 * 2)
                units = DateTimeUnit.Day;
            else if (minutesApart > 60 * 2)
                units = DateTimeUnit.Hour;
            else if (secondsApart > 60 * 2)
                units = DateTimeUnit.Minute;
            else if (decisecondsApart > 10 * 2)
                units = DateTimeUnit.Second;
            else if (centisecondsApart > 10 * 2)
                units = DateTimeUnit.Decisecond;
            else if (millisecondsApart > 10 * 2)
                units = DateTimeUnit.Centisecond;
            else
                units = DateTimeUnit.Millisecond;
            return Create(units, culture, maxTickCount, null);
        }
    }
}
