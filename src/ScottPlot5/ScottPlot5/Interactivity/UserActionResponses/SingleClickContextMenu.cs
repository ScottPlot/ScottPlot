namespace ScottPlot.Interactivity.UserActionResponses;

public class SingleClickContextMenu(MouseButton button) : SingleClickResponse(button, LaunchContextMenu)
{
    public static void LaunchContextMenu(IPlotControl plotControl, Pixel pixel)
    {
        Plot? plot = plotControl.GetPlotAtPixel(pixel);
        if (plot is null)
            return;

        plot.PlotControl?.ShowContextMenu(pixel);
    }
}

