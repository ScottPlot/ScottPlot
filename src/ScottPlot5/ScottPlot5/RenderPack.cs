namespace ScottPlot;

/// <summary>
/// This object pairs a Plot with pixel/canvas information
/// and is passed throughout the render system to provide
/// screen and canvas information to methods performing rendering.
/// </summary>
public class RenderPack(Plot plot, PixelRect figureRect, SKCanvas canvas)
{
    public SKCanvas Canvas { get; } = canvas;
    public CanvasState CanvasState { get; } = new(canvas);
    public PixelRect FigureRect { get; } = figureRect;
    public PixelRect DataRect { get; private set; }
    public Layout Layout { get; private set; }
    public Plot Plot { get; } = plot;
    private Stopwatch Stopwatch { get; } = Stopwatch.StartNew();
    public TimeSpan Elapsed => Stopwatch.Elapsed;

    /// <summary>
    /// Uses the layout engine to measure panels and set the
    /// Layout and DataRect for this render pack
    /// </summary>
    internal void CalculateLayout()
    {
        if (DataRect.HasArea)
            throw new InvalidOperationException("CalculateLayout() must only be called once per render");

        PixelRect scaledFigureRect = new(
            left: FigureRect.Left / Plot.ScaleFactorF,
            right: FigureRect.Right / Plot.ScaleFactorF,
            bottom: FigureRect.Bottom / Plot.ScaleFactorF,
            top: FigureRect.Top / Plot.ScaleFactorF);

        Layout = Plot.Layout.LayoutEngine.GetLayout(scaledFigureRect, Plot);
        DataRect = Layout.DataRect;
    }
}
