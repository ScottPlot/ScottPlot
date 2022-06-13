namespace ScottPlot;

public struct Tick
{
    public readonly double Position;
    public readonly string Label;
    public readonly bool IsMajor;

    public Tick(double position, string label, bool isMajor)
    {
        Position = position;
        Label = label;
        IsMajor = isMajor;
    }
}
