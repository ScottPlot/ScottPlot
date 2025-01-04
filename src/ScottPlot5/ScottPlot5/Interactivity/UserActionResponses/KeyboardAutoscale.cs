namespace ScottPlot.Interactivity.UserActionResponses;

internal class KeyboardAutoscale(Key key) : KeyPressResponse(key, AutoScale)
{
    public static void AutoScale(IPlotControl plotControl, Pixel pixel)
    {
        Plot? plot = plotControl.GetPlotAtPixel(pixel);
        if (plot is null)
            return;

        plot.Axes.AutoScale();
    }
}
