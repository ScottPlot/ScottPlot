namespace ScottPlot.Interactivity.PlotResponses;

public class ScrollWheelZoom : IPlotResponse
{
    public PlotResponseResult Execute(Plot plot, IUserAction userInput, KeyState keys)
    {
        if (userInput is UserActions.MouseWheelUp mouseDownInput)
        {
            double xFrac = keys.IsPressed(StandardKeys.Shift) ? 1 : 1.15;
            double yFrac = keys.IsPressed(StandardKeys.Control) ? 1 : 1.15;
            plot.Axes.Zoom(mouseDownInput.Pixel, xFrac, yFrac);
            return new PlotResponseResult()
            {
                Summary = $"scroll wheel zoom into {mouseDownInput.Pixel}",
                RefreshRequired = true,
            };
        }

        if (userInput is UserActions.MouseWheelDown mouseUpInput)
        {
            double xFrac = keys.IsPressed(StandardKeys.Shift) ? 1 : 0.85;
            double yFrac = keys.IsPressed(StandardKeys.Control) ? 1 : 0.85;
            plot.Axes.Zoom(mouseUpInput.Pixel, xFrac, yFrac);
            return new PlotResponseResult()
            {
                Summary = $"scroll wheel zoom away from {mouseUpInput.Pixel}",
                RefreshRequired = true,
            };
        }

        return PlotResponseResult.NoActionTaken;
    }
}
