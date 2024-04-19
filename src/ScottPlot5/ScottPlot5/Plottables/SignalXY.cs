namespace ScottPlot.Plottables;

public class SignalXY(ISignalXYSource dataSource) : IPlottable, IHasLine, IHasMarker
{
    public ISignalXYSource Data { get; set; } = dataSource;

    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public IAxes Axes { get; set; } = new Axes();

    public LineStyle LineStyle { get; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public MarkerStyle MarkerStyle { get; } = new() { Size = 5, Shape = MarkerShape.FilledCircle };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.Fill.Color; set => MarkerStyle.Fill.Color = value; }
    public Color MarkerLineColor { get => MarkerStyle.Outline.Color; set => MarkerStyle.Outline.Color = value; }
    public float MarkerLineWidth { get => MarkerStyle.Outline.Width; set => MarkerStyle.Outline.Width = value; }

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.Fill.Color = value;
            MarkerStyle.Outline.Color = value;
        }
    }

    public string Label = string.Empty;
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, LineStyle, MarkerStyle);

    public AxisLimits GetAxisLimits() => Data.GetAxisLimits();

    public DataPoint GetNearest(Coordinates location, RenderDetails renderInfo, float maxDistance = 15) =>
        Data.GetNearest(location, renderInfo, maxDistance);

    public void Render(RenderPack rp)
    {
        Pixel[] pixels = Data.GetPixelsToDraw(rp, Axes);

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, pixels, LineStyle);
        Drawing.DrawMarkers(rp.Canvas, paint, pixels, MarkerStyle);
    }
}
