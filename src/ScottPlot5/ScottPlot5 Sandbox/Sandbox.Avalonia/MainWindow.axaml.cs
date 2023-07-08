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

    public MainWindow()
    {
        InitializeComponent();

        AvaPlot.Plot.Add.Signal(Generate.Sin(51));
        AvaPlot.Plot.Add.Signal(Generate.Cos(51));
    }
}
