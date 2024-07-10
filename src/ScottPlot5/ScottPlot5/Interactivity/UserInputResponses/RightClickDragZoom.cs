namespace ScottPlot.Interactivity.UserInputResponses;

public class RightClickDragZoom : IUserInputResponse
{
    private Pixel MouseDownPixel = Pixel.NaN;

    // TODO: re-implement this being more careful about allocations
    private Control.MultiAxisLimitManager? RememberedLimits = null;

    public UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys)
    {
        if (userInput is UserInputs.RightMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            RememberedLimits = new(plot);
            return new UserInputResponseResult()
            {
                Summary = $"right click drag zoom STARTED",
                IsPrimaryDragResponse = true,
            };
        }

        if (MouseDownPixel == Pixel.NaN)
            return UserInputResponseResult.NoActionTaken;

        if (userInput is UserInputs.MouseMove mouseMoveInput)
        {
            RememberedLimits?.Apply(plot);
            ApplyToPlot(plot, MouseDownPixel, mouseMoveInput.Pixel, keys);
            return new UserInputResponseResult()
            {
                Summary = $"right click drag zoom in progress from {MouseDownPixel} to {mouseMoveInput.Pixel}",
                RefreshRequired = true,
                IsPrimaryDragResponse = true,
            };
        }

        if (userInput is UserInputs.RightMouseUp mouseUpInput)
        {
            RememberedLimits?.Apply(plot);
            ApplyToPlot(plot, MouseDownPixel, mouseUpInput.Pixel, keys);
            MouseDownPixel = Pixel.NaN;
            return new UserInputResponseResult()
            {
                Summary = $"right click drag zoom COMPLETED",
                RefreshRequired = true,
            };
        }

        return new UserInputResponseResult()
        {
            Summary = $"right click drag zoom ignored {userInput}",
            IsPrimaryDragResponse = true,
        };
    }

    private static void ApplyToPlot(Plot plot, Pixel px1, Pixel px2, KeyState keys)
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
