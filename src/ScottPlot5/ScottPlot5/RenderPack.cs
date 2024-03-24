namespace ScottPlot;

/// <summary>
/// This object pairs a Plot with pixel/canvas information
/// and is passed throughout the render system to provide
/// screen and canvas information to methods performing rendering.
/// </summary>
public class RenderPack
{
    public SKCanvas Canvas { get; }
    public CanvasState CanvasState { get; }
    public PixelRect FigureRect { get; }
    public PixelRect DataRect { get; private set; }
    public Layout Layout { get; private set; }
    public Plot Plot { get; }
    private Stopwatch Stopwatch { get; }
    public TimeSpan Elapsed => Stopwatch.Elapsed;

    /// <summary>
    /// This object is passed through the render system.
    /// The plot will be drawn on the canvas to create a figure of the given size.
    /// </summary>
    public RenderPack(Plot plot, PixelRect figureRect, SKCanvas canvas)
    {
        Canvas = canvas;
        CanvasState = new(canvas);
        FigureRect = figureRect;
        Plot = plot;
        Stopwatch = Stopwatch.StartNew();
    }

    public void CalculateLayout()
    {
        if (DataRect.HasArea)
            throw new InvalidOperationException("DataRect must only be calculated once per render");

        PixelRect scaledRect = new(
            left: FigureRect.Left / Plot.ScaleFactorF,
            right: FigureRect.Right / Plot.ScaleFactorF,
            bottom: FigureRect.Bottom / Plot.ScaleFactorF,
            top: FigureRect.Top / Plot.ScaleFactorF);

        Layout = Plot.Layout.LayoutEngine.GetLayout(scaledRect, Plot.Axes.GetPanels());
        DataRect = Layout.DataRect;
    }

    public override string ToString()
    {
        return $"RenderPack FigureRect={FigureRect} DataRect={DataRect}";
    }

    [Obsolete("Call CanvasState.Clip() instead", true)]
    public void ClipToDataArea()
    {
        CanvasState.Clip(DataRect);
    }

    [Obsolete("Call CanvasState.DisableClipping() instead", true)]
    public void DisableClipping()
    {
        CanvasState.DisableClipping();
    }
}
