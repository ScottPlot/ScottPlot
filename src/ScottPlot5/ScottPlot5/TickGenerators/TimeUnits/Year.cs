using System.Globalization;
using ScottPlot.TickGenerators.TimeUnits;

namespace ScottPlot.Axis.TimeUnits;

public class Year : ITimeUnit
{
    public IReadOnlyList<int> Divisors => StandardDivisors.Years;

    public TimeSpan MinSize => TimeSpan.FromDays(365);

    public DateTime Snap(DateTime dt)
    {
        return new DateTime(dt.Year);
    }

    public string GetDateTimeFormatString()
    {
        return $"yyyy";
    }

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        try
        {
            return dateTime.AddYears(increment);
        }
        catch (ArgumentOutOfRangeException)
        {
            return DateTime.MaxValue;
        }
    }
}
