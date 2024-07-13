using ScottPlot.Interactivity.UserActions;

namespace ScottPlot.Interactivity.PlotResponses;

public class RightClickContextMenu : IPlotResponse
{
    Pixel MouseDownPixel = Pixel.NaN;

    public PlotResponseResult Execute(Plot plot, IUserAction userInput, KeyboardState keys)
    {
        if (userInput is RightMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            return PlotResponseResult.NoActionTaken;
        }

        if (userInput is RightMouseUp mouseUpInput)
        {
            if (double.IsNaN(MouseDownPixel.X))
                return PlotResponseResult.NoActionTaken;

            // do not respond to right-click-drag
            double dX = Math.Abs(MouseDownPixel.X - mouseUpInput.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - mouseUpInput.Pixel.Y);
            double rightClickDragDistance = Math.Max(dX, dY);
            if (rightClickDragDistance >= 5)
            {
                MouseDownPixel = Pixel.NaN;
                return PlotResponseResult.NoActionTaken;
            }

            plot.PlotControl?.ShowContextMenu(mouseUpInput.Pixel);
            MouseDownPixel = Pixel.NaN;

            return new PlotResponseResult()
            {
                Summary = "right-click open context menu",
                RefreshRequired = true,
            };
        }

        return PlotResponseResult.NoActionTaken;
    }
}
