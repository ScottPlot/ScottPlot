namespace ScottPlot.Interactivity.UserActionResponses;

public class MouseWheelZoom(Key horizontalLockKey, Key verticalLockKey) : IUserActionResponse
{
    Key LockHorizontalKey { get; } = horizontalLockKey;
    Key LockVerticalKey { get; } = verticalLockKey;

    public ResponseInfo Execute(Plot plot, IUserAction userInput, KeyboardState keys)
    {
        if (userInput is UserActions.MouseWheelUp mouseDownInput)
        {
            double xFrac = keys.IsPressed(LockHorizontalKey) ? 1 : 1.15;
            double yFrac = keys.IsPressed(LockVerticalKey) ? 1 : 1.15;
            plot.Axes.Zoom(mouseDownInput.Pixel, xFrac, yFrac);
            return new ResponseInfo() { RefreshNeeded = true };
        }

        if (userInput is UserActions.MouseWheelDown mouseUpInput)
        {
            double xFrac = keys.IsPressed(LockHorizontalKey) ? 1 : 0.85;
            double yFrac = keys.IsPressed(LockVerticalKey) ? 1 : 0.85;
            plot.Axes.Zoom(mouseUpInput.Pixel, xFrac, yFrac);
            return new ResponseInfo() { RefreshNeeded = true };
        }

        return ResponseInfo.NoActionRequired;
    }
}
