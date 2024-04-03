namespace ScottPlot.Plottables;

public class Scatter(IScatterSource data) : IPlottable
{
    public string Label { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public LineStyle LineStyle { get; set; } = new();

    public MarkerStyle MarkerStyle { get; set; } = MarkerStyle.Default;

    public IScatterSource Data { get; } = data;

    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }

    /// <summary>
    /// The style of lines to use when connecting points.
    /// </summary>
    public ConnectStyle ConnectStyle = ConnectStyle.Straight;

    /// <summary>
    /// If enabled, points will be connected by smooth lines instead of straight diagnal lines.
    /// <see cref="SmoothTension"/> adjusts the smoothnes of the lines.
    /// </summary>
    public bool Smooth = false;

    /// <summary>
    /// Tension to use for smoothing when <see cref="Smooth"/> is enabled
    /// </summary>
    public double SmoothTension = 0.5;

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

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, MarkerStyle, LineStyle);

    public void Render(RenderPack rp)
    {
        // TODO: can this be more effecient by moving this logic into the DataSource to avoid copying?
        Pixel[] markerPixels = Data.GetScatterPoints().Select(Axes.GetPixel).ToArray();

        if (!markerPixels.Any())
            return;

        Pixel[] linePixels = ConnectStyle switch
        {
            ConnectStyle.Straight => markerPixels,
            ConnectStyle.StepHorizontal => GetStepDisplayPixels(markerPixels, true),
            ConnectStyle.StepVertical => GetStepDisplayPixels(markerPixels, false),
            _ => throw new NotImplementedException($"unsupported {nameof(ConnectStyle)}: {ConnectStyle}"),
        };

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, linePixels, LineStyle);
        Drawing.DrawMarkers(rp.Canvas, paint, markerPixels, MarkerStyle);
    }

    /// <summary>
    /// Convert scatter plot points (connected by diagnal lines) to step plot points (connected by right angles)
    /// by inserting an extra point between each of the original data points to result in L-shaped steps.
    /// </summary>
    /// <param name="points">Array of corner positions</param>
    /// <param name="right">Indicates that a line will extend to the right before rising or falling.</param>
    public static Pixel[] GetStepDisplayPixels(Pixel[] pixels, bool right)
    {
        Pixel[] pixelsStep = new Pixel[pixels.Count() * 2 - 1];

        int offsetX = right ? 1 : 0;
        int offsetY = right ? 0 : 1;

        for (int i = 0; i < pixels.Count() - 1; i++)
        {
            pixelsStep[i * 2] = pixels[i];
            pixelsStep[i * 2 + 1] = new Pixel(pixels[i + offsetX].X, pixels[i + offsetY].Y);
        }

        pixelsStep[pixelsStep.Length - 1] = pixels[pixels.Length - 1];

        return pixelsStep;
    }
}
