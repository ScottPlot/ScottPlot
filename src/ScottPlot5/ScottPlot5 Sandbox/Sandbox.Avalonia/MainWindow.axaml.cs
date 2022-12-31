using Avalonia;
using Avalonia.Controls;
using ScottPlot.Avalonia;
using ScottPlot;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Sandbox.Avalonia;

public partial class MainWindow : Window
{
    private readonly AvaPlot avaPlot;

    public MainWindow()
    {
        InitializeComponent();
        avaPlot = this.Find<AvaPlot>("AvaPlot");

        avaPlot.Plot.Add.Signal(Generate.Sin(51));
        avaPlot.Plot.Add.Signal(Generate.Cos(51));
    }

    private void InitializeComponent()
    {
        Width = 800;
        Height = 450;
        WindowStartupLocation = WindowStartupLocation.CenterScreen;
        AvaloniaXamlLoader.Load(this);
    }
}
