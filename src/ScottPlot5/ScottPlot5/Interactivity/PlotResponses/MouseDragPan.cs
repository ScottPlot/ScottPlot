namespace ScottPlot.Interactivity.PlotResponses;

public class MouseDragPan(MouseButton button) : IPlotResponse
{
    /// <summary>
    /// Which mouse button to watch for click-drag events
    /// </summary>
    public MouseButton MouseButton { get; } = button;

    private Pixel MouseDownPixel;

    // TODO: re-implement this being more careful about allocations
    private Control.MultiAxisLimitManager? RememberedLimits = null;

    public PlotResponseResult Execute(Plot plot, IUserAction userInput, KeyboardState keys)
    {
        // mouse down starts drag
        if (userInput is IMouseButtonAction mouseDownAction
            && mouseDownAction.Button == MouseButton
            && mouseDownAction.IsPressed)
        {
            MouseDownPixel = mouseDownAction.Pixel;
            RememberedLimits = new(plot);

            return new PlotResponseResult() { Summary = $"MouseDragPan remembered current limits" };
        }

        // mouse up ends drag
        if (userInput is IMouseButtonAction mouseUpAction
            && mouseUpAction.Button == MouseButton
            && !mouseUpAction.IsPressed
            && RememberedLimits is not null)
        {
            RememberedLimits.Apply(plot);
            RememberedLimits = null;
            ApplyToPlot(plot, MouseDownPixel, mouseUpAction.Pixel, keys);

            return new PlotResponseResult() { Summary = $"MouseDragPan COMPLETED", RefreshRequired = true };
        }

        // mouse move while dragging
        if (userInput is IMouseAction mouseAction
            && RememberedLimits is not null)
        {
            // Ignore dragging if it's only a few pixels. This leaves room for
            // single- and double-click events that may drag by a few pixels accidentally.
            double dX = mouseAction.Pixel.X - MouseDownPixel.X;
            double dY = mouseAction.Pixel.Y - MouseDownPixel.Y;
            double maxDragDistance = Math.Max(dX, dY);
            if (maxDragDistance < 5)
            {
                return new PlotResponseResult()
                {
                    Summary = $"MouseDragPan moved only {maxDragDistance} pixels so not taking over",
                    IsPrimaryResponse = false,
                    RefreshRequired = false,
                };
            }

            RememberedLimits.Apply(plot);
            ApplyToPlot(plot, MouseDownPixel, mouseAction.Pixel, keys);
            return new PlotResponseResult() { Summary = $"MouseDragPan COMPLETE", IsPrimaryResponse = true, RefreshRequired = true };
        }

        return PlotResponseResult.NoActionTaken;
    }

    private static void ApplyToPlot(Plot plot, Pixel px1, Pixel px2, KeyboardState keys)
    {
        if (keys.IsPressed(StandardKeys.Shift))
        {
            px2.X = px1.X;
        }

        if (keys.IsPressed(StandardKeys.Control))
        {
            px2.Y = px1.Y;
        }

        plot.Axes.Pan(px1, px2);
    }
}
