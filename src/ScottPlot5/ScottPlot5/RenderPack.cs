namespace ScottPlot;

public class RenderPack
{
    public SKCanvas Canvas { get; }
    public PixelSize FigureSize { get; }
    public PixelRect DataRect { get; private set; }
    public Layout Layout { get; private set; }
    public Plot Plot { get; }
    private Stopwatch Stopwatch { get; }
    public TimeSpan Elapsed => Stopwatch.Elapsed;

    /// <summary>
    /// This object is passed through the render system.
    /// The plot will be drawn on the canvas to create a figure of the given size.
    /// </summary>
    public RenderPack(Plot plot, PixelSize figureSize, SKCanvas canvas)
    {
        Canvas = canvas;
        FigureSize = figureSize;
        Plot = plot;
        Stopwatch = Stopwatch.StartNew();
    }

    public void CalculateLayout()
    {
        if (DataRect.HasArea)
            throw new InvalidOperationException("DataRect must only be calculated once per render");

        PixelSize figSize = new(
            width: FigureSize.Width / Plot.ScaleFactor,
            height: FigureSize.Height / Plot.ScaleFactor);

        Layout = Plot.LayoutEngine.GetLayout(figSize, Plot.GetAllPanels());
        DataRect = Layout.DataRect;
    }

    public override string ToString()
    {
        return $"RenderPack FigureSize={FigureSize} DataRect={DataRect}";
    }
}
