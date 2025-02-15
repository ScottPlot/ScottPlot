namespace ScottPlot.Plottables;

public class LollipopPlot : IPlottable, IHasLine, IHasMarker
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    public string LegendText { get; set; } = string.Empty;

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(this, LegendText, LineStyle, MarkerStyle);

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public MarkerStyle MarkerStyle { get; set; } = new()
    {
        Size = 5,
        Shape = MarkerShape.FilledCircle,
        IsVisible = true,
    };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.FillColor = value;
            MarkerStyle.LineColor = value;
        }
    }

    public IEnumerable<Coordinates> Coordinates { get; set; }

    public Orientation Orientation { get; set; } = Orientation.Vertical;

    public LollipopPlot(IEnumerable<Coordinates> coordinates)
    {
        Coordinates = coordinates;
    }

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(Coordinates);
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        foreach (Coordinates c in Coordinates)
        {
            Coordinates lineBase = (Orientation == Orientation.Vertical) ? new(c.X, 0) : new(0, c.Y);
            Coordinates lineTip = c;
            CoordinateLine line = new(lineBase, lineTip);
            PixelLine pxLine = Axes.GetPixelLine(line);
            Pixel px = Axes.GetPixel(lineTip);

            Drawing.DrawLine(rp.Canvas, paint, pxLine, LineStyle);
            Drawing.DrawMarker(rp.Canvas, paint, px, MarkerStyle);
        }
    }
}
