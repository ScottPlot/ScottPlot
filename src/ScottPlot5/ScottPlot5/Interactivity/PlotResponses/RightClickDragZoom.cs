namespace ScottPlot.Interactivity.PlotResponses;

public class RightClickDragZoom : IPlotResponse
{
    private Pixel MouseDownPixel = Pixel.NaN;

    // TODO: re-implement this being more careful about allocations
    private Control.MultiAxisLimitManager? RememberedLimits = null;

    public PlotResponseResult Execute(Plot plot, IUserAction userInput, KeyboardState keys)
    {
        if (userInput is UserActions.RightMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            RememberedLimits = new(plot);
            return new PlotResponseResult()
            {
                Summary = $"right click drag zoom STARTED",
                IsPrimaryResponse = false,
            };
        }

        if (MouseDownPixel == Pixel.NaN)
            return PlotResponseResult.NoActionTaken;

        if (userInput is UserActions.MouseMove mouseMoveInput)
        {
            RememberedLimits?.Apply(plot);
            ApplyToPlot(plot, MouseDownPixel, mouseMoveInput.Pixel, keys);
            return new PlotResponseResult()
            {
                Summary = $"right click drag zoom in progress from {MouseDownPixel} to {mouseMoveInput.Pixel}",
                RefreshRequired = true,
                IsPrimaryResponse = true,
            };
        }

        if (userInput is UserActions.RightMouseUp mouseUpInput)
        {
            RememberedLimits?.Apply(plot);
            ApplyToPlot(plot, MouseDownPixel, mouseUpInput.Pixel, keys);
            MouseDownPixel = Pixel.NaN;
            return new PlotResponseResult()
            {
                Summary = $"right click drag zoom COMPLETED",
                RefreshRequired = true,
            };
        }

        return new PlotResponseResult()
        {
            Summary = $"right click drag zoom ignored {userInput}",
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
