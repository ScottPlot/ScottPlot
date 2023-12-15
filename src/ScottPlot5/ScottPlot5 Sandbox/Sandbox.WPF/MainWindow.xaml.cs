using System.Windows;
using ScottPlot;

#nullable enable

namespace Sandbox.WPF;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Loaded += (s, e) =>
        {
            double[] data1 = ScottPlot.RandomDataGenerator.Generate.RandomWalk(count: 51, mult: 1);
            var sig1 = WpfPlot1.Plot.Add.Signal(data1);
            sig1.Label = "Signal 1";
            sig1.Axes.YAxis = WpfPlot1.Plot.LeftAxis;
            sig1.Axes.YAxis.Label.Text = "YAxis1";
            sig1.Axes.YAxis.Label.Font.Color = sig1.LineStyle.Color;
            sig1.IsVisible = false;

            double[] data2 = ScottPlot.RandomDataGenerator.Generate.RandomWalk(count: 51, mult: 1000);
            var sig2 = WpfPlot1.Plot.Add.Signal(data2);
            sig2.Label = "Signal 2";
            sig2.Axes.YAxis = WpfPlot1.Plot.RightAxis;
            sig2.Axes.YAxis.Label.Text = "YAxis2";
            sig2.Axes.YAxis.Label.Font.Color = sig2.LineStyle.Color;

            /*
            WpfPlot1.Plot.Add.Signal(Generate.Sin());
            WpfPlot1.Plot.Add.Signal(Generate.Cos());
            */
            WpfPlot1.Plot.AutoScale();
            WpfPlot1.Plot.Legend.IsVisible = true;
            var legenImage = new Image(WpfPlot1.Plot.GetLegendImage()!);
            legenImage.SavePng("test.png");
            legenImage.Dispose();
            WpfPlot1.Plot.SaveLegendAsSvg("test.svg");
            WpfPlot1.Refresh();

            WpfPlot2.Plot.Add.Signal(Generate.Sin());
            WpfPlot2.Plot.Add.Signal(Generate.Cos());
            WpfPlot2.Plot.AutoScale();
            WpfPlot2.Refresh();
        };
    }
}
