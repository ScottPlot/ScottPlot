namespace ScottPlot.Plottables;

public class Marker : IPlottable, IHasMarker, IHasLegendText
{
    public double X { get; set; }
    public double Y { get; set; }
    public Coordinates Location
    {
        get => new(X, Y);
        set { X = value.X; Y = value.Y; }
    }

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;

    public MarkerStyle MarkerStyle { get; set; } = new();
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor
    {
        get => MarkerFillColor;
        set
        {
            MarkerFillColor = value;
            MarkerLineColor = value;
        }
    }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public float Size { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public float LineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }
    public MarkerShape Shape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public Color Color
    {
        get => MarkerFillColor;
        set
        {
            MarkerFillColor = value;
            MarkerLineColor = value;
        }
    }

    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, MarkerStyle);
    public AxisLimits GetAxisLimits() => new(Location);

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        Drawing.DrawMarker(rp.Canvas, paint, Axes.GetPixel(Location), MarkerStyle);
    }
}
