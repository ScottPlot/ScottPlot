namespace ScottPlot.Plottables;

public class Markers(IScatterSource data) : IPlottable, IHasMarker, IHasLegendText
{
    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public MarkerStyle MarkerStyle { get; set; } = new() { LineWidth = 1 };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public IScatterSource Data { get; } = data;

    public Color Color
    {
        get => MarkerStyle.FillColor;
        set
        {
            MarkerStyle.FillColor = value;
            MarkerStyle.LineColor = value;
        }
    }

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, MarkerStyle);

    public virtual void Render(RenderPack rp)
    {
        IReadOnlyList<Coordinates> points = Data.GetScatterPoints();

        if (this.MarkerStyle == MarkerStyle.None || points.Count == 0)
            return;

        IEnumerable<Pixel> markerPixels = Data.GetScatterPoints().Select(Axes.GetPixel);

        using SKPaint paint = new();
        Drawing.DrawMarkers(rp.Canvas, paint, markerPixels, MarkerStyle);
    }
}
