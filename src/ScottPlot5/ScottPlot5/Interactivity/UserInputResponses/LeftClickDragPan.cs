namespace ScottPlot.Interactivity.UserInputResponses;

public class LeftClickDragPan : IUserInputResponse
{
    private Pixel MouseDownPixel;

    // TODO: re-implement this being more careful about allocations
    private Control.MultiAxisLimitManager? RememberedLimits = null;

    public UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys)
    {
        if (userInput is UserInputs.LeftMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            RememberedLimits = new(plot);

            return new UserInputResponseResult()
            {
                Summary = $"left click drag pan STARTED",
                IsPrimaryResponse = true,
            };
        }

        if (RememberedLimits is null)
        {
            return UserInputResponseResult.NoActionTaken;
        }

        if (userInput is UserInputs.MouseMove mouseMoveInput)
        {
            RememberedLimits.Apply(plot);
            plot.Axes.Pan(MouseDownPixel, mouseMoveInput.Pixel);

            return new UserInputResponseResult()
            {
                Summary = $"left click drag pan in progress from {MouseDownPixel} to {mouseMoveInput.Pixel}",
                IsPrimaryResponse = true,
                RefreshRequired = true,
            };
        }

        if (userInput is UserInputs.LeftMouseUp mouseUpInput)
        {
            RememberedLimits.Apply(plot);
            RememberedLimits = null;
            plot.Axes.Pan(MouseDownPixel, mouseUpInput.Pixel);
            return new UserInputResponseResult()
            {
                Summary = $"left click drag pan COMPLETED",
                RefreshRequired = true,
            };
        }

        return new UserInputResponseResult()
        {
            Summary = $"left click drag pan ignored {userInput}",
            IsPrimaryResponse = true,
        };
    }
}
