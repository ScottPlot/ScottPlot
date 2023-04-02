namespace ScottPlot.TickGenerators.TimeUnits;

public class Second : ITimeUnit
{
    public IReadOnlyList<int> Divisors => StandardDivisors.Sexagesimal;

    public TimeSpan MinSize => TimeSpan.FromSeconds(1);

    public DateTime Snap(DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
    }

    public string GetDateTimeFormatString()
    {
        return $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}\n{CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern}";
    }

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        return dateTime.AddSeconds(increment);
    }
}
