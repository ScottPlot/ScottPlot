namespace ScottPlot;

public readonly struct Tick
{
    public readonly double Position;
    public readonly string Label;
    public readonly bool IsMajor;

    public Tick(double position, string label)
    {
        Position = position;
        Label = label;
        IsMajor = true;
    }

    public Tick(double position, string label, bool isMajor)
    {
        Position = position;
        Label = label;
        IsMajor = isMajor;
    }

    public static Tick Major(double position, string label)
    {
        return new Tick(position, label, true);
    }

    public static Tick Minor(double position)
    {
        return new Tick(position, string.Empty, false);
    }
}
