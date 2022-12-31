namespace ScottPlot.Style;

[Obsolete()]
public struct Stroke
{
    public Color Color { get; set; } = Colors.Black;

    public float Width { get; set; } = 1;

    public Stroke()
    {
    }

    public Stroke(Color color, float width)
    {
        Color = color;
        Width = width;
    }
}
