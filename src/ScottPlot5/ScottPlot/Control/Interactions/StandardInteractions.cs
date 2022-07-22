using ScottPlot.Control.EventArgs;

namespace ScottPlot.Control.Interactions;

public class StandardInteractions : IInteractions
{
    public IPlotControl Control { get; private set; }

    public Plot Plot => Control.Plot;

    /// <summary>
    /// The standard set of interactions used by ScottPlot.
    /// E.g., left-click-drag pan, right-click-drag zoom, middle-click auto-axis
    /// </summary>
    /// <param name="control">The plot control these actions act on (axis manipulation, redrawing, etc.)</param>
    public StandardInteractions(IPlotControl control)
    {
        Control = control;
    }

    public virtual void MouseDown(Pixel pixel, MouseButton button)
    {

    }

    public virtual void MouseUp(Pixel pixel, MouseButton button, bool endDrag)
    {
        switch (button)
        {
            case MouseButton.Middle:
                if (!endDrag)
                {
                    Plot.MouseZoomRectangleClear(applyZoom: false);
                    Plot.AutoScale();
                }
                break;
            default:
                return;
        }

        Control.Refresh();
    }

    public virtual void MouseMove(Pixel pixel)
    {

    }

    public virtual void MouseDrag(Pixel from, Pixel to, MouseButton button, IEnumerable<Key> keys, AxisLimits start)
    {
        Pixel to2 = to;
        to2.X = keys.Contains(Key.Shift) ? from.X : to2.X;
        to2.Y = keys.Contains(Key.Ctrl) ? from.Y : to2.Y;

        switch (button)
        {
            case MouseButton.Left:
                if (keys.Contains(Key.Alt))
                {
                    Plot.MouseZoomRectangle(from, to);
                }
                else
                {
                    Plot.MousePan(start, from, to2);
                }
                break;

            case MouseButton.Right:
                Plot.MouseZoom(start, from, to2);
                break;

            case MouseButton.Middle:
                Plot.MouseZoomRectangle(from, to2);
                break;

            default:
                return;
        }

        Control.Refresh();
    }

    public virtual void DoubleClick()
    {
        Plot.Benchmark.IsVisible = !Plot.Benchmark.IsVisible;
        Control.Refresh();
    }

    public virtual void MouseWheel(Pixel pixel, float delta)
    {
        double fracX = delta > 0 ? 1.15 : .85;
        double fracY = delta > 0 ? 1.15 : .85;
        Plot.MouseZoom(fracX, fracY, pixel);

        Control.Refresh();
    }

    public virtual void MouseDragEnd(MouseButton button, IEnumerable<Key> keys)
    {
        if (button == MouseButton.Middle || (button == MouseButton.Left && keys.Contains(Key.Alt)))
        {
            if (Plot.ZoomRectangle.IsVisible)
            {
                Plot.MouseZoomRectangleClear(applyZoom: true);
                Control.Refresh();
            }
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
