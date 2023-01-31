using System.Globalization;

namespace ScottPlot.Axis.TimeUnits;

public class Minute : ITimeUnit
{
    public IReadOnlyList<int> NiceIncrements => NiceNumbers.Sexagesimal;

    public TimeSpan MinSize => TimeSpan.FromMinutes(1);

    public string GetFormat()
    {
        return $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}\n{CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern}";
    }

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        return dateTime.AddMinutes(increment);
    }
}
