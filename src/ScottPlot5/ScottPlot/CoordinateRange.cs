namespace ScottPlot;

/// <summary>
/// Represents a range of values between two coordinates on a single axis
/// </summary>
public struct CoordinateRange
{
    public double Min { get; private set; }
    public double Max { get; private set; }
    public double Span => Max - Min;

    public CoordinateRange(double min, double max)
    {
        Min = min;
        Max = max;
    }

    public void Expand(double value)
    {
        if (value < Min)
            Min = value;
        if (value > Max)
            Max = value;
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
