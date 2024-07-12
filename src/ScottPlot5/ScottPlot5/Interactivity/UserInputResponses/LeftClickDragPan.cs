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
            };
        }

        if (RememberedLimits is null)
        {
            return UserInputResponseResult.NoActionTaken;
        }

        if (userInput is UserInputs.MouseMove mouseMoveInput)
        {
            double dX = mouseMoveInput.Pixel.X - MouseDownPixel.X;
            double dY = mouseMoveInput.Pixel.Y - MouseDownPixel.Y;
            double maxDragDistance = Math.Max(dX, dY);
            if (maxDragDistance < 5)
            {
                return new UserInputResponseResult()
                {
                    Summary = $"left click drag pan only moved {maxDragDistance} pixels so not taking over",
                    IsPrimaryResponse = false,
                    RefreshRequired = false,
                };
            }

            RememberedLimits.Apply(plot);
            ApplyToPlot(plot, MouseDownPixel, mouseMoveInput.Pixel, keys);

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
            ApplyToPlot(plot, MouseDownPixel, mouseUpInput.Pixel, keys);
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

        plot.Axes.Pan(px1, px2);
    }
}
