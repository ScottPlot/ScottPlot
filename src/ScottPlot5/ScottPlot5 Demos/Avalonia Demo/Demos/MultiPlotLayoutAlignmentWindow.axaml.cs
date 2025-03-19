using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot;

namespace Avalonia_Demo.Demos;

public class MultiPlotLayoutAlignmentDemo : IDemo
{
    public string Title => "Multiplot Layout Alignment";
    public string Description => "Plots automatically resize the data area to accommodate tick labels of varying length, " +
        "but this may lead to misaligned data areas in multiplot figures. Using a fixed padding resolves this issue.";

    public Window GetWindow()
    {
        return new MultiPlotLayoutAlignmentWindow();
    }
}

public partial class MultiPlotLayoutAlignmentWindow : Window
{
    public MultiPlotLayoutAlignmentWindow()
    {
        InitializeComponent();

        // setup a multiplot with 2 subplots
        AvaPlot.Multiplot.AddPlots(2);
        Plot plot1 = AvaPlot.Multiplot.GetPlot(0);
        Plot plot2 = AvaPlot.Multiplot.GetPlot(1);

        // add sample data to each subplot
        plot1.Add.Signal(Generate.RandomWalk(100, mult: 1));
        plot2.Add.Signal(Generate.RandomWalk(100, mult: 10_000));
    }

    private void OnDefaultClick(object? sender, RoutedEventArgs e)
    {
        foreach (var plot in AvaPlot.Multiplot.GetPlots())
            plot.Layout.Default();

        AvaPlot.Refresh();
    }

    private void OnFixedPaddingClick(object? sender, RoutedEventArgs e)
    {
        PixelPadding padding = new(left: 60, right: 10, bottom: 30, top: 10);
        foreach (var plot in AvaPlot.Multiplot.GetPlots())
            plot.Layout.Fixed(padding);

        AvaPlot.Refresh();
    }
}
