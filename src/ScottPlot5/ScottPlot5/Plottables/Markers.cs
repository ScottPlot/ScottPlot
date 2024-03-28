namespace ScottPlot.Plottables;

public class Markers(IScatterSource data) : IPlottable
{
    public string Label { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public MarkerStyle MarkerStyle { get; set; } = MarkerStyle.Default;

    public IScatterSource Data { get; } = data;

    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }

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

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, MarkerStyle);

    public void Render(RenderPack rp)
    {
        if (this.MarkerStyle == MarkerStyle.None)
            return;

        // TODO: can this be more effecient by moving this logic into the DataSource to avoid copying?
        Pixel[] markerPixels = Data.GetScatterPoints().Select(Axes.GetPixel).ToArray();

        if (!markerPixels.Any())
            return;

        using SKPaint paint = new();
        Drawing.DrawMarkers(rp.Canvas, paint, markerPixels, MarkerStyle);
    }
}
