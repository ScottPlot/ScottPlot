using ScottPlot.Interactivity.UserInputs;

namespace ScottPlot.Interactivity.UserInputResponses;

public class RightClickContextMenu : IUserInputResponse
{
    Pixel MouseDownPixel = Pixel.NaN;

    public UserInputResponseResult Execute(Plot plot, IUserInput userInput, KeyState keys)
    {
        if (userInput is RightMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            return UserInputResponseResult.NoActionTaken;
        }

        if (userInput is RightMouseUp mouseUpInput)
        {
            if (double.IsNaN(MouseDownPixel.X))
                return UserInputResponseResult.NoActionTaken;

            // do not respond to right-click-drag
            double dX = Math.Abs(MouseDownPixel.X - mouseUpInput.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - mouseUpInput.Pixel.Y);
            double rightClickDragDistance = Math.Max(dX, dY);
            if (rightClickDragDistance >= 5)
            {
                MouseDownPixel = Pixel.NaN;
                return UserInputResponseResult.NoActionTaken;
            }

            plot.PlotControl?.ShowContextMenu(mouseUpInput.Pixel);
            MouseDownPixel = Pixel.NaN;

            return new UserInputResponseResult()
            {
                Summary = "right-click open context menu",
                RefreshRequired = true,
            };
        }

        return UserInputResponseResult.NoActionTaken;
    }
}
