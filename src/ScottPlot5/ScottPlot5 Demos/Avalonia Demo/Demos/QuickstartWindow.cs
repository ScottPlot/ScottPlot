using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia_Demo.ViewModels.Demos;

namespace Avalonia_Demo.Demos;

public class QuickstartDemo : IDemo
{
    public string Title => "Avalonia Quickstart";
    public string Description => "Create a simple plot using the Avalonia control.";

    public Window GetWindow()
    {
        return new QuickstartWindow();
    }
}

public partial class QuickstartWindow : SimpleDemoWindow
{
    public QuickstartWindow() : base("Avalonia Quickstart")
    {

    }

    protected override void StartDemo()
    {
        AvaPlot.Plot.Add.Signal(ScottPlot.Generate.Sin());
        AvaPlot.Plot.Add.Signal(ScottPlot.Generate.Cos());
    }
}
