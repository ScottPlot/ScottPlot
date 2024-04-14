using System.Windows;

namespace WPF_Demo.DemoWindows;

public partial class Quickstart : Window, IDemoWindow
{
    public string DemoTitle => "WPF Quickstart";
    public string Description => "Create a simple plot using the WPF control.";

    public Quickstart()
    {
        InitializeComponent();

        WpfPlot1.Plot.Add.Signal(ScottPlot.Generate.Sin());
        WpfPlot1.Plot.Add.Signal(ScottPlot.Generate.Cos());
    }
}
