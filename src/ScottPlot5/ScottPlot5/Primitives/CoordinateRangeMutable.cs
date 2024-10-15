namespace ScottPlot;

// TODO: strangle this class and replace with CoordinateRangeStruct

/// <summary>
/// Represents a range of values between a pair of bounding coordinates on a single axis.
/// Inverted ranges are permitted, but <see cref="Min"/> is always less than <see cref="Max"/>
/// and <see cref="IsInverted"/> indicates whether this range is inverted.
/// </summary>
public class CoordinateRangeMutable(double value1, double value2) : IEquatable<CoordinateRangeMutable> // TODO: rename to MutableCoordinateRange or something
{
    public double Value1 { get; set; } = value1;
    public double Value2 { get; set; } = value2;

    public double Min => double.IsNaN(Value2) ? Value1 : Math.Min(Value1, Value2);
    public double Max => double.IsNaN(Value1) ? Value2 : Math.Max(Value1, Value2);
    public bool IsInverted => Value1 > Value2;

    /// <summary>
    /// Distance from <see cref="Value1"/> to <see cref="Value2"/> (may be negative)
    /// </summary>
    public double Span => Value2 - Value1;

    /// <summary>
    /// Value located in the center of the range, between <see cref="Value1"/> and <see cref="Value2"/> (may be negative)
    /// </summary>
    public double Center => (Value1 + Value2) / 2;

    /// <summary>
    /// Distance from <see cref="Value1"/> to <see cref="Value2"/> (always positive)
    /// </summary>
    public double Length => Math.Abs(Span);

    // TODO: obsolete this
    public bool HasBeenSet => NumericConversion.IsReal(Span) && Length > 0;

    /// <summary>
    /// Return the present range rectified so <see cref="Value1"/> is not greater than <see cref="Value2"/>
    /// </summary>
    public CoordinateRangeMutable Rectified() => new(Min, Max);

    public CoordinateRange ToCoordinateRange() => new(Value1, Value2);

    public override string ToString()
    {
        return IsInverted
            ? $"CoordinateRange [{Min}, {Max}] (inverted)"
            : $"CoordinateRange [{Min}, {Max}]";
    }

    /// <summary>
    /// This magic value is used to indicate the range has not been set.
    /// It is equal to an inverted infinite range [∞, -∞]
    /// </summary>
    public static CoordinateRangeMutable NotSet => new(double.PositiveInfinity, double.NegativeInfinity);

    /// <summary>
    /// This magic value is used to indicate the range has no defined limits.
    /// It is equal to an inverted infinite range [NaN, NaN]
    /// </summary>
    public static CoordinateRangeMutable NoLimits => new(double.NaN, double.NaN);

    /// <summary>
    /// Returns true if the given position is within the range (inclusive)
    /// </summary>
    public bool Contains(double value)
    {
        return Min <= value && value <= Max;
    }

    // TODO: deprecate
    /// <summary>
    /// Expand the range if needed to include the given point
    /// </summary>
    public void Expand(double value)
    {
        if (double.IsNaN(value))
            return;

        if (double.IsNaN(Value1) || value < Value1)
            Value1 = value;

        if (double.IsNaN(Value2) || value > Value2)
            Value2 = value;
    }

    // TODO: deprecate
    /// <summary>
    /// Expand this range if needed to ensure the given range is included
    /// </summary>
    public void Expand(CoordinateRangeMutable range)
    {
        Expand(range.Value1);
        Expand(range.Value2);
    }

    /// <summary>
    /// Expand this range if needed to ensure the given range is included
    /// </summary>
    public void Expand(CoordinateRange range)
    {
        Expand(range.Min);
        Expand(range.Max);
    }

    // TODO: deprecate
    /// <summary>
    /// Reset this range to inverted infinite values to indicate the range has not yet been set
    /// </summary>
    public void Reset()
    {
        Value1 = double.PositiveInfinity;
        Value2 = double.NegativeInfinity;
        if (HasBeenSet)
            throw new InvalidOperationException();
    }

    public void Set(double min, double max, bool inverted = false)
    {
        if (inverted)
        {
            Value1 = max;
            Value2 = min;
        }
        else
        {
            Value1 = min;
            Value2 = max;
        }
    }

    public void Set(CoordinateRange range)
    {
        Value1 = range.Min;
        Value2 = range.Max;
    }

    public void Set(CoordinateRangeMutable range)
    {
        Value1 = range.Value1;
        Value2 = range.Value2;
    }

    public void Set(IAxis otherAxis)
    {
        Value1 = otherAxis.Min;
        Value2 = otherAxis.Max;
    }

    public void Pan(double delta)
    {
        Value1 += delta;
        Value2 += delta;
    }

    public void PanMouse(float mouseDeltaPx, float dataSizePx)
    {
        double pxPerUnitx = dataSizePx / Span;
        double delta = mouseDeltaPx / pxPerUnitx;
        Pan(delta);
    }

    public void ZoomFrac(double frac)
    {
        ZoomFrac(frac, Center);
    }

    public void ZoomOut(double multiple)
    {
        double newSpan = Span * multiple;
        double halfSpan = newSpan / 2;
        Set(Center - halfSpan, Center + halfSpan);
    }

    public void ZoomMouseDelta(float deltaPx, float dataSizePx)
    {
        double deltaFracX = deltaPx / (Math.Abs(deltaPx) + dataSizePx);
        double fracX = Math.Pow(10, deltaFracX);
        ZoomFrac(fracX);
    }

    public void ZoomFrac(double frac, double zoomTo)
    {
        double spanLeftX = zoomTo - Value1;
        double spanRightX = Value2 - zoomTo;
        Value1 = zoomTo - spanLeftX / frac;
        Value2 = zoomTo + spanRightX / frac;
    }

    public static bool operator ==(CoordinateRangeMutable a, CoordinateRangeMutable b)
    {
        return a switch
        {
            not null when b is not null => a.GetHashCode() == b.GetHashCode(),
            _ => a is null && b is null,
        };
    }

    public static bool operator !=(CoordinateRangeMutable a, CoordinateRangeMutable b)
    {
        return !(a switch
        {
            not null when b is not null => a.GetHashCode() == b.GetHashCode(),
            _ => a is null && b is null,
        });
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is CoordinateRangeMutable other)
        {
            return this == other;
        }

        return false;
    }

    public bool Equals(CoordinateRangeMutable? other)
    {
        return other is not null && this == other;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + Value1.GetHashCode();
        hash = hash * 23 + Value2.GetHashCode();
        return hash;
    }
}
