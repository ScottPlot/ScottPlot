using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Avalonia_Demo.Demos;

public class QuickstartDemo: IDemo
{
    public string DemoTitle => "Avalonia Quickstart";
    public string Description => "Create a simple plot using the Avalonia control.";

    public Window GetWindow()
    {
        return new QuickstartWindow();
    }

}

public partial class QuickstartWindow : Window
{
    public QuickstartWindow()
    {
        InitializeComponent();

        AvaPlot.Plot.Add.Signal(ScottPlot.Generate.Sin());
        AvaPlot.Plot.Add.Signal(ScottPlot.Generate.Cos());
    }
}
