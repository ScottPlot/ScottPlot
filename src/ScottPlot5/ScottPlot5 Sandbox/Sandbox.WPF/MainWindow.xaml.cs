using System;
using System.Windows;
using ScottPlot;

#pragma warning disable CA1416 // Validate platform compatibility

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        int pointCount = 100_000;
        double[] ys = Generate.NoisySin(Random.Shared, pointCount);
        double[] xs = Generate.Consecutive(pointCount);

        var sp = WpfPlot.Plot.Add.ScatterGLCustom(WpfPlot, xs, ys);
        sp.LineStyle.Width = 5;
        sp.MarkerStyle = new MarkerStyle(MarkerShape.OpenSquare, 9, Colors.Red);
        sp.MarkerStyle.Outline.Width = 3;

        WpfPlot.Refresh();
    }
}
