namespace ScottPlot.Style;

public struct Fill
{
    public Color Color { get; set; } = Colors.CornflowerBlue;

    public Hatch? Hatch { get; set; } = null;

    public Fill()
    {
    }

    public Fill(Color color)
    {
        Color = color;
    }
}
