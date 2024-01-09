namespace ScottPlot;

public readonly record struct CoordinateRangeStruct(double Min, double Max)
{
    public double Span => Max - Min;
    public double Center => (Min + Max) / 2;
}