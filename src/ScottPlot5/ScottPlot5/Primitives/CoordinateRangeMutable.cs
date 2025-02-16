namespace ScottPlot;

// TODO: strangle this class and replace with CoordinateRangeStruct

/// <summary>
/// Represents a range of values between two coordinates on a single axis
/// </summary>
public class CoordinateRangeMutable : IEquatable<CoordinateRangeMutable> // TODO: rename to MutableCoordinateRange or something
{
    public double Value1 { get; set; }
    public double Value2 { get; set; }
    public bool IsInverted => Value1 > Value2;
    public double Min {
        get => Math.Min(Value1, Value2);
        set // Unfortunately we need a setter to support code from when Min was allowed to be greater than Max
        {
            if (Value1 < Value2)
            {
                Value1 = value;
            } else
            {
                Value2 = value;
            }
        }
    }
    public double Max
    {
        get => Math.Max(Value1, Value2);
        set
        {
            if (Value1 >= Value2)
            {
                Value1 = value;
            }
            else
            {
                Value2 = value;
            }
        }
    }
    public double Center => (Value1 + Value2) / 2;
    public double Span => Value2 - Value1;

    // TODO: obsolete this
    public bool HasBeenSet => NumericConversion.IsReal(Span) && Span != 0;

    public CoordinateRange ToCoordinateRange => new(Value1, Value2);

    public CoordinateRange ToRectifiedCoordinateRange => new(Min, Max);

    public CoordinateRangeMutable(double value1, double value2)
    {
        Value1 = value1;
        Value2 = value2;
    }

    public static CoordinateRangeMutable Infinity => new(double.NegativeInfinity, double.PositiveInfinity);

    public override string ToString()
    {
        return IsInverted
            ? $"CoordinateRangeMutable [{Min}, {Max}], Span = {Span} (inverted)"
            : $"CoordinateRangeMutable [{Min}, {Max}], Span = {Span}";
    }

    /// <summary>
    /// Returns true if the given position is within the range (inclusive)
    /// </summary>
    public bool Contains(double position)
    {
        return position >= Min && position <= Max; // TODO: Confirm if we want to consider the inverted ranges to include anything (e.g. does [1, -1] include 0?)
    }

    // TODO: deprecate
    /// <summary>
    /// Expand the range if needed to include the given point
    /// </summary>
    public void Expand(double value)
    {
        if (double.IsNaN(value))
            return;

        // TODO: Confirm that we should allow calling Expand to uninvert a range?
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
        Expand(range.Min);
        Expand(range.Max);
    }

    /// <summary>
    /// Expand this range if needed to ensure the given range is included
    /// </summary>
    public void Expand(CoordinateRange range)
    {
        Expand(range.Min);
        Expand(range.Max);
    }

    /// <summary>
    /// This infinite inverted range is used to indicate a range that has not yet been set
    /// </summary>
    public static CoordinateRangeMutable NotSet => new(double.PositiveInfinity, double.NegativeInfinity);

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

    public void Set(double value1, double value2)
    {
        Value1 = value1;
        Value2 = value2;
    }

    public void Set(CoordinateRange range)
    {
        // TODO: Confirm that we want to implicitly uninvert ranges like this?
        if (range.IsInverted)
        {
            Value2 = range.Min;
            Value1 = range.Max;
        }
        else
        {
            Value1 = range.Min;
            Value2 = range.Max;
        }
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
        double spanLeftX = zoomTo - Min;
        double spanRightX = Max - zoomTo;
        Min = zoomTo - spanLeftX / frac;
        Max = zoomTo + spanRightX / frac;
    }

    public bool Equals(CoordinateRangeMutable? other)
    {
        if (other is null)
            return false;

        return Equals(Value1, other.Value1) && Equals(Value2, other.Value2);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj is CoordinateRangeMutable other)
            return Equals(other);

        return false;
    }

    public static bool operator ==(CoordinateRangeMutable a, CoordinateRangeMutable b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(CoordinateRangeMutable a, CoordinateRangeMutable b)
    {
        return !a.Equals(b);
    }

    public override int GetHashCode()
    {
        return Value1.GetHashCode() ^ Value2.GetHashCode();
    }
}
