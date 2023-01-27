using System.Globalization;

namespace ScottPlot.Axis.TimeUnits;

public class Centisecond : ITimeUnit
{
    public IReadOnlyList<int> NiceIncrements => NiceNumbers.Decimal;

    public TimeSpan MinSize => TimeSpan.FromMilliseconds(10);

    public string GetFormat()
    {
        string hourSpecifier = CultureInfo.CurrentCulture.Uses24HourClock() ? "HH" : "hh";
        return $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}\n{hourSpecifier}:mm:ss.ff"; // TODO: This assumes colons as the separators, but consider (some) French-language locales use 12h30 rather than 12:30
    }

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        return dateTime.AddMilliseconds(increment * 10);
    }
}
