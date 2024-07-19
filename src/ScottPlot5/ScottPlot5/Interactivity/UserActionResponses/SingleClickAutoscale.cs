namespace ScottPlot.Interactivity.UserActionResponses;

public class SingleClickAutoscale(MouseButton button) : SingleClickResponse(button, AutoScale)
{
    public static void AutoScale(Plot plot, Pixel pixel)
    {
        MouseAxisManipulation.AutoScale(plot, pixel, false);
    }
}

