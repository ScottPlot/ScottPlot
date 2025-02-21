using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot;
using System;

namespace Avalonia_Demo.Demos;

public class CustomContextMenuDemo : IDemo
{
    public string Title => "Custom Right-Click Context Menu";

    public string Description => "Demonstrates how to replace the default " +
        "right-click menu with a user-defined one that performs custom actions.";

    public Window GetWindow()
    {
        return new CustomContextMenuWindow();
    }

}

public partial class CustomContextMenuWindow : Window
{
    public CustomContextMenuWindow()
    {
        InitializeComponent();

        SetCustomContextMenu();
    }

    private void OnDefaultButtonClicked(object? sender, RoutedEventArgs e)
    {
        SetDefaultContextMenu();
    }

    private void OnCustomButtonClicked(object? sender, RoutedEventArgs e)
    {
        SetCustomContextMenu();
    }

    private void SetDefaultContextMenu()
    {
        // Reset menu to default options
        AvaPlot.Menu?.Reset();

        AvaPlot.Plot.Title("Default Right-Click Menu");
        AvaPlot.Refresh();
    }

    private void SetCustomContextMenu()
    {
        // clear existing menu items
        AvaPlot.Menu?.Clear();

        // add menu items with custom actions
        AvaPlot.Menu?.Add("Add Scatter", (plot) =>
        {
            plot.Add.Scatter(Generate.RandomCoordinates(5));
            plot.Axes.AutoScale();
            plot.PlotControl?.Refresh();
        });

        AvaPlot.Menu?.Add("Add Line", (plot) =>
        {
            var line = plot.Add.Line(Generate.RandomLine());
            line.LineWidth = 2;
            line.MarkerSize = 20;
            plot.Axes.AutoScale();
            plot.PlotControl?.Refresh();
        });

        AvaPlot.Menu?.Add("Add Text", (plot) =>
        {
            var txt = plot.Add.Text("Test", Generate.RandomLocation());
            txt.LabelFontSize = 10 + Generate.RandomInteger(20);
            txt.LabelFontColor = Generate.RandomColor(128);
            txt.LabelBold = true;
            plot.Axes.AutoScale();
            plot.PlotControl?.Refresh();
        });

        AvaPlot.Menu?.AddSeparator();

        AvaPlot.Menu?.Add("Clear", (plot) =>
        {
            plot.Clear();
            plot.Axes.AutoScale();
            plot.PlotControl?.Refresh();
        });

        AvaPlot.Plot.Title("Custom Right-Click Menu");
        AvaPlot.Refresh();
    }
}
