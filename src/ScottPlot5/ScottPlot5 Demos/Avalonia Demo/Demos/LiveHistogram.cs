using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using ScottPlot;
using ScottPlot.Plottables;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor

namespace Avalonia_Demo.Demos;

public class LiveHistogramDemo : IDemo
{
    public string Title => "Live Histogram";
    public string Description => "A continuously updating histogram that expands binned counts as new values are added.";

    public Window GetWindow()
    {
        return new LiveHistogramWindow();
    }

}

public class LiveHistogramWindow : SimpleDemoWindow
{
    private DispatcherTimer Timer;

    public LiveHistogramWindow() : base("Live Histogram")
    {

    }

    protected override void StartDemo()
    {

        var histogram = Histogram.WithBinCount(count: 50, minValue: 0, maxValue: 100);
        var histogramPlot = AvaPlot.Plot.Add.Histogram(histogram);

        Timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(20) };
        Timer.Start();

        // simulate live incoming data
        Timer.Tick += (s, e) =>
        {
            int numberOfNewValues = Generate.RandomInteger(50, 100);
            double[] valuesToAdd = new double[numberOfNewValues]; // We store them here for now so we only need to take the lock once

            for (int i = 0; i < numberOfNewValues; i++)
            {
                valuesToAdd[i] = Generate.RandomNormalNumber(mean: 50, stdDev: 10);
            }

            lock (AvaPlot.Plot.Sync)
            {
                histogram.AddRange(valuesToAdd);
            }

            AvaPlot.Plot.Title($"Total: {histogram.Counts.Sum():N0}");
            AvaPlot.Plot.Axes.AutoScale();
            AvaPlot.Refresh();
        };
    }
}
