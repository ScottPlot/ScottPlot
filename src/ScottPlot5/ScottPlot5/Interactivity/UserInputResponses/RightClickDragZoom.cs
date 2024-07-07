namespace ScottPlot.Interactivity.UserInputResponses;

public class RightClickDragZoom : IUserInputResponse
{
    private Pixel MouseDownPixel = Pixel.NaN;

    // TODO: re-implement this being more careful about allocations
    private Control.MultiAxisLimitManager? RememberedLimits = null;

    public UserInputResponseResult Execute(Plot plot, IUserInput userInput)
    {
        if (userInput is DefaultInputs.RightMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            RememberedLimits = new(plot);
            return new UserInputResponseResult()
            {
                Summary = $"right click drag zoom STARTED",
                IsPrimaryResponse = true,
            };
        }

        if (MouseDownPixel == Pixel.NaN)
            return UserInputResponseResult.NoActionTaken;

        if (userInput is DefaultInputs.MouseMove mouseMoveInput)
        {
            RememberedLimits?.Apply(plot);
            plot.Axes.Zoom(MouseDownPixel, mouseMoveInput.Pixel);
            return new UserInputResponseResult()
            {
                Summary = $"right click drag zoom in progress from {MouseDownPixel} to {mouseMoveInput.Pixel}",
                RefreshRequired = true,
                IsPrimaryResponse = true,
            };
        }

        if (userInput is DefaultInputs.RightMouseUp mouseUpInput)
        {
            RememberedLimits?.Apply(plot);
            plot.Axes.Zoom(MouseDownPixel, mouseUpInput.Pixel);
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
            IsPrimaryResponse = true,
        };
    }
}
