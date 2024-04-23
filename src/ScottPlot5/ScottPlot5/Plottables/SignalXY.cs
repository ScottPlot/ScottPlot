namespace ScottPlot.Plottables;

public class SignalXY(ISignalXYSource dataSource) : IPlottable, IHasLine, IHasMarker, IHasLegendText
{
    public ISignalXYSource Data { get; set; } = dataSource;

    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public IAxes Axes { get; set; } = new Axes();

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public MarkerStyle MarkerStyle { get; set; } = new() { Size = 0, Shape = MarkerShape.FilledCircle };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.OutlineColor; set => MarkerStyle.OutlineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.OutlineWidth; set => MarkerStyle.OutlineWidth = value; }

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.FillColor = value;
            MarkerStyle.OutlineColor = value;
        }
    }

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, LineStyle, MarkerStyle);

    public AxisLimits GetAxisLimits() => Data.GetAxisLimits();

    public DataPoint GetNearest(Coordinates location, RenderDetails renderInfo, float maxDistance = 15) =>
        Data.GetNearest(location, renderInfo, maxDistance);

    public virtual void Render(RenderPack rp)
    {
        Pixel[] pixels = Data.GetPixelsToDraw(rp, Axes);

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, pixels, LineStyle);
        Drawing.DrawMarkers(rp.Canvas, paint, pixels, MarkerStyle);
    }
}
