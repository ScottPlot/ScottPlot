using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ScottPlot.AxisPanels;
using System;

namespace Avalonia_Demo.Demos;

public class DataLoggerDemo : IDemo
{
    public string Title => "Data Logger";
    public string Description => "Plots live streaming data as a growing line plot.";

    public Window GetWindow()
    {
        return new DataLoggerWindow();
    }
}

public partial class DataLoggerWindow : Window
{
    readonly DispatcherTimer AddNewDataTimer = new() { Interval = TimeSpan.FromMilliseconds(10) };
    readonly DispatcherTimer UpdatePlotTimer = new() { Interval = TimeSpan.FromMilliseconds(50) };

    readonly ScottPlot.Plottables.DataLogger Logger1;
    readonly ScottPlot.Plottables.DataLogger Logger2;

    readonly ScottPlot.DataGenerators.RandomWalker Walker1 = new(0, multiplier: 0.01);
    readonly ScottPlot.DataGenerators.RandomWalker Walker2 = new(1, multiplier: 1000);

    public DataLoggerWindow()
    {
        InitializeComponent();

        // disable interactivity by default
        AvaPlot.UserInputProcessor.Disable();

        // create two loggers and add them to the plot
        Logger1 = AvaPlot.Plot.Add.DataLogger();
        Logger2 = AvaPlot.Plot.Add.DataLogger();

        Logger1.ViewFull();
        Logger2.ViewFull();

        // use the right axis (already there by default) for the first logger
        RightAxis axis1 = (RightAxis)AvaPlot.Plot.Axes.Right;
        Logger1.Axes.YAxis = axis1;
        axis1.Color(Logger1.Color);

        // create and add a secondary right axis to use for the other logger
        RightAxis axis2 = AvaPlot.Plot.Axes.AddRightAxis();
        Logger2.Axes.YAxis = axis2;
        axis2.Color(Logger2.Color);

        AddNewDataTimer.Tick += (s, e) =>
        {
            int count = 5;

            lock (AvaPlot.Plot.Sync)
            {
                Logger1.Add(Walker1.Next(count));
                Logger2.Add(Walker2.Next(count));
            }
        };

        UpdatePlotTimer.Tick += (s, e) =>
        {
            if (Logger1.HasNewData || Logger2.HasNewData)
                AvaPlot.Refresh();
        };

        AddNewDataTimer.Start();
        UpdatePlotTimer.Start();
    }

    public void OnFullButtonClicked(object? sender, RoutedEventArgs e)
    {
        Logger1.ViewFull();
        Logger2.ViewFull();
    }

    public void OnJumpButtonClicked(object? sender, RoutedEventArgs e)
    {
        Logger1.ViewJump();
        Logger2.ViewJump();
    }

    public void OnSlideButtonClicked(object? sender, RoutedEventArgs e)
    {
        Logger1.ViewSlide();
        Logger2.ViewSlide();
    }
}
