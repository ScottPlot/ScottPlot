using ScottPlot.Control.EventArgs;

namespace ScottPlot.Control.Interactions;

public class Default : IInteractions
{
    public IPlotControl Control { get; private set; }
    public Plot Plot => Control.Plot;

    /// <summary>
    /// asdfasdf
    /// </summary>
    /// <param name="control"></param>
    public Default(IPlotControl control)
    {
        Control = control;
    }

    public virtual void MouseDown(MouseDownEventArgs e)
    {
    }

    public virtual void MouseUp(MouseUpEventArgs e)
    {
        if (e.Handled)
            return;


        switch (e.Button)
        {
            case MouseButton.Mouse3:
                if (!e.CancelledDrag)
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

    public virtual void MouseMove(MouseMoveEventArgs e)
    {

    }

    public virtual void MouseDrag(MouseDragEventArgs e)
    {
        if (e.Handled)
            return;

        switch (e.Button)
        {
            case MouseButton.Mouse1:
                if (e.PressedKeys.Contains(Key.Alt))
                {
                    Plot.MouseZoomRectangle(e.From, e.To);
                }
                else
                {
                    Pixel panTo = e.To;
                    panTo.X = e.PressedKeys.Contains(Key.Shift) ? e.From.X : panTo.X;
                    panTo.Y = e.PressedKeys.Contains(Key.Ctrl) ? e.From.Y : panTo.Y;

                    Plot.MousePan(e.MouseDown.AxisLimits, e.From, panTo);
                }
                break;
            case MouseButton.Mouse2:
                Plot.MouseZoom(e.MouseDown.AxisLimits, e.From, e.To);
                break;
            case MouseButton.Mouse3:
                Plot.MouseZoomRectangle(e.From, e.To);
                break;
            default:
                return;
        }

        Control.Refresh();
    }

    public virtual void DoubleClick(MouseDownEventArgs e)
    {
        if (e.Handled)
            return;

        Plot.Benchmark.IsVisible = !Plot.Benchmark.IsVisible;

        Control.Refresh();
    }

    public virtual void MouseWheel(MouseWheelEventArgs e)
    {
        if (e.Handled)
            return;

        double fracX = e.DeltaY > 0 ? 1.15 : .85;
        double fracY = e.DeltaY > 0 ? 1.15 : .85;
        Plot.MouseZoom(fracX, fracY, e.Position);

        Control.Refresh();
    }

    public virtual void MouseDragEnd(MouseDragEventArgs e)
    {
        if (e.Handled)
            return;

        if (e.Button == MouseButton.Mouse3 || (e.Button == MouseButton.Mouse1 && e.PressedKeys.Contains(Key.Alt)))
        {
            Plot.MouseZoomRectangleClear(applyZoom: true);
            Control.Refresh();
        }
    }

    public virtual void KeyDown(KeyDownEventArgs e)
    {
    }

    public virtual void KeyUp(KeyUpEventArgs e)
    {
    }
}
