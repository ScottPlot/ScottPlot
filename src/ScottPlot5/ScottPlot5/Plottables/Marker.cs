namespace ScottPlot.Plottables;

public class Marker : IPlottable
{
    public double X { get; set; }
    public double Y { get; set; }
    public Coordinates Location
    {
        get => new(X, Y);
        set { X = value.X; Y = value.Y; }
    }

    public string Label { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public MarkerStyle MarkerStyle { get; set; } = MarkerStyle.Default;
    public float Size { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public float LineWidth { get => MarkerStyle.Outline.Width; set => MarkerStyle.Outline.Width = value; }
    public MarkerShape Shape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public Color Color
    {
        get => MarkerStyle.Fill.Color;
        set { MarkerStyle.Fill.Color = value; MarkerStyle.Outline.Color = value; }
    }

    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, MarkerStyle);
    public AxisLimits GetAxisLimits() => new(Location);

    public void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        Drawing.DrawMarker(rp.Canvas, paint, Axes.GetPixel(Location), MarkerStyle);
    }
}
