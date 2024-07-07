namespace ScottPlot.Interactivity.UserInputActions;

public class MiddleClickDragZoomRectangle : IUserInputAction
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
            return UserActionResult.Handled($"middle click zoom rectangle STARTED at {MouseDownPixel}");
        }

        if (MouseDownPixel == Pixel.NaN)
            return UserActionResult.NotRelevant();

        if (userInput is DefaultInputs.MouseMove mouseMoveInput)
        {
            double dX = Math.Abs(MouseDownPixel.X - mouseMoveInput.Pixel.X);
            double dY = Math.Abs(MouseDownPixel.Y - mouseMoveInput.Pixel.Y);
            double dragDistance = Math.Sqrt(dX * dX + dY * dY);

            if (dragDistance <= 5)
            {
                plot.ZoomRectangle.IsVisible = false;
                return UserActionResult.Handled($"middle click zoom rectangle IGNORED because drag distance was too small ({dragDistance:N2} px)");
            }

            plot.ZoomRectangle.IsVisible = true;
            plot.ZoomRectangle.MouseDown = MouseDownPixel;
            plot.ZoomRectangle.MouseUp = mouseMoveInput.Pixel;
            return UserActionResult.Refresh($"middle click zoom rectangle UPDATED");
        }

        if (userInput is DefaultInputs.MiddleMouseUp mouseUpInput)
        {
            if (plot.ZoomRectangle.IsVisible)
            {
                plot.Axes.GetAxes().OfType<IXAxis>().ToList().ForEach(plot.ZoomRectangle.Apply);
                plot.Axes.GetAxes().OfType<IYAxis>().ToList().ForEach(plot.ZoomRectangle.Apply);
                plot.ZoomRectangle.IsVisible = false;
                return UserActionResult.RefreshAndReset($"middle click zoom rectangle APPLIED");
            }
        }

        return UserActionResult.NotRelevant();
    }
}
