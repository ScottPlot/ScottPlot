namespace ScottPlot.Control;

// TODO: give this an interface so users can inject their own

/// <summary>
/// This class contains methods which apply UI interactions to a Plot in a Control.
/// </summary>
public class PlotActions
{
    private double ZoomInFraction { get; set; } = 1.15;

    private double ZoomOutFraction { get; set; } = 0.85;

    private double PanFraction { get; set; } = 0.1;

    private readonly IPlotControl Control;

    public PlotActions(IPlotControl control)
    {
        Control = control;
    }

    public void ZoomIn(Pixel pixel, bool lockX, bool lockY)
    {
        double xFrac = lockX ? 1 : ZoomInFraction;
        double yFrac = lockY ? 1 : ZoomInFraction;

        Control.Plot.MouseZoom(xFrac, yFrac, pixel);
        Control.Refresh();
    }

    public void ZoomOut(Pixel pixel, bool lockX, bool lockY)
    {
        double xFrac = lockX ? 1 : ZoomOutFraction;
        double yFrac = lockY ? 1 : ZoomOutFraction;

        Control.Plot.MouseZoom(xFrac, yFrac, pixel);
        Control.Refresh();
    }

    public void PanUp()
    {
        AxisLimits limits = Control.Plot.GetAxisLimits();
        double deltaY = limits.Rect.Height * PanFraction;
        Control.Plot.SetAxisLimits(limits.WithPan(0, deltaY));
        Control.Refresh();
    }

    public void PanDown()
    {
        AxisLimits limits = Control.Plot.GetAxisLimits();
        double deltaY = limits.Rect.Height * PanFraction;
        Control.Plot.SetAxisLimits(limits.WithPan(0, -deltaY));
        Control.Refresh();
    }

    public void PanLeft()
    {
        AxisLimits limits = Control.Plot.GetAxisLimits();
        double deltaX = limits.Rect.Width * PanFraction;
        Control.Plot.SetAxisLimits(limits.WithPan(-deltaX, 0));
        Control.Refresh();
    }

    public void PanRight()
    {
        AxisLimits limits = Control.Plot.GetAxisLimits();
        double deltaX = limits.Rect.Width * PanFraction;
        Control.Plot.SetAxisLimits(limits.WithPan(deltaX, 0));
        Control.Refresh();
    }

    public void DragPan(AxisLimits start, Pixel from, Pixel to, bool lockX, bool lockY)
    {
        Pixel to2 = new(
            x: lockX ? from.X : to.X,
            y: lockY ? from.Y : to.Y);

        Control.Plot.MousePan(start, from, to2);
        Control.Refresh();
    }

    public void DragZoom(AxisLimits start, Pixel from, Pixel to, bool lockX, bool lockY)
    {
        Pixel to2 = new(
            x: lockX ? from.X : to.X,
            y: lockY ? from.Y : to.Y);

        Control.Plot.MouseZoom(start, from, to2);
        Control.Refresh();
    }

    public void ZoomRectangleClear()
    {
        Control.Plot.MouseZoomRectangleClear(applyZoom: false);
        Control.Refresh();
    }

    public void ZoomRectangleApply()
    {
        Control.Plot.MouseZoomRectangleClear(applyZoom: true);
        Control.Refresh();
    }

    public void ZoomRectangle(Pixel from, Pixel to, bool lockX, bool lockY)
    {
        Control.Plot.MouseZoomRectangle(from, to, vSpan: lockY, hSpan: lockX);
        Control.Refresh();
    }

    public void ToggleBenchmark()
    {
        Control.Plot.Benchmark.IsVisible = !Control.Plot.Benchmark.IsVisible;
        Control.Refresh();
    }

    public void AutoScale()
    {
        Control.Plot.AutoScale();
        Control.Refresh();
    }
}
