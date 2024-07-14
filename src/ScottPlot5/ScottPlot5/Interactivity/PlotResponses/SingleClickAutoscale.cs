namespace ScottPlot.Interactivity.PlotResponses;

public class SingleClickAutoscale(MouseButton button) : SingleClickResponse(button, AutoScale)
{
    public static void AutoScale(Plot plot, Pixel pixel)
    {
        plot.Axes.AutoScale();
    }
}

