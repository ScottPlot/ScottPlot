namespace ScottPlot.Interactivity.PlotResponses;

public class MouseDragZoom(MouseButton button) : IPlotResponse
{
    Pixel MouseDownPixel = Pixel.NaN;

    MouseButton MouseButton { get; } = button;

    // TODO: re-implement this being more careful about allocations
    Control.MultiAxisLimitManager? RememberedLimits = null;

    public PlotResponseResult Execute(Plot plot, IUserAction userInput, KeyboardState keys)
    {
        if (userInput is IMouseButtonAction mouseDownAction && mouseDownAction.Button == MouseButton && mouseDownAction.IsPressed)
        {
            MouseDownPixel = mouseDownAction.Pixel;
            RememberedLimits = new(plot);
            return new PlotResponseResult()
            {
                Summary = $"MouseDragZoom STARTED",
                IsPrimaryResponse = false,
            };
        }

        if (MouseDownPixel == Pixel.NaN)
            return PlotResponseResult.NoActionTaken;

        if (userInput is IMouseButtonAction mouseUpAction && mouseUpAction.Button == MouseButton && !mouseUpAction.IsPressed)
        {
            RememberedLimits?.Apply(plot);
            ApplyToPlot(plot, MouseDownPixel, mouseUpAction.Pixel, keys);
            MouseDownPixel = Pixel.NaN;
            return new PlotResponseResult()
            {
                Summary = $"MouseDragZoom COMPLETED",
                RefreshRequired = true,
            };
        }

        if (userInput is IMouseAction mouseMoveAction)
        {
            RememberedLimits?.Apply(plot);
            ApplyToPlot(plot, MouseDownPixel, mouseMoveAction.Pixel, keys);
            return new PlotResponseResult()
            {
                Summary = $"MouseDragZoom from {MouseDownPixel} to {mouseMoveAction.Pixel}",
                RefreshRequired = true,
                IsPrimaryResponse = true,
            };
        }

        return new PlotResponseResult()
        {
            Summary = $"MouseDragZoom is active and ignored {userInput}",
            IsPrimaryResponse = true,
        };
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

        plot.Axes.Zoom(px1, px2);
    }
}
