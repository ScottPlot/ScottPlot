using ScottPlot.TickGenerators.TimeUnits;

namespace ScottPlot.Axis.TimeUnits;

public class Month : ITimeUnit
{
    public IReadOnlyList<int> Divisors => StandardDivisors.Months;

    public TimeSpan MinSize => TimeSpan.FromDays(28);

    public string GetDateTimeFormatString()
    {
        return $"d";
    }

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        return dateTime.AddMonths(increment);
    }
}
