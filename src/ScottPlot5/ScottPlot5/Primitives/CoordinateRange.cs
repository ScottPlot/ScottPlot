namespace ScottPlot;

/// <summary>
/// Represents a range of values between two coordinates on a single axis
/// </summary>
public readonly record struct CoordinateRange(double Min, double Max)
{
    public static CoordinateRange Infinity => new(double.NegativeInfinity, double.PositiveInfinity);

    /// <summary>
    /// This infinite inverted range is used to indicate a range that has not yet been set
    /// </summary>
    public static CoordinateRange NotSet => new(double.PositiveInfinity, double.NegativeInfinity);

    public static CoordinateRange NoLimits => new(double.NaN, double.NaN);

    public double Span => Max - Min;
    public double Center => (Min + Max) / 2;
    public double TrueMin => Math.Min(Min, Max);
    public double TrueMax => Math.Max(Min, Max);
    public double AbsSpan => Math.Abs(Max - Min);

    public bool IsReal => NumericConversion.IsReal(Max) && NumericConversion.IsReal(Min);
    public bool IsInverted => Min > Max;

    public override string ToString()
    {
        return $"Min={Min}, Max={Max}, Span={Span}";
    }

    /// <summary>
    /// Returns true if the given position is within the range (inclusive)
    /// </summary>
    public bool Contains(double position)
    {
        return TrueMin <= position && position <= TrueMax;
    }

    public bool Intersects(CoordinateRange other)
    {
        // When intersecting, one range must contain points within the other range.
        return Contains(other.TrueMin) || other.Contains(TrueMin) ||
               Contains(other.TrueMax) || other.Contains(TrueMax);
    }

    /// <summary>
    /// Return the range of values spanned by the given collection
    /// </summary>
    public static CoordinateRange MinMax(IEnumerable<double> values)
    {
        return values is not null && values.Any()
            ? new CoordinateRange(values.Min(), values.Max())
            : NotSet;
    }

    /// <summary>
    /// Return the range of values spanned by the given collection (ignoring NaN)
    /// </summary>
    public static CoordinateRange MinMaxNan(IEnumerable<double> values)
    {
        return MinMax(values.Where(i => !double.IsNaN(i)));
    }

    /// <summary>
    /// Return a new range expanded to include the given point
    /// </summary>
    public CoordinateRange Expanded(double value)
    {
        return new(Math.Min(value, Min), Math.Max(value, Max));
    }

    /// <summary>
    /// Return a copy of this range where <see cref="Max"/> is never less than <see cref="Min"/>
    /// </summary>
    public CoordinateRange Rectified()
    {
        return new(TrueMin, TrueMax);
    }
}
