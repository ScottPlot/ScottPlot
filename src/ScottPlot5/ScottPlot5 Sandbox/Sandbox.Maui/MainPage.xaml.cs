using ScottPlot;

namespace Sandbox.Maui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        MauiPlot1.Plot.Add.Signal(Generate.Sin());
        MauiPlot1.Plot.Add.Signal(Generate.Cos());
        MauiPlot1.Refresh();
    }
}

