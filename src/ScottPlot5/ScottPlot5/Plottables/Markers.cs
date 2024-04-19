namespace ScottPlot.Plottables;

public class Markers(IScatterSource data) : IPlottable, IHasMarker, IHasLegendText
{
    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public MarkerStyle MarkerStyle { get; } = new();
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.Fill.Color; set => MarkerStyle.Fill.Color = value; }
    public Color MarkerLineColor { get => MarkerStyle.Outline.Color; set => MarkerStyle.Outline.Color = value; }
    public float MarkerLineWidth { get => MarkerStyle.Outline.Width; set => MarkerStyle.Outline.Width = value; }

    public IScatterSource Data { get; } = data;

    public Color Color
    {
        get => MarkerStyle.Fill.Color;
        set
        {
            MarkerStyle.Fill.Color = value;
            MarkerStyle.Outline.Color = value;
        }
    }

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, MarkerStyle);

    public void Render(RenderPack rp)
    {
        if (this.MarkerStyle == MarkerStyle.None)
            return;

        // TODO: can this be more efficient by moving this logic into the DataSource to avoid copying?
        Pixel[] markerPixels = Data.GetScatterPoints().Select(Axes.GetPixel).ToArray();

        if (!markerPixels.Any())
            return;

        using SKPaint paint = new();
        Drawing.DrawMarkers(rp.Canvas, paint, markerPixels, MarkerStyle);
    }
}
