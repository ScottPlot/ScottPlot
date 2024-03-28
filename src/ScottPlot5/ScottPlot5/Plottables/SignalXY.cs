namespace ScottPlot.Plottables;

public class SignalXY : IPlottable
{
    public ISignalXYSource Data { get; set; }

    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public IAxes Axes { get; set; } = new Axes();
    public LineStyle LineStyle { get; set; } = new();
    public Color Color { get => LineStyle.Color; set => LineStyle.Color = value; }
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public string Label = string.Empty;
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, LineStyle);

    public SignalXY(ISignalXYSource dataSource)
    {
        Data = dataSource;
    }

    public AxisLimits GetAxisLimits() => Data.GetAxisLimits();

    public DataPoint GetNearest(Coordinates location, RenderDetails renderInfo, float maxDistance = 15) => Data.GetNearest(location, renderInfo, maxDistance);

    public void Render(RenderPack rp)
    {
        Pixel[] pixels = Data.GetPixelsToDraw(rp, Axes);

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, pixels, LineStyle);
    }
}
