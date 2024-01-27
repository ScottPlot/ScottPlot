namespace ScottPlot;

public readonly record struct CoordinateRange(double Min, double Max)
{
    public double Span => Max - Min;
    public double Center => (Min + Max) / 2;
    public static CoordinateRange Infinity => new(double.NegativeInfinity, double.PositiveInfinity);
    public static CoordinateRange NotSet => new(double.PositiveInfinity, double.NegativeInfinity);
    public bool IsReal => NumericConversion.IsReal(Max) && NumericConversion.IsReal(Min);

    public bool Contains(double value)
    {
        if (value < Min)
            return false;
        else if (value > Max)
            return false;
        else
            return true;
    }

    public bool Intersects(CoordinateRange other)
    {
        // other engulfs this
        if (other.Min < Min && other.Max > Max)
            return true;

        // this engulfs other
        if (Min < other.Min && Max > other.Max)
            return true;

        // partial intersection
        if (Contains(other.Min) || Contains(other.Max))
            return true;

        return false;
    }
}
