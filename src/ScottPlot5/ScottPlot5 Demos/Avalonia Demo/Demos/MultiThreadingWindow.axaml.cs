using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System;
using Avalonia.Threading;
using ScottPlot;

namespace Avalonia_Demo.Demos;

public class MultiThreadingDemo: IDemo
{
    public string DemoTitle => "Avalonia Multi-Threading";
    public string Description => "Demonstrate how to safely change data while rendering asynchronously.";

    public Window GetWindow()
    {
        return new MultiThreadingWindow();
    }
}

public partial class MultiThreadingWindow : Window
{
    System.Timers.Timer SystemTimer = new() { Interval = 10 };
    private readonly DispatcherTimer DispatcherTimer = new() { Interval = TimeSpan.FromMilliseconds(10) };

    private readonly List<double> Xs = [];
    private readonly List<double> Ys = [];

    public MultiThreadingWindow()
    {
        InitializeComponent();

        // pre-populate lists with valid data
        ChangeDataLength();

        // add the scatter plot
        AvaPlot.Plot.Add.ScatterLine(Xs, Ys);

        SystemTimer.Elapsed += (s, e) =>
        {
            // Changing data length will throw an exception if it occurs mid-render.
            // Operations performed while the sync object will occur outside renders.
            lock (AvaPlot.Plot.Sync)
            {
                ChangeDataLength();
            }
            AvaPlot.Refresh();
        };

        DispatcherTimer.Tick += (s, e) =>
        {
            // Changing data length will throw an exception if it occurs mid-render.
            // Operations performed while the sync object will occur outside renders.
            lock (AvaPlot.Plot.Sync)
            {
                ChangeDataLength();
            }
            AvaPlot.Refresh();
        };

    }

    private void ChangeDataLength(int minLength = 10_000, int maxLength = 20_000)
    {
        int newLength = Random.Shared.Next(minLength, maxLength);
        Xs.Clear();
        Ys.Clear();
        Xs.AddRange(Generate.Consecutive(newLength));
        Ys.AddRange(Generate.RandomWalk(newLength));
        AvaPlot.Plot.Axes.AutoScale(true);
    }

    private void StartTimer(object sender, RoutedEventArgs e)
    {
        SystemTimer.Start();
        ButtonStackPanel.IsEnabled = false;
    }

    private void StartDispatchTimer(object sender, RoutedEventArgs e)
    {
        DispatcherTimer.Start();
        ButtonStackPanel.IsEnabled = false;
    }
}
