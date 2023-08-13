namespace ScottPlot;

public class RenderPack
{
    public SKCanvas Canvas { get; }
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
        FigureRect = figureRect;
        Plot = plot;
        Stopwatch = Stopwatch.StartNew();
    }

    public void CalculateLayout()
    {
        if (DataRect.HasArea)
            throw new InvalidOperationException("DataRect must only be calculated once per render");

        PixelRect scaledRect = new(
            left: FigureRect.Left / Plot.ScaleFactor,
            right: FigureRect.Right / Plot.ScaleFactor,
            bottom: FigureRect.Bottom / Plot.ScaleFactor,
            top: FigureRect.Top / Plot.ScaleFactor);

        Layout = Plot.LayoutEngine.GetLayout(scaledRect, Plot.GetAllPanels());
        DataRect = Layout.DataRect;
    }

    public override string ToString()
    {
        return $"RenderPack FigureRect={FigureRect} DataRect={DataRect}";
    }
}
