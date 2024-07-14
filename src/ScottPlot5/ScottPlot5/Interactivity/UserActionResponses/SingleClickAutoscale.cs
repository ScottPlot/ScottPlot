namespace ScottPlot.Interactivity.UserActionResponses;

public class SingleClickAutoscale(MouseButton button) : SingleClickResponse(button, AutoScale)
{
    public static void AutoScale(Plot plot, Pixel pixel)
    {
        plot.Axes.AutoScale();
    }
}

