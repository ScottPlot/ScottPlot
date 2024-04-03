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
}
