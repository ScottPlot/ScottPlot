using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;
using ScottPlot;
using ScottPlot.Grids;
using ScottPlot.Plottables;

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    readonly Plot _Plot;

    enum TestPlots
    {
        Lines, Scatter, Bar, Pie, Box, OHLC
    }

    public MainWindow()
    {
        InitializeComponent();
        _Plot = WpfPlot1.Plot;

        AvailablePlots = Enum.GetNames<TestPlots>();

        plotsCombo.ItemsSource = AvailablePlots;
        plotsCombo.SelectedIndex = 0;
    }

    public string[] AvailablePlots { get; set; }

    void ShowLinePlot()
    {
        _Plot.Clear();

        Signal sinSignal = _Plot.Add.Signal(Generate.Sin());
        sinSignal.LineStyle.Width = 2;
        sinSignal.Color = Colors.Black;

        Signal cosSignal = _Plot.Add.Signal(Generate.Cos());
        cosSignal.LineStyle.Width = 2;
        cosSignal.Color = Colors.Gray;

        _Plot.Style.SetFont(Fonts.Monospace);
        _Plot.Style.SetLineStylePatterns(LinePattern.HandDrawn);

        WpfPlot1.Refresh();
    }

    void ShowScatterPlot()
    {
        _Plot.Clear();

        double[] dataX = { 1, 2, 3, 4, 5 };
        double[] dataY = { 1, 4, 9, 16, 25 };
        var scatterPlot = _Plot.Add.Scatter(dataX, dataY);

        _Plot.Style.SetLineStylePatterns(LinePattern.HandDrawn);

        _Plot.Axes.Margins();

        WpfPlot1.Refresh();
    }

    void ShowBarPlot()
    {
        _Plot.Clear();

        // add bars
        double[] values = { 5, 10, 7, 13 };
        var barPlot = _Plot.Add.Bars(values);
        
        _Plot.Style.SetLineStylePatterns(LinePattern.HandDrawn);

        // tell the plot to autoscale with no padding beneath the bars
        _Plot.Axes.Margins(bottom: 0);
        
        WpfPlot1.Refresh();
    }

    void ShowPiePlot()
    {
        _Plot.Clear();

        double[] values = { 5, 2, 8, 4, 8 };
        var pie = _Plot.Add.Pie(values);
        pie.ExplodeFraction = .1;

        _Plot.Style.SetLineStylePatterns(LinePattern.HandDrawn);

        _Plot.Axes.Margins();

        WpfPlot1.Refresh();
    }

    private void plotsCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        string? selected = plotsCombo.SelectedItem as string;
        if (selected != null)
        {
            if (Enum.TryParse<TestPlots>(selected, out TestPlots plotSelected))
            {
                switch (plotSelected)
                {
                    case TestPlots.Lines:
                        ShowLinePlot();
                        break;
                    case TestPlots.Scatter:
                        ShowScatterPlot();
                        break;
                    case TestPlots.Bar:
                        ShowBarPlot();
                        break;
                    case TestPlots.Pie:
                        ShowPiePlot();
                        break;
                    case TestPlots.Box:
                        ShowBoxPlot();
                        break;
                    case TestPlots.OHLC:
                        ShowOHLCPlot();
                        break;
                    default:
                        break;
                }
            }
 
        }
    }

    private void ShowOHLCPlot()
    {
        _Plot.Clear();

        var prices = Generate.RandomOHLCs(30);
        _Plot.Add.OHLC(prices);
        _Plot.Axes.DateTimeTicksBottom();

        _Plot.Style.SetFont(Fonts.Monospace);
        _Plot.Style.SetLineStylePatterns(LinePattern.HandDrawn);

        _Plot.Axes.AutoScale();
        WpfPlot1.Refresh();
    }

    private void ShowBoxPlot()
    {
        _Plot.Clear();

        List<Box> boxes1 = new()
        {
            Generate.RandomBox(1),
            Generate.RandomBox(2),
            Generate.RandomBox(3),
        };

        List<Box> boxes2 = new() 
        {
            Generate.RandomBox(5),
            Generate.RandomBox(6),
            Generate.RandomBox(7),
        };

        var bp1 = _Plot.Add.Boxes(boxes1);
        bp1.Label = "Group 1";

        var bp2 = _Plot.Add.Boxes(boxes2);
        bp2.Label = "Group 2";

        _Plot.ShowLegend(Alignment.UpperRight);

        _Plot.Axes.AutoScale();

        _Plot.Style.SetFont(Fonts.Monospace);
        _Plot.Style.SetLineStylePatterns(LinePattern.HandDrawn);

        WpfPlot1.Refresh();
    }
}
