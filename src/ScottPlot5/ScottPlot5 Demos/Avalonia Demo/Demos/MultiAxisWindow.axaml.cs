using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace Avalonia_Demo.Demos;

public class MultiAxisDemo : IDemo
{
    public string Title => "Multi-Axis";
    public string Description => "Display data which visually overlaps but is plotted on different axes";

    public Window GetWindow()
    {
        return new MultiAxisWindow();
    }
}

public partial class MultiAxisWindow : Window
{
    readonly ScottPlot.IYAxis YAxis1;
    readonly ScottPlot.IYAxis YAxis2;

    public MultiAxisWindow()
    {
        InitializeComponent();

        // Store the primary Y axis so we can refer to it later
        YAxis1 = AvaPlot.Plot.Axes.Left;

        // Create a second Y axis, add it to the plot, and save it for later
        YAxis2 = AvaPlot.Plot.Axes.AddLeftAxis();

        // plot random data to start
        PlotRandomData();
    }

    private void PlotRandomData()
    {
        AvaPlot.Plot.Clear();

        double[] data1 = ScottPlot.RandomDataGenerator.Generate.RandomWalk(count: 51, mult: 1);
        var sig1 = AvaPlot.Plot.Add.Signal(data1);
        sig1.Axes.YAxis = YAxis1;
        YAxis1.Label.Text = "YAxis1";
        YAxis1.Label.ForeColor = sig1.Color;

        double[] data2 = ScottPlot.RandomDataGenerator.Generate.RandomWalk(count: 51, mult: 1000);
        var sig2 = AvaPlot.Plot.Add.Signal(data2);
        sig2.Axes.YAxis = YAxis2;
        YAxis2.Label.Text = "YAxis2";
        YAxis2.Label.ForeColor = sig2.Color;

        AvaPlot.Plot.Axes.AutoScale();
        AvaPlot.Plot.Axes.Zoom(.8, .7); // zoom out slightly
        AvaPlot.Refresh();
    }

    private void OnRandomDataClick(object sender, RoutedEventArgs e)
    {
        PlotRandomData();
    }

    private void OnManualAxisClick(object sender, RoutedEventArgs e)
    {
        AvaPlot.Plot.Axes.SetLimits(0, 50, -20, 20, AvaPlot.Plot.Axes.Bottom, YAxis1);
        AvaPlot.Plot.Axes.SetLimits(0, 50, -20_000, 20_000, AvaPlot.Plot.Axes.Bottom, YAxis2);
        AvaPlot.Refresh();
    }

    private void OnAutoscaleAxisClick(object sender, RoutedEventArgs e)
    {
        AvaPlot.Plot.Axes.Margins();
        AvaPlot.Plot.Axes.AutoScale();
        AvaPlot.Refresh();
    }

    private void OnAutoscaleTightAxisClick(object sender, RoutedEventArgs e)
    {
        AvaPlot.Plot.Axes.Margins(0, 0);
        AvaPlot.Refresh();
    }

    private void OnAutoscalePaddedAxisClick(object sender, RoutedEventArgs e)
    {
        AvaPlot.Plot.Axes.Margins(1, 1);
        AvaPlot.Refresh();
    }
}
