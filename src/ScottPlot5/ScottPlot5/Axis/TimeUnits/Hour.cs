using System.Globalization;

namespace ScottPlot.Axis.TimeUnits;

public class Hour : ITimeUnit
{
    public IReadOnlyList<int> NiceIncrements => NiceNumbers.Dozenal;

    public TimeSpan MinSize => TimeSpan.FromHours(1);

    public string GetFormat()
    {
        return $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}\n{CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern}";
    }

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        return dateTime.AddHours(increment);
    }
}
