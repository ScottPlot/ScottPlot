using System.Globalization;
using ScottPlot.TickGenerators.TimeUnits;

namespace ScottPlot.Axis.TimeUnits;

public class Millisecond : ITimeUnit
{
    public IReadOnlyList<int> Divisors => StandardDivisors.Decimal;

    public TimeSpan MinSize => TimeSpan.FromMilliseconds(1);

    public string GetDateTimeFormatString()
    {
        string hourSpecifier = CultureInfo.CurrentCulture.Uses24HourClock() ? "HH" : "hh";
        return $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}\n{hourSpecifier}:mm:ss.fff"; // TODO: This assumes colons as the separators, but consider (some) French-language locales use 12h30 rather than 12:30
    }

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        return dateTime.AddMilliseconds(increment);
    }
}
