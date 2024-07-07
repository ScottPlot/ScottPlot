using ScottPlot.Control;

namespace ScottPlot.Interactivity.UserInputActions;

public class LeftClickDragPan : IUserInputAction
{
    private Pixel MouseDownPixel;

    // TODO: re-implement this being more careful about allocations
    private MultiAxisLimitManager? RememberedLimits = null;

    public void Reset()
    {
        MouseDownPixel = Pixel.NaN;
        RememberedLimits = null;
    }

    public UserActionResult Execute(Plot plot, IUserInput userInput)
    {
        if (userInput is DefaultInputs.LeftMouseDown leftDownInput)
        {
            MouseDownPixel = leftDownInput.Pixel;
            RememberedLimits = new(plot);
            return UserActionResult.Handled($"left click drag pan STARTED");
        }

        if (MouseDownPixel == Pixel.NaN)
            return UserActionResult.NotRelevant();

        if (userInput is DefaultInputs.MouseMove mouseMoveInput)
        {
            RememberedLimits?.Apply(plot);
            plot.Axes.Pan(MouseDownPixel, mouseMoveInput.Pixel);
            return UserActionResult.Refresh($"left click drag in progress from {MouseDownPixel} to {mouseMoveInput.Pixel}");
        }

        if (userInput is DefaultInputs.LeftMouseUp leftMouseUpInput)
        {
            RememberedLimits?.Apply(plot);
            plot.Axes.Pan(MouseDownPixel, leftMouseUpInput.Pixel);
            return UserActionResult.RefreshAndReset($"left click drag pan COMPLETED");
        }

        return UserActionResult.NotRelevant();
    }
}
