namespace ScottPlot;

public readonly record struct CoordinateRange(double Min, double Max)
{
    public double Span => Max - Min;
    public double Center => (Min + Max) / 2;
    public static CoordinateRange Infinity => new(double.NegativeInfinity, double.PositiveInfinity);
    public static CoordinateRange NotSet => new(double.PositiveInfinity, double.NegativeInfinity);
    public bool IsReal => NumericConversion.IsReal(Max) && NumericConversion.IsReal(Min);

    // TODO: ranges could be inverted, so min/max should be renamed start/stop
    public bool IsInverted => Min > Max;
    public double TrueMin => Math.Min(Min, Max);
    public double TrueMax => Math.Max(Min, Max);

    public bool Contains(double value)
    {
        if (value < TrueMin)
            return false;
        else if (value > TrueMax)
            return false;
        else
            return true;
    }

    public bool Intersects(CoordinateRange other)
    {
        var trueMin = Math.Min(Min, Max);
        var trueMax = Math.Max(Min, Max);
        var otherTrueMin = Math.Min(other.Min, other.Max);
        var otherTrueMax = Math.Max(other.Min, other.Max);

        // other engulfs this
        if (otherTrueMin < trueMin && otherTrueMax > trueMax)
            return true;

        // this engulfs other
        if (trueMin < otherTrueMin && trueMax > otherTrueMax)
            return true;

        // partial intersection
        if (Contains(otherTrueMin) || Contains(otherTrueMax))
            return true;

        return false;
    }

    /// <summary>
    /// Return the range of values spanned by the given collection
    /// </summary>
    public static CoordinateRange MinMax(IEnumerable<double> values)
    {
        if (values.Any())
            return NotSet;

        double min = double.MaxValue;
        double max = double.MinValue;

        foreach (double value in values)
        {
            min = Math.Min(min, value);
            max = Math.Max(max, value);
        }

        return new CoordinateRange(min, max);
    }

    /// <summary>
    /// Return the range of values spanned by the given collection (ignoring NaN)
    /// </summary>
    public static CoordinateRange MinMaxNan(IEnumerable<double> values)
    {
        double min = double.NaN;
        double max = double.NaN;

        foreach (double value in values)
        {
            if (double.IsNaN(value)) continue;
            min = double.IsNaN(min) ? value : Math.Min(min, value);
            max = double.IsNaN(max) ? value : Math.Max(max, value);
        }

        return new CoordinateRange(min, max);
    }

    /// <summary>
    /// Return a new range expanded to include the given point
    /// </summary>
    public CoordinateRange Expanded(double value)
    {
        double min = Math.Min(value, Min);
        double max = Math.Max(value, Max);
        return new CoordinateRange(min, max);
    }
}
