namespace ScottPlot.Interactivity.PlotResponses;

public class MouseClickAutoscale : IPlotResponse
{
    private Pixel MouseDownPixel = Pixel.NaN;
    public MouseButton MouseButton { get; set; } = StandardMouseButtons.Middle;

    public PlotResponseResult Execute(Plot plot, IUserAction userAction, KeyboardState keys)
    {
        if (userAction is IMouseButtonAction buttonAction)
        {
            if (buttonAction.Button != MouseButton)
                return PlotResponseResult.NoActionTaken;

            if (buttonAction.IsPressed)
            {
                MouseDownPixel = buttonAction.Pixel;
                return new PlotResponseResult() { Summary = $"middle click AutoScale STARTED at {MouseDownPixel}" };
            }

            if (MouseDownPixel == Pixel.NaN)
                return PlotResponseResult.NoActionTaken;

            double dX = Math.Abs(MouseDownPixel.X - buttonAction.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - buttonAction.Pixel.Y);

            MouseDownPixel = Pixel.NaN;

            if (dX > 5 || dY > 5)
                return PlotResponseResult.NoActionTaken;

            plot.Axes.AutoScale();
            return new PlotResponseResult() { Summary = $"AutoScale APPLIED", RefreshRequired = true };
        }


        return PlotResponseResult.NoActionTaken;
    }
}

