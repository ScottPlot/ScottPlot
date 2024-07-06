namespace ScottPlot.Interactivity.UserInputActions;

public class LeftClickDragPan : IUserInputAction
{
    public UserActionResult Execute(Plot plot, UserInputQueue queue)
    {
        if (queue.Events.Last() is DefaultInputs.LeftMouseDown leftDownInput)
        {
            return new UserActionResult($"left click drag may have started at {leftDownInput.Pixel}", startTrackingMouse: true);
        }

        if (queue.Events.Last() is DefaultInputs.LeftMouseUp lastLeftMouseUp)
        {
            var leftDownInputs = queue.Events.OfType<DefaultInputs.LeftMouseDown>();
            if (!leftDownInputs.Any())
            {
                return UserActionResult.NoAction;
            }

            Pixel downPixel = leftDownInputs.Last().Pixel;
            Pixel upPixel = lastLeftMouseUp.Pixel;

            return new UserActionResult($"left click drag pan completed from {downPixel} to {upPixel}", clearPreviousEvents: true);
        }

        return UserActionResult.NoAction;
    }
}
