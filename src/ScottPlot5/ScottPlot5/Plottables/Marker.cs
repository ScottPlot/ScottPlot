namespace ScottPlot.Plottables;

public class Marker : IPlottable
{
    public string Label { get; set; } = string.Empty;
    public Coordinates Location { get; set; }
    public bool IsVisible { get; set; } = true;
    public MarkerStyle MarkerStyle { get; set; } = MarkerStyle.Default;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, MarkerStyle);
    public AxisLimits GetAxisLimits() => new(Location);

    public void Render(RenderPack rp)
    {
        Pixel pixelLocation = Axes.GetPixel(Location);
        MarkerStyle.Render(rp.Canvas, pixelLocation);
    }
}
