namespace ScottPlot;

// TODO: break this into CoordinateRangeX and CoordinateRangeY?
// TODO: a struct is useful and a class is useful for limits only within the axis base or something

/// <summary>
/// Represents a range of values between two coordinates on a single axis
/// </summary>
public class CoordinateRange
{
    public double Min { get; set; }
    public double Max { get; set; }
    public double Center => (Min + Max) / 2;
    public double Span => Max - Min;
    public bool HasBeenSet => Max >= Min;

    public CoordinateRange(double min, double max)
    {
        Min = min;
        Max = max;
    }

    public override string ToString()
    {
        return $"Min={Min}, Max={Max}, Span={Span}";
    }

    /// <summary>
    /// Returns true if the given position is within the range (inclusive)
    /// </summary>
    public bool Contains(double position)
    {
        return position >= Min && position <= Max;
    }

    /// <summary>
    /// Expand the range if needed to include the given point
    /// </summary>
    public void Expand(double value)
    {
        Min = Math.Min(Min, value);
        Max = Math.Max(Max, value);
    }

    /// <summary>
    /// Expand this range if needed to ensure the given range is included
    /// </summary>
    public void Expand(CoordinateRange range)
    {
        Min = Math.Min(Min, range.Min);
        Max = Math.Max(Max, range.Max);
    }

    /// <summary>
    /// This infinite inverted range is used to indicate a range that has not yet been set
    /// </summary>
    public static CoordinateRange NotSet => new(double.PositiveInfinity, double.NegativeInfinity);

    /// <summary>
    /// Reset this range to inverted infinite values to indicate the range has not yet been set
    /// </summary>
    public void Reset()
    {
        Min = double.PositiveInfinity;
        Max = double.NegativeInfinity;
        if (HasBeenSet)
            throw new InvalidOperationException();
    }

    public void Set(double min, double max)
    {
        Min = min;
        Max = max;
    }

    public void Set(CoordinateRange range)
    {
        Min = range.Min;
        Max = range.Max;
    }

    public void Pan(double delta)
    {
        Min += delta;
        Max += delta;
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
}
