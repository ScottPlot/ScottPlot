namespace ScottPlot.Interactivity.PlotResponses;

public class MiddleClickAutoscale : IPlotResponse
{
    private Pixel MouseDownPixel = Pixel.NaN;

    public PlotResponseResult Execute(Plot plot, IUserAction userInput, KeyState keys)
    {
        if (userInput is UserActions.MiddleMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            return new PlotResponseResult()
            {
                Summary = $"middle click AutoScale STARTED at {MouseDownPixel}",
            };
        }

        if (MouseDownPixel == Pixel.NaN)
            return PlotResponseResult.NoActionTaken;

        if (userInput is UserActions.MiddleMouseUp mouseUpInput)
        {
            double dX = Math.Abs(MouseDownPixel.X - mouseUpInput.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - mouseUpInput.Pixel.Y);

            MouseDownPixel = Pixel.NaN;

            if (dX > 5 || dY > 5)
            {
                return new PlotResponseResult()
                {
                    Summary = $"middle click AutoScale ABORTED because drag distance was large",
                };
            }

            plot.Axes.AutoScale();
            return new PlotResponseResult()
            {
                Summary = $"middle click AutoScale APPLIED",
                RefreshRequired = true,
            };
        }

        return PlotResponseResult.NoActionTaken;
    }
}

