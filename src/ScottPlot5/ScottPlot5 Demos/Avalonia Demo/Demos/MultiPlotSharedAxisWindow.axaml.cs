using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot;

namespace Avalonia_Demo.Demos;

public class MultiPlotSharedAxisDemo : IDemo
{
    public string Title => "Multiplot with Shared Axis Limits";
    public string Description => "Updates to axis limits of a subplot may be applied to all other subplots in the multiplot.";

    public Window GetWindow()
    {
        return new MultiPlotSharedAxisWindow();
    }
}

public partial class MultiPlotSharedAxisWindow : Window
{
    public MultiPlotSharedAxisWindow()
    {
        InitializeComponent();

        // setup a multiplot with 3 subplots
        AvaPlot.Multiplot.AddPlots(3);

        // add sample data to each subplot
        foreach (Plot plot in AvaPlot.Multiplot.GetPlots())
        {
            plot.Add.Signal(Generate.AddNoise(Generate.Sin()));
        }
    }

    private void OnShareNoneClick(object? sender, RoutedEventArgs e)
    {
        AvaPlot.Multiplot.SharedAxes.ShareX([]);
        AvaPlot.Multiplot.SharedAxes.ShareY([]);
        AvaPlot.Refresh();
    }

    private void OnShareXClick(object? sender, RoutedEventArgs e)
    {
        AvaPlot.Multiplot.SharedAxes.ShareX(AvaPlot.Multiplot.GetPlots());
        AvaPlot.Multiplot.SharedAxes.ShareY([]);
        AvaPlot.Refresh();
    }

    private void OnShareYClick(object? sender, RoutedEventArgs e)
    {
        AvaPlot.Multiplot.SharedAxes.ShareX([]);
        AvaPlot.Multiplot.SharedAxes.ShareY(AvaPlot.Multiplot.GetPlots());
        AvaPlot.Refresh();
    }

    private void OnShareXYClick(object? sender, RoutedEventArgs e)
    {
        AvaPlot.Multiplot.SharedAxes.ShareX(AvaPlot.Multiplot.GetPlots());
        AvaPlot.Multiplot.SharedAxes.ShareY(AvaPlot.Multiplot.GetPlots());
        AvaPlot.Refresh();
    }
}
