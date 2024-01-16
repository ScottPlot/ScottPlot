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
    public RenderDetails LastRender { get; private set; }

    /// <summary>
    /// These events are invoked before any render action.
    /// Users can add blocking code to this event to ensure processes
    /// that modify plottables are complete before rendering begins.
    /// </summary>
    public EventHandler PreRenderLock { get; set; } = delegate { };

    /// <summary>
    /// This event is invoked just before each render, 
    /// after axis limits are determined and axis limits are set
    /// </summary>
    public EventHandler<RenderPack> RenderStarting { get; set; } = delegate { };

    /// <summary>
    /// This event is invoked after each render
    /// </summary>
    public EventHandler<RenderDetails> RenderFinished { get; set; } = delegate { };

    /// <summary>
    /// This event a render where the figure size (in pixels) changed from the previous render
    /// </summary>
    public EventHandler<RenderDetails> SizeChanged { get; set; } = delegate { };

    /// <summary>
    /// This event a render where the axis limits (in coordinate units) changed from the previous render
    /// </summary>
    public EventHandler<RenderDetails> AxisLimitsChanged { get; set; } = delegate { };

    /// <summary>
    /// Indicates whether this plot is in the process of executing a render
    /// </summary>
    public bool IsRendering { get; private set; } = false;

    /// <summary>
    /// Disable this in multiplot environments
    /// </summary>
    public bool ClearCanvasBeforeRendering { get; set; } = true;


    /// <summary>
    /// If false, any calls to Render() return immediately
    /// </summary>
    private bool EnableRendering { get; set; } = true;

    public bool EnableEvents { get; set; } = true;

    private Plot Plot { get; }

    public RenderManager(Plot plot)
    {
        Plot = plot;
        RenderActions = DefaultRenderActions;
    }

    public static List<IRenderAction> DefaultRenderActions => new()
    {
        new RenderActions.PreRenderLock(),
        new RenderActions.ClearCanvas(),
        new RenderActions.ReplaceNullAxesWithDefaults(),
        new RenderActions.AutoScaleUnsetAxes(),
        new RenderActions.EnsureAxesHaveArea(),
        new RenderActions.ExecutePlottableAxisManagers(),
        new RenderActions.ApplyAxisRulesBeforeLayout(),
        new RenderActions.CalculateLayout(),
        new RenderActions.ApplyAxisRulesAfterLayout(),
        new RenderActions.RegenerateTicks(),
        new RenderActions.InvokePreRenderEvent(),
        new RenderActions.RenderBackground(),
        new RenderActions.RenderGridsBelowPlottables(),
        new RenderActions.RenderPlottables(),
        new RenderActions.RenderGridsAbovePlottables(),
        new RenderActions.RenderLegends(),
        new RenderActions.RenderPanels(),
        new RenderActions.RenderZoomRectangle(),
        new RenderActions.SyncGLPlottables(),
        new RenderActions.RenderPlottablesLast(),
        new RenderActions.RenderBenchmark(),
    };

    public void Render(SKCanvas canvas, PixelRect rect)
    {
        if (EnableRendering == false)
            return;

        IsRendering = true;
        canvas.Scale(Plot.ScaleFactor);

        List<(string, TimeSpan)> actionTimes = new();

        RenderPack rp = new(Plot, rect, canvas);

        Stopwatch sw = new();
        foreach (IRenderAction action in RenderActions)
        {
            if ((action is RenderActions.ClearCanvas) && (!ClearCanvasBeforeRendering))
            {
                continue;
            }

            sw.Restart();
            rp.Canvas.Save();
            action.Render(rp);
            rp.Canvas.Restore();
            actionTimes.Add((action.ToString() ?? string.Empty, sw.Elapsed));
        }

        LastRender = new(rp, actionTimes.ToArray(), LastRender);

        if (EnableEvents)
        {
            RenderFinished.Invoke(Plot, LastRender);

            if (LastRender.SizeChanged)
            {
                SizeChanged.Invoke(Plot, LastRender);
            }

            if (LastRender.AxisLimitsChanged)
            {
                AxisLimitsChanged.Invoke(Plot, LastRender);
            }
        }

        // TODO: event for when layout changes

        IsRendering = false;
    }
}
