namespace ScottPlot.Style;

public enum MarkerShape
{
    // TODO: need more shapes... no actually make this a class!
    Circle
}

public struct Marker
{
    public Color Color { get; set; } = Colors.Black;

    public float Size { get; set; } = 5;

    public MarkerShape Shape { get; set; } = MarkerShape.Circle;

    public Marker()
    {
    }

    public Marker(Color color)
    {
        Color = color;
    }

    public Marker(MarkerShape shape, Color color, float size = 5)
    {
        Shape = shape;
        Color = color;
        Size = size;
    }
}
