using System.Globalization;

namespace ScottPlot.Axis.TimeUnits;

public class Month : ITimeUnit
{
    public IReadOnlyList<int> NiceIncrements => NiceNumbers.Months;

    public TimeSpan MinSize => TimeSpan.FromDays(28);

    public string GetFormat()
    {
        return $"d";
    }

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        return dateTime.AddMonths(increment);
    }
}
