namespace ScottPlot;

/// <summary>
/// Represents a range of values between two coordinates on a single axis
/// </summary>
public struct CoordinateRange
{
    public readonly double Min;
    public readonly double Max;
    public readonly double Span;

    public CoordinateRange(double min, double max)
    {
        Min = min;
        Max = max;
        Span = max - min;
    }

    public override string ToString()
    {
        return $"Min={Min}, Max={Max}, Span={Span}";
    }

    public bool Contains(double position)
    {
        return position >= Min && position <= Max;
    }
}
