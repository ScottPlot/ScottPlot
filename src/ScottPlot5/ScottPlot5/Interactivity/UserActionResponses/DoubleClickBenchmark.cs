namespace ScottPlot.Interactivity.UserActionResponses;

public class DoubleClickBenchmark(MouseButton button) : DoubleClickResponse(button, ToggleBenchmarkVisibility)
{
    public static void ToggleBenchmarkVisibility(Plot plot, Pixel pixel)
    {
        plot.Benchmark.IsVisible = !plot.Benchmark.IsVisible;
    }
}
