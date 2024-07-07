namespace ScottPlot.Interactivity.UserInputActions;

public class RightClickDragZoom : IUserInputAction
{
    private Pixel MouseDownPixel;

    // TODO: re-implement this being more careful about allocations
    private Control.MultiAxisLimitManager? RememberedLimits = null;

    public void Reset()
    {
        MouseDownPixel = Pixel.NaN;
        RememberedLimits = null;
    }

    public UserActionResult Execute(Plot plot, IUserInput userInput)
    {
        if (userInput is DefaultInputs.RightMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            RememberedLimits = new(plot);
            return UserActionResult.Handled($"left click drag zoom STARTED");
        }

        if (MouseDownPixel == Pixel.NaN)
            return UserActionResult.NotRelevant();

        if (userInput is DefaultInputs.MouseMove mouseMoveInput)
        {
            RememberedLimits?.Apply(plot);
            plot.Axes.Zoom(MouseDownPixel, mouseMoveInput.Pixel);
            return UserActionResult.Refresh($"left click zoom in progress from {MouseDownPixel} to {mouseMoveInput.Pixel}");
        }

        if (userInput is DefaultInputs.RightMouseUp mouseUpInput)
        {
            RememberedLimits?.Apply(plot);
            plot.Axes.Zoom(MouseDownPixel, mouseUpInput.Pixel);
            return UserActionResult.RefreshAndReset($"left click drag zoom COMPLETED");
        }

        return UserActionResult.NotRelevant();
    }
}
