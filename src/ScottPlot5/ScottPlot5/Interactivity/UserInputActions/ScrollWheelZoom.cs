namespace ScottPlot.Interactivity.UserInputActions;

public class ScrollWheelZoom : IUserInputAction
{
    public void Reset() { }

    public UserActionResult Execute(Plot plot, IUserInput userInput)
    {
        if (userInput is DefaultInputs.MouseWheelUp mouseDownInput)
        {
            plot.Axes.Zoom(mouseDownInput.Pixel, 1.15);
            return UserActionResult.RefreshAndReset($"scroll wheel zoom into {mouseDownInput.Pixel}");
        }
        
        if (userInput is DefaultInputs.MouseWheelDown mouseUpInput)
        {
            plot.Axes.Zoom(mouseUpInput.Pixel, 0.85);
            return UserActionResult.RefreshAndReset($"scroll wheel zoom away from {mouseUpInput.Pixel}");
        }

        return UserActionResult.NotRelevant();
    }
}
