namespace ScottPlot.Control.Interactions;

/// <summary>
/// This class contains implementation for actions initialted by GUI interaction
/// </summary>
public class StandardInteractions : IInteractions
{
    public IPlotControl Control { get; private set; }

    public InputMap InputMap { get; set; } = InputMap.Standard();

    public Plot Plot => Control.Plot;

    public double ZoomInFraction { get; set; } = 1.15;

    public double ZoomOutFraction { get; set; } = 0.85;

    /// <summary>
    /// The standard set of interactions used by ScottPlot.
    /// E.g., left-click-drag pan, right-click-drag zoom, middle-click auto-axis
    /// </summary>
    /// <param name="control">The plot control these actions act on (axis manipulation, redrawing, etc.)</param>
    public StandardInteractions(IPlotControl control)
    {
        Control = control;
    }

    public virtual void MouseDown(Pixel pixel, MouseButton button, IEnumerable<Key> keys)
    {
        if (button == InputMap.ZoomIn)
        {
            ZoomIn(pixel, keys);
        }
        else if (button == InputMap.ZoomOut)
        {
            ZoomOut(pixel, keys);
        }
        else
        {
            return;
        }
    }

    private void ZoomIn(Pixel pixel, IEnumerable<Key> keys)
    {
        double xFrac = InputMap.ShouldLockX(keys) ? 1 : ZoomInFraction;
        double yFrac = InputMap.ShouldLockY(keys) ? 1 : ZoomInFraction;

        Plot.MouseZoom(xFrac, yFrac, pixel);
        Control.Refresh();
    }

    private void ZoomOut(Pixel pixel, IEnumerable<Key> keys)
    {
        double xFrac = InputMap.ShouldLockX(keys) ? 1 : ZoomOutFraction;
        double yFrac = InputMap.ShouldLockY(keys) ? 1 : ZoomOutFraction;

        Plot.MouseZoom(xFrac, yFrac, pixel);
        Control.Refresh();
    }

    public virtual void MouseUp(Pixel pixel, MouseButton button, bool endDrag)
    {
        if (button == InputMap.ClickDragZoomRectangle)
        {
            if (!endDrag)
            {
                Plot.MouseZoomRectangleClear(applyZoom: false);
                Plot.AutoScale();
            }
            Control.Refresh();
        }
    }

    public virtual void MouseMove(Pixel pixel)
    {

    }

    public virtual void MouseDrag(Pixel from, Pixel to, MouseButton button, IEnumerable<Key> keys, AxisLimits start)
    {
        bool lockedY = InputMap.ShouldLockY(keys);
        bool lockedX = InputMap.ShouldLockX(keys);

        Pixel to2 = new(
            x: lockedX ? from.X : to.X,
            y: lockedY ? from.Y : to.Y);

        if (InputMap.ShouldZoomRectangle(button, keys))
        {
            Plot.MouseZoomRectangle(from, to, vSpan: lockedY, hSpan: lockedX);
        }
        else if (button == InputMap.ClickDragPan)
        {
            Plot.MousePan(start, from, to2);
        }
        else if (button == InputMap.ClickDragZoom)
        {
            Plot.MouseZoom(start, from, to2);
        }

        Control.Refresh();
    }

    public virtual void DoubleClick()
    {
        Plot.Benchmark.IsVisible = !Plot.Benchmark.IsVisible;
        Control.Refresh();
    }

    public virtual void MouseDragEnd(MouseButton button, IEnumerable<Key> keys)
    {
        if (InputMap.ShouldZoomRectangle(button, keys) && Plot.ZoomRectangle.IsVisible)
        {
            Plot.MouseZoomRectangleClear(applyZoom: true);
            Control.Refresh();
        }
    }

    public virtual void KeyDown(Key key)
    {
        Control.Refresh();
    }

    public virtual void KeyUp(Key key)
    {
        Control.Refresh();
    }
}
