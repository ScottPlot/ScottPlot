namespace ScottPlot;

public struct Alignment
{
    public HorizontalAlignment X { get; set; }
    public VerticalAlignment Y { get; set; }

    public Alignment(
        HorizontalAlignment x = HorizontalAlignment.Center,
        VerticalAlignment y = VerticalAlignment.Center)
    {
        X = x;
        Y = y;
    }
}

public enum HorizontalAlignment
{
    Left,
    Center,
    Right,
}

public enum VerticalAlignment
{
    Bottom,
    Center,
    Top,
}
