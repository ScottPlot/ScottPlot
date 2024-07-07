namespace ScottPlot.Interactivity.UserInputActions;

public class MiddleClickAutoscale : IUserInputAction
{
    private Pixel MouseDownPixel;

    public void Reset()
    {
        MouseDownPixel = Pixel.NaN;
    }

    public UserActionResult Execute(Plot plot, IUserInput userInput)
    {
        if (userInput is DefaultInputs.MiddleMouseDown mouseDownInput)
        {
            MouseDownPixel = mouseDownInput.Pixel;
            return UserActionResult.Handled($"middle click AutoScale STARTED at {MouseDownPixel}");
        }

        if (MouseDownPixel == Pixel.NaN)
            return UserActionResult.NotRelevant();

        if (userInput is DefaultInputs.MiddleMouseUp mouseUpInput)
        {
            double dX = Math.Abs(MouseDownPixel.X - mouseUpInput.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - mouseUpInput.Pixel.Y);
            double dragDistance = Math.Sqrt(dX * dX + dY * dY);

            if (dragDistance > 5)
            {
                MouseDownPixel = Pixel.NaN;
                return UserActionResult.Handled($"middle click AutoScale ABORTED because drag distance was large ({dragDistance:N2} px)");
            }

            plot.Axes.AutoScale();
            return UserActionResult.RefreshAndReset($"middle click AutoScale APPLIED");
        }

        return UserActionResult.NotRelevant();
    }
}

