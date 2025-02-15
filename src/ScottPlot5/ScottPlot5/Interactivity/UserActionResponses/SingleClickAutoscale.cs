namespace ScottPlot.Interactivity.UserActionResponses;

public class SingleClickAutoscale(MouseButton button) : SingleClickResponse(button, AutoScale)
{
    public static void AutoScale(IPlotControl plotControl, Pixel pixel)
    {
        Plot? plot = plotControl.GetPlotAtPixel(pixel);
        if (plot is null)
            return;

        MouseAxisManipulation.AutoScale(plot, pixel, false);
    }
}

