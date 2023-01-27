using System.Globalization;

namespace ScottPlot.Axis.TimeUnits;

public class Second : ITimeUnit
{
    public IReadOnlyList<int> NiceIncrements => NiceNumbers.Sexagesimal;

    public TimeSpan MinSize => TimeSpan.FromSeconds(1);

    public string GetFormat()
    {
        return $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}\n{CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern}";
    }

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        return dateTime.AddSeconds(increment);
    }
}
