using System.Globalization;
using ScottPlot.TickGenerators.TimeUnits;

namespace ScottPlot.Axis.TimeUnits;

public class Year : ITimeUnit
{
    public IReadOnlyList<int> NiceIncrements => NiceNumbers.Years;

    public TimeSpan MinSize => TimeSpan.FromDays(365);

    public string GetDateTimeFormatString()
    {
        return $"yyyy";
    }

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        return dateTime.AddYears(increment);
    }
}
