namespace ScottPlot.Rendering;

public class RenderManager
{
    /// <summary>
    /// This list of actions is performed in sequence to render a plot.
    /// It may be modified externally to inject custom functionality.
    /// </summary>
    public List<IRenderAction> RenderActions { get; }

    /// <summary>
    /// Information about the previous render
    /// </summary>
    public RenderDetails LastRenderInfo { get; private set; }

    /// <summary>
    /// Total number of renders
    /// </summary>
    public int RenderCount { get; private set; } = 0;

    private Plot Plot { get; }

    public RenderManager(Plot plot)
    {
        Plot = plot;
        RenderActions = new(DefaultRenderActions());
    }

    public IRenderAction[] DefaultRenderActions()
    {
        return new IRenderAction[]
        {
            new RenderActions.ReplaceNullAxesWithDefaults(),
            new RenderActions.AutoAxisAnyUnsetAxes(),
            new RenderActions.EnsureAxesHaveArea(),
            new RenderActions.CalculateLayout(),
            new RenderActions.RegenerateTicks(),
            new RenderActions.RenderBackground(),
            new RenderActions.RenderGridsBelowPlottables(),
            new RenderActions.RenderPlottables(),
            new RenderActions.RenderGridsAbovePlottables(),
            new RenderActions.RenderLegends(),
            new RenderActions.RenderPanels(),
            new RenderActions.RenderZoomRectangle(),
            new RenderActions.SyncGLPlottables(),
            new RenderActions.RenderBenchmark(),
        };
    }

    public void Render(SKCanvas canvas, int width, int height)
    {
        canvas.Scale(Plot.ScaleFactor);

        List<(string, TimeSpan)> actionTimes = new();

        PixelSize figureSize = new(width, height);
        RenderPack rp = new(Plot, figureSize, canvas);

        Stopwatch sw = new();
        foreach (IRenderAction action in RenderActions)
        {
            sw.Restart();
            action.Render(rp);
            actionTimes.Add((action.ToString() ?? string.Empty, sw.Elapsed));
        }

        LastRenderInfo = new(rp, actionTimes.ToArray());

        RenderCount += 1;
    }

    public void Render(SKSurface surface)
    {
        Render(surface.Canvas, (int)surface.Canvas.LocalClipBounds.Width, (int)surface.Canvas.LocalClipBounds.Height);
    }
}
