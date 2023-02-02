namespace ScottPlot;

/// <summary>
/// These methods hold conversion logic between DateTime (used for representing dates)
/// and double (used for defining positions on a Cartesian coordinate plane).
/// 
/// Convering double <-> OADate has issues (e.g., OADates have a limited range), so by
/// isolating the conversion here we can ensure we can change the logic later without
/// hunting around the code base finding all the OADate conversion calls.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// Convert a number that can be plotted on a numeric axis into a DateTime
    /// </summary>
    public static DateTime ToDateTime(this double value)
    {
        if (value <= -657435)
            return new DateTime(100, 1, 1);

        if (value >= 2958466)
            return new DateTime(9_999, 1, 1);

        return DateTime.FromOADate(value);
    }

    /// <summary>
    /// Convert a DateTime into a number that can be plotted on a numeric axis
    /// </summary>
    public static double ToNumber(this DateTime value)
    {
        if (value.Year < 100)
            return new DateTime(100, 1, 1).ToOADate();

        if (value.Year >= 10_000)
            return new DateTime(10_000, 1, 1).ToOADate();

        return value.ToOADate();
    }
}
