namespace ScottPlot.Plottables;

public class Markers(IScatterSource data) : IPlottable, IHasMarker, IHasLegendText
{
    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public MarkerStyle MarkerStyle { get; set; } = new() { OutlineWidth = 1 };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.OutlineColor; set => MarkerStyle.OutlineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.OutlineWidth; set => MarkerStyle.OutlineWidth = value; }

    public IScatterSource Data { get; } = data;

    public Color Color
    {
        get => MarkerStyle.FillColor;
        set
        {
            MarkerStyle.FillColor = value;
            MarkerStyle.OutlineColor = value;
        }
    }

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, MarkerStyle);

    public virtual void Render(RenderPack rp)
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
