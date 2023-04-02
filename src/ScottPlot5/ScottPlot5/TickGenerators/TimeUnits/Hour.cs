namespace ScottPlot.TickGenerators.TimeUnits;

public class Hour : ITimeUnit
{
    public IReadOnlyList<int> Divisors => StandardDivisors.Dozenal;

    public TimeSpan MinSize => TimeSpan.FromHours(1);

    public DateTime Snap(DateTime dt)
    {
        return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, 0, 0);
    }

    public string GetDateTimeFormatString()
    {
        return $"{CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern}\n{CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern}";
    }

    public DateTime Next(DateTime dateTime, int increment = 1)
    {
        return dateTime.AddHours(increment);
    }
}
