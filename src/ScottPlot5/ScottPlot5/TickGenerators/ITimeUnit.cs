namespace ScottPlot.TickGenerators;

public interface ITimeUnit
{
    /// <summary>
    /// An array of integers that serve as good divisors to subdivide this time unit
    /// </summary>
    public IReadOnlyList<int> Divisors { get; }

    /// <summary>
    /// Returns the format string used to display tick labels of this time unit.
    /// https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tostring
    /// </summary>
    public string GetDateTimeFormatString();

    /// <summary>
    /// Return the DateTime N units relative to this one
    /// </summary>
    public DateTime Next(DateTime dateTime, int increment = 1);

    /// <summary>
    /// Minimum span this time unit can represent.
    /// To represent spans smaller than this, try the next smaller unit.
    /// </summary>
    public TimeSpan MinSize { get; }

    /// <summary>
    /// Return a given date "snapped" back to the nearest nice tick position.
    /// Use this to find a good tick position for a given DateTime.
    /// </summary>
    public DateTime Snap(DateTime dateTime);
}
