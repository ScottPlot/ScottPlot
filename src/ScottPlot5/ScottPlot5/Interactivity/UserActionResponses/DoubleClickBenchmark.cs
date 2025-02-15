namespace ScottPlot.Interactivity.UserActionResponses;

public class DoubleClickBenchmark(MouseButton button) : DoubleClickResponse(button, ToggleBenchmarkVisibility)
{
    public static void ToggleBenchmarkVisibility(IPlotControl plotControl, Pixel pixel)
    {
        Plot? plot = plotControl.GetPlotAtPixel(pixel);
        if (plot is null)
            return;

        plot.Benchmark.IsVisible = !plot.Benchmark.IsVisible;
    }
}
