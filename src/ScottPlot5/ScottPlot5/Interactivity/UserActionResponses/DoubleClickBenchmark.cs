﻿namespace ScottPlot.Interactivity.UserActionResponses;

public class DoubleClickBenchmark(MouseButton button) : DoubleClickResponse(button, Autoscale)
{
    public static void Autoscale(Plot plot, Pixel pixel)
    {
        plot.Benchmark.IsVisible = !plot.Benchmark.IsVisible;
    }
}
