using System;
using System.Windows;
using ScottPlot;

#nullable enable

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        int pointCount = 1000;
        Random rand = new(0);
        double[] ys = Generate.NoisySin(rand, pointCount);
        double[] xs = Generate.Consecutive(pointCount);

        var sp = WpfPlot.Plot.Add.Scatter(xs, ys);
        sp.LineStyle.Width = 5;
        sp.MarkerStyle = new MarkerStyle(MarkerShape.OpenSquare, 9, Colors.Red);
        sp.MarkerStyle.Outline.Width = 3;

        WpfPlot.Refresh();
    }
}
