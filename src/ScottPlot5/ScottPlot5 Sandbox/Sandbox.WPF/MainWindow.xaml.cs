using System;
using System.Windows;
using ScottPlot;

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        //WpfPlot1.Plot.Add.Signal(Generate.Sin());
        //WpfPlot1.Plot.Add.Signal(Generate.Cos());

        double[,] intensityMap = SampleData.MonaLisa();

        ScottPlot.Range forcedRange = new(50, 150);
        var map = WpfPlot1.Plot.Add.Heatmap(intensityMap, forcedRange);
        map.Colormap = new ScottPlot.Colormaps.Turbo();
        WpfPlot1.Plot.Add.ColorBar(map);
    }
}
