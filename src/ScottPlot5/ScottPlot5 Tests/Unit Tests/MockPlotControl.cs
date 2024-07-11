using SkiaSharp;

namespace ScottPlotTests;

internal class MockPlotControl : IPlotControl
{
    public int Width { get; set; } = 400;
    public int Height { get; set; } = 300;

    public Plot Plot { get; private set; }
    public MockPlotControl() => Plot = new() { PlotControl = this };

    public IPlotInteraction Interaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public IPlotMenu Menu { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public GRContext? GRContext => throw new NotImplementedException();

    public float DisplayScale { get; set; } = 1;
    public float DetectDisplayScale() => throw new NotImplementedException();

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
