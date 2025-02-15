namespace ScottPlot;

/// <summary>
/// Represents a range of values between a pair of bounding coordinates on a single axis.
/// Inverted ranges are permitted, but <see cref="Min"/> is always less than <see cref="Max"/>
/// and <see cref="IsInverted"/> indicates whether this range is inverted.
/// </summary>
public readonly struct CoordinateRange(double value1, double value2)
{
    public readonly double Value1 = value1;
    public readonly double Value2 = value2;

    public readonly double Min = Math.Min(value1, value2);
    public readonly double Max = Math.Max(value1, value2);
    public readonly bool IsInverted = value1 > value2;

    /// <summary>
    /// Distance from <see cref="Value1"/> to <see cref="Value2"/> (may be negative)
    /// </summary>
    public double Span => Value2 - Value1;

    /// <summary>
    /// Value located in the center of the range, between <see cref="Value1"/> and <see cref="Value2"/> (may be negative)
    /// </summary>
    public double Center => (Value1 + Value2) / 2;

    /// <summary>
    /// Distance from <see cref="Min"/> to <see cref="Max"/> (always positive)
    /// </summary>
    public double Length => Math.Abs(Span);

    /// <summary>
    /// Return the present range rectified so <see cref="Value1"/> is not greater than <see cref="Value2"/>
    /// </summary>
    public CoordinateRange Rectified() => new(Min, Max);

    public override string ToString()
    {
        return IsInverted
            ? $"CoordinateRange [{Min}, {Max}] (inverted)"
            : $"CoordinateRange [{Min}, {Max}]";
    }

    // TODO: Figure out how to avoid using NotSet and NoLimits magic numbers.
    // This struct should probably be broken into a small collection of types.

    /// <summary>
    /// This magic value is used to indicate the range has not been set.
    /// It is equal to an inverted infinite range [∞, -∞]
    /// </summary>
    public static CoordinateRange NotSet => new(double.PositiveInfinity, double.NegativeInfinity);

    /// <summary>
    /// This magic value is used to indicate the range has no defined limits.
    /// It is equal to an inverted infinite range [NaN, NaN]
    /// </summary>
    public static CoordinateRange NoLimits => new(double.NaN, double.NaN);

    /// <summary>
    /// Returns true if the given position is within the range (inclusive)
    /// </summary>
    public bool Contains(double value)
    {
        return Min <= value && value <= Max;
    }

    /// <summary>
    /// Indicates whether two ranges have any overlapping values
    /// </summary>
    public bool Overlaps(CoordinateRange other)
    {
        if (Contains(other.Min) || Contains(other.Max))
            return true;

        if (other.Contains(Min) || other.Contains(Max))
            return true;

        return false;
    }

    /// <summary>
    /// Return the range of values spanned by the given collection (ignoring NaN)
    /// </summary>
    public static CoordinateRange Extrema(IEnumerable<double> values)
    {
        var nonNanValues = values.Where(i => !double.IsNaN(i)).ToList();
        return nonNanValues.Any()
            ? new CoordinateRange(nonNanValues.Min(), nonNanValues.Max())
            : NoLimits;
    }

    /// <summary>
    /// Return a new range expanded to include the given point
    /// </summary>
    public CoordinateRange Expanded(double value)
    {
        double min = Math.Min(Min, value);
        double max = Math.Max(Max, value);
        return new(min, max);
    }

    public bool Equals(CoordinateRange other)
    {
        return Equals(Value1, other.Value1) && Equals(Value2, other.Value2);
    }

    public static bool operator ==(CoordinateRange a, CoordinateRange b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(CoordinateRange a, CoordinateRange b)
    {
        return !a.Equals(b);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is CoordinateRange other)
        {
            return Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Value1.GetHashCode();
        hash = hash * 23 + Value2.GetHashCode();
        return hash;
    }
}
