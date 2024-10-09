using ScottPlot;
using System.Windows;

namespace WPF_Demo.DemoWindows;

public partial class Quickstart : Window, IDemoWindow
{
    public string DemoTitle => "WPF Quickstart";
    public string Description => "Create a simple plot using the WPF control.";

    public Quickstart()
    {
        InitializeComponent();

        LabelStyle.RTLSupport = true;
        var sig1 = WpfPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin());
        var sig2 = WpfPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos());
        sig1.LegendText = "אמת";
        sig2.LegendText = "Engish";
        WpfPlot1.Plot.Legend.FontSize = 40;
        WpfPlot1.Plot.ShowLegend();
    }
}
