namespace ScottPlot.Interactivity.UserInputResponses;

public class ScrollWheelZoom : IUserInputResponse
{
    public UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys)
    {
        if (userInput is UserInputs.MouseWheelUp mouseDownInput)
        {
            plot.Axes.Zoom(mouseDownInput.Pixel, 1.15);
            return new UserInputResponseResult()
            {
                Summary = $"scroll wheel zoom into {mouseDownInput.Pixel}",
                RefreshRequired = true,
            };
        }

        if (userInput is UserInputs.MouseWheelDown mouseUpInput)
        {
            plot.Axes.Zoom(mouseUpInput.Pixel, 0.85);
            return new UserInputResponseResult()
            {
                Summary = $"scroll wheel zoom away from {mouseUpInput.Pixel}",
                RefreshRequired = true,
            };
        }

        return UserInputResponseResult.NoActionTaken;
    }
}
