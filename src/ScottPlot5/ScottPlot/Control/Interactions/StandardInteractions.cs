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
            case MouseButton.Mouse3:
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
        switch (button)
        {
            case MouseButton.Mouse1:
                if (keys.Contains(Key.Alt))
                {
                    Plot.MouseZoomRectangle(from, to);
                }
                else
                {
                    Pixel panTo = to;
                    panTo.X = keys.Contains(Key.Shift) ? from.X : panTo.X;
                    panTo.Y = keys.Contains(Key.Ctrl) ? from.Y : panTo.Y;

                    Plot.MousePan(start, from, to);
                }
                break;
            case MouseButton.Mouse2:
                Plot.MouseZoom(start, from, to);
                break;
            case MouseButton.Mouse3:
                Plot.MouseZoomRectangle(from, to);
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
        if (button == MouseButton.Mouse3 || (button == MouseButton.Mouse1 && keys.Contains(Key.Alt)))
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
    }

    public virtual void KeyUp(Key key)
    {
    }
}
