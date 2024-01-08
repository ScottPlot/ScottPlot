namespace ScottPlot.DataSources;

public readonly struct SignalRangeY
{
    public double Min { get; }
    public double Max { get; }
    public SignalRangeY(double min, double max)
    {
        Min = min;
        Max = max;
    }
    public override string ToString()
    {
        return $"Range [{Min}, {Max}]";
    }
}
