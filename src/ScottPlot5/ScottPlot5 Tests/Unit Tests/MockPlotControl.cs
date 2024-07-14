using SkiaSharp;

namespace ScottPlotTests;

internal class MockPlotControl : IPlotControl
{
    public int Width { get; set; } = 400;
    public int Height { get; set; } = 300;

    public Plot Plot { get; private set; }
    public GRContext? GRContext => null;

    public IPlotInteraction Interaction { get; set; }
    public ScottPlot.Interactivity.UserInputProcessor UserInputProcessor { get; }

    public IPlotMenu Menu // TODO: mock menu
    {
        get => throw new NotImplementedException();
        set => throw new NotImplementedException();
    } 

    public MockPlotControl()
    {
        Plot = new() { PlotControl = this };
        Interaction = new ScottPlot.Control.Interaction(this);
        UserInputProcessor = new(Plot);
    }

    public float DisplayScale { get; set; } = 1;
    public float DetectDisplayScale() => DisplayScale;

    public int RefreshCount { get; private set; } = 0;
    public void Refresh()
    {
        RefreshCount += 1;
        Plot.RenderInMemory(Width, Height);
    }

    public void Reset()
    {
        Reset(new Plot() { PlotControl = this });
    }

    public void Reset(Plot plot)
    {
        Plot oldPlot = Plot;
        Plot = plot;
        Plot.PlotControl = this;
        oldPlot.Dispose();
    }

    public int ContextMenuLaunchCount { get; private set; } = 0;
    public void ShowContextMenu(Pixel position) => ContextMenuLaunchCount += 1;
}
